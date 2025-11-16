using Mandatory2DGameFramework.Worlds;
using Mandatory2DGameFramework.Patterns;
using static Mandatory2DGameFramework.Patterns.GameLogger;
using System.Collections.Generic;
using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Models.CombatStrategies;

namespace Mandatory2DGameFramework.Models
{
    // TODO consider how many attack / defense weapons are allowed (currently 1 of each for simplicity)
    // TODO skal creature overhoved have Lootable ? skal setup refaktureres til at creature ikke er worldobject?
    // TODO skal creatures metoder være virtual? så brugere af systemet kan override dem og lave deres egne creature metoder som Hit osv?
    // NOTE: This class acts as a template/root for concrete creatures (may be split for stricter SOLID later).

    /// <summary>
    /// Base creature that can attack, receive hits, loot items and notify observers.
    /// Now DIP-compliant for combat behavior via <see cref="ICombatStrategyProvider"/> + <see cref="ICombatStrategy"/>.
    /// Framework creature implementation using only interfaces (no concrete class coupling).
    /// Handles intent (move / fight); delegates rule enforcement to <see cref="IWorld"/>.
    /// </summary>
    public abstract class Creature : IWorldObject, ICreature, IPositionable, IMovable
    {
        /// <summary>Current health points of the creature.</summary>
        public int HitPoint { get; set; }

        /// <summary>Maximum health points (used to compute health ratio for strategy selection).</summary>
        public int MaxHitPoint { get; set; }

        /// <summary>Base damage dealt when unarmed.</summary>
        public int UnarmedDamage { get; set; }

        /// <summary>Equipped attack item (optional). Unarmed if null (uses <see cref="ICreature.UnarmedDamage"/>).</summary>
        public IAttackItem? Attack { get; set; }

        /// <summary>Equipped defense item (optional). Damage reduction depends on its <see cref="IDefenseItem.DefenseValue"/>.</summary>
        public IDefenseItem? Defense { get; set; }

        /// <summary>Display name of the creature.</summary>
        public string Name { get; set; }

        /// <summary>Whether the creature can be looted.</summary>
        public bool Lootable { get; set; }

        /// <summary>Whether the creature can be removed from the world (e.g. when defeated).</summary>
        public bool Removable { get; set; }

        /// <summary>X coordinate in the world.</summary>
        public int X { get; set; }

        /// <summary>Y coordinate in the world.</summary>
        public int Y { get; set; }

        /// <summary>Current combat strategy selected for this creature (may change with HP).</summary>
        public ICombatStrategy CombatStrategy { get; private set; }

        private readonly List<ICreatureObserver> _observer = new();
        private readonly ICombatStrategyProvider _strategyProvider;

        /// <summary>
        /// Preferred base constructor. Forces all important settings to be supplied at creation time.
        /// </summary>
        /// <param name="strategyProvider">Required. Provides the combat strategy based on current HP ratio.</param>
        /// <param name="name">Creature display name (e.g. "Goblin").</param>
        /// <param name="maxHitPoint">Max HP (also used as initial HP). Must be &gt; 0.</param>
        /// <param name="unarmedDamage">Damage dealt when no weapon is equipped. Must be &gt; 0.</param>
        /// <param name="startX">Start X position.</param>
        /// <param name="startY">Start Y position.</param>
        /// <param name="lootable">Whether this creature can be looted.</param>
        /// <param name="removable">Whether this creature can be removed from the world after defeat.</param>
        /// <param name="startingAttack">Optional starting attack item (weapon).</param>
        /// <param name="startingDefense">Optional starting defense item (armor/shield).</param>
        /// <remarks>
        /// Usage in a game demo (in your concrete creature class):
        /// <code>
        /// public sealed class DemoCreature : Creature
        /// {
        ///     public DemoCreature(ICombatStrategyProvider provider)
        ///         : base(
        ///             strategyProvider: provider,
        ///             name: "Goblin",
        ///             maxHitPoint: 100,
        ///             unarmedDamage: 5,
        ///             startX: 10,
        ///             startY: 5,
        ///             lootable: false,
        ///             removable: true,
        ///             startingAttack: new AttackItem { Name = "Dagger", Damage = 8, Lootable = true, Removable = true },
        ///             startingDefense: new DefenseItem { Name = "Hide Shield", Lootable = true, Removable = true, DefenseValue = 3 }
        ///         )
        ///     {
        ///     }
        /// }
        /// </code>
        /// Make sure you pass a valid <see cref="ICombatStrategyProvider"/> (e.g. <see cref="DefaultCombatStrategyProvider"/>).
        /// </remarks>
        protected Creature(
            ICombatStrategyProvider strategyProvider,
            string name,
            int maxHitPoint,
            int unarmedDamage,
            int startX,
            int startY,
            bool lootable = false,
            bool removable = true,
            IAttackItem? startingAttack = null,
            IDefenseItem? startingDefense = null)
        {
            _strategyProvider = strategyProvider ?? throw new ArgumentNullException(nameof(strategyProvider), "Provide an ICombatStrategyProvider (e.g., DefaultCombatStrategyProvider).");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Creature name must be provided.", nameof(name));
            if (maxHitPoint <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHitPoint), "MaxHitPoint must be > 0.");
            if (unarmedDamage <= 0)
                throw new ArgumentOutOfRangeException(nameof(unarmedDamage), "UnarmedDamage must be > 0.");

            Name = name;

            MaxHitPoint = maxHitPoint;
            HitPoint = maxHitPoint;

            UnarmedDamage = unarmedDamage;

            X = startX;
            Y = startY;

            Lootable = lootable;
            Removable = removable;

            Attack = startingAttack;
            Defense = startingDefense;

            CombatStrategy = SelectStrategy();

            GameLogger.Instance.LogInfo($"Creature '{Name}' created at ({X},{Y}) HP {HitPoint}/{MaxHitPoint}, Atk {(Attack?.Damage ?? UnarmedDamage)}, Def {Defense?.DefenseValue ?? 0}.");
        }

        #region Observer Pattern
        /// <summary>Registers an observer for hit notifications.</summary>
        public void RegisterObserver(ICreatureObserver observer)
        {
            if (!_observer.Contains(observer))
                _observer.Add(observer);
        }

        /// <summary>Removes a previously registered observer.</summary>
        public void RemoveObserver(ICreatureObserver observer)
        {
            if (_observer.Contains(observer))
                _observer.Remove(observer);
        }

        /// <summary>Notifies observers that the creature took damage.</summary>
        /// <param name="damage">Final damage applied to HP.</param>
        protected void NotifyObservers(int damage)
        {
            foreach (var observer in _observer)
                observer.OnCreatureHit(Name, damage);
        }
        #endregion

        #region Strategy Selection
        /// <summary>Determines the correct strategy according to current HP ratio.</summary>
        private ICombatStrategy SelectStrategy()
        {
            double ratio = MaxHitPoint > 0 ? (double)HitPoint / MaxHitPoint : 0;
            return _strategyProvider.Select(this, ratio);
        }

        /// <summary>Updates the active strategy if the health ratio bracket changed.</summary>
        private void RefreshStrategyIfChanged()
        {
            var newStrategy = SelectStrategy();
            if (!ReferenceEquals(newStrategy, CombatStrategy))
            {
                CombatStrategy = newStrategy;
                GameLogger.Instance.LogInfo($"{Name} strategy changed to {CombatStrategy.GetType().Name} (HP {HitPoint}/{MaxHitPoint}).");
            }
        }
        #endregion

        #region Combat
        /// <summary>
        /// Calculates outgoing damage using weapon (if any) or unarmed damage, then passes through combat strategy.
        /// </summary>
        /// <returns>Final attack power.</returns>
        public virtual int Hit(ICreature creature)
        {
            // Use THIS creature's unarmed damage when no weapon is equipped.
            int baseDamage = Attack?.Damage ?? UnarmedDamage;
            int damage = CombatStrategy.CalculateAttackPower(this, baseDamage);
            GameLogger.Instance.LogInfo($"{Name} attacks for {damage} damage{(Attack != null ? $" using {Attack.Name}" : " (unarmed)")} ({CombatStrategy.GetType().Name}).");
            return damage;
        }

        /// <summary>
        /// Applies incoming hit: defense mitigation then strategy mitigation, updates HP and notifies observers.
        /// </summary>
        /// <param name="hit">Raw incoming damage before mitigation.</param>
        public virtual void ReceiveHit(int hit)
        {
            int afterDefense = Defense?.ReduceDamage(hit) ?? hit;
            if (afterDefense < 0) afterDefense = 0;

            int finalDamage = CombatStrategy.CalculateDamageTaken(this, afterDefense);

            HitPoint -= finalDamage;
            if (HitPoint < 0) HitPoint = 0;

            GameLogger.Instance.LogInfo($"{Name} received {hit} incoming, defense -> {afterDefense}, strategy -> {finalDamage}. Remaining HP: {HitPoint}/{MaxHitPoint}.");
            NotifyObservers(finalDamage);

            RefreshStrategyIfChanged();

            if (HitPoint == 0)
            {
                GameLogger.Instance.LogInfo($"{Name} is defeated.");
                Removable = true;
            }
        }

        /// <summary>
        /// Performs a simple two-way combat exchange with the opponent:
        /// this creature hits first, and if the opponent survives,
        /// it retaliates. Both sides use their current combat strategy.
        /// </summary>
        /// <param name="opponent">The opponent creature.</param>
        public virtual void Fight(ICreature opponent)
        {
            if (opponent == null || ReferenceEquals(opponent, this))
            {
                GameLogger.Instance.LogWarning($"{Name} attempted to fight an invalid opponent.");
                return;
            }

            GameLogger.Instance.LogInfo($"{Name} engages {opponent.Name} in combat.");

            // This creature attacks first
            int myDamage = Hit(opponent);
            opponent.ReceiveHit(myDamage);

            // If the opponent is still alive, it retaliates
            if (opponent.HitPoint > 0)
            {
                int enemyDamage = opponent.Hit(this);
                ReceiveHit(enemyDamage);
            }
        }
        #endregion

        #region Movement
        /// <summary>
        /// Requests a move by (dx, dy); actual rule enforcement is delegated to the world
        /// via <see cref="IWorld.MoveCreature(ICreature,int,int)"/>.
        /// </summary>
        public virtual void Move(int dx, int dy, IWorld world)
        {
            if (world == null)
            {
                GameLogger.Instance.LogWarning($"{Name} cannot move without a valid world reference.");
                return;
            }

            int newX = X + dx;
            int newY = Y + dy;

            bool moved = world.MoveCreature(this, newX, newY);
            if (!moved)
            {
                GameLogger.Instance.LogInfo($"{Name}'s move to ({newX},{newY}) was blocked or consumed (e.g. by combat).");
            }
        }
        #endregion

        #region Looting
        /// <summary>
        /// Attempts to equip better items from a world object if it is lootable.
        /// Replaces current equipment only if the new item is stronger.
        /// </summary>
        public virtual void Loot(IWorldObject obj)
        {
            if (obj == null)
            {
                GameLogger.Instance.LogWarning($"{Name} tried to loot a null object.");
                return;
            }

            if (!obj.Lootable)
            {
                GameLogger.Instance.LogWarning($"{Name} attempted to loot '{obj.Name}', but it is not lootable.");
                return;
            }

            switch (obj)
            {
                case IAttackItem attack:
                    if (Attack == null || attack.Damage > Attack.Damage)
                    {
                        GameLogger.Instance.LogInfo($"{Name} equips attack item '{attack.Name}' (Damage: {attack.Damage}).");
                        Attack = attack;
                        obj.Removable = true;
                    }
                    else
                    {
                        GameLogger.Instance.LogInfo($"{Name} ignores weaker attack item '{attack.Name}' (Damage: {attack.Damage}).");
                    }
                    break;

                case IDefenseItem newDefense:
                    if (Defense == null || newDefense.DefenseValue > Defense.DefenseValue)
                    {
                        GameLogger.Instance.LogInfo($"{Name} equips defense item '{obj.Name}' (Value: {newDefense.DefenseValue}).");
                        Defense = newDefense;
                        obj.Removable = true;
                    }
                    else
                    {
                        GameLogger.Instance.LogInfo($"{Name} ignores weaker defense item '{obj.Name}' (Value: {newDefense.DefenseValue}).");
                    }
                    break;
            }
        }
        #endregion

        /// <summary>Returns a debug string for the creature.</summary>
        public override string ToString()
            => $"{{Name={Name}, HP={HitPoint}/{MaxHitPoint}, Attack={Attack?.Damage ?? UnarmedDamage}, Defense={Defense?.DefenseValue ?? 0}, Strategy={CombatStrategy?.GetType().Name}}}";
    }
}
