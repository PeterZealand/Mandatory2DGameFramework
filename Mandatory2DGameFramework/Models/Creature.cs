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
    //      eller skal world object ændres så lootable ikke er med i base abstract class?
    // TODO skal creatures metoder være virtual? så brugere af systemet kan override dem og lave deres egne creature metoder som Hit osv?
    // NOTE: This class acts as a template/root for concrete creatures (may be split for stricter SOLID later).

    /// <summary>
    /// Base creature that can attack, receive hits, loot items and notify observers.
    /// Now DIP-compliant for combat behavior via <see cref="ICombatStrategyProvider"/> + <see cref="ICombatStrategy"/>.
    /// </summary>
    public abstract class Creature : IWorldObject, ICreature, IPositionable, IMove
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
        /// Primary constructor for dependency injection.
        /// </summary>
        /// <param name="strategyProvider">Provides appropriate combat strategies based on health ratio.</param>
        /// <param name="maxHitPoint">Initial and maximum hit points.</param>
        protected Creature(ICombatStrategyProvider strategyProvider, int maxHitPoint = 100)
        {
            _strategyProvider = strategyProvider;
            Name = string.Empty;
            MaxHitPoint = maxHitPoint;
            HitPoint = maxHitPoint;

            Attack = null;
            Defense = null;
            Lootable = false;
            Removable = false;

            CombatStrategy = SelectStrategy();
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
            int baseDamage = Attack?.Damage ?? creature.UnarmedDamage;
            int damage = CombatStrategy.CalculateAttackPower(this, baseDamage);
            GameLogger.Instance.LogInfo($"{Name} attacks for {damage} damage{(Attack != null ? $" using {Attack.Name}" : " (unarmed)")} ({CombatStrategy.GetType().Name}).");
            return damage;
        }

        /// <summary>
        /// Applies incoming hit: reduce by defense, then adjust through combat strategy, update HP, observers and strategy.
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

        //TODO heal metode hvis jeg vil have healing i spillet
        /// <summary>
        /// Heals the creature (clamped to MaxHitPoint) and refreshes strategy if threshold crossing occurs.
        /// </summary>
        /// <param name="amount">Positive healing amount.</param>
        //public virtual void Heal(int amount)
        //{
        //    if (amount <= 0 || HitPoint <= 0) return;
        //    HitPoint += amount;
        //    if (HitPoint > MaxHitPoint) HitPoint = MaxHitPoint;
        //    GameLogger.Instance.LogInfo($"{Name} heals {amount}. HP: {HitPoint}/{MaxHitPoint}.");
        //    RefreshStrategyIfChanged();
        //}

        ///// <summary>
        ///// Template Method: Executes a full attack turn against a target.
        ///// Non-virtual; override the protected hooks to customize behavior.
        ///// </summary>
        //public virtual void PerformAttackTurn(ICreature target)
        //{
        //    OnBeforeAttack(target);
        //    int dmg = Hit(target);
        //    target.ReceiveHit(dmg);
        //    OnAfterAttack(target, dmg);
        //    CombatStrategy.OnCombatEnd(this);
        //}

        /// <summary>Hook: called before an attack; override to inject behavior.</summary>
        protected virtual void OnBeforeAttack(Creature target) { }

        /// <summary>Hook: called after an attack; override to inject behavior.</summary>
        protected virtual void OnAfterAttack(Creature target, int dealtDamage) { }
        #endregion

        #region Movement
        /// <summary>Moves the creature by (dx, dy) if inside world bounds and not blocked.</summary>
        public virtual void Move(int dx, int dy, IWorld world)
        {
            int newX = X + dx;
            int newY = Y + dy;

            if (newX < 0 || newY < 0 || newX >= world.MaxX || newY >= world.MaxY)
            {
                GameLogger.Instance.LogWarning($"{Name} cannot move outside the world bounds!");
                return;
            }

            if (world.IsBlocked(newX, newY))
            {
                GameLogger.Instance.LogWarning($"{Name} cannot move to ({newX}, {newY}) — blocked by wall!");
                return;
            }

            X = newX;
            Y = newY;
            GameLogger.Instance.LogInfo($"{Name} moved to ({X}, {Y}).");
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
        /// <summary>Returns a debug string for the creature.</summary>
        public override string ToString()
            => $"{{Name={Name}, HP={HitPoint}/{MaxHitPoint}, Attack={Attack?.Damage ?? UnarmedDamage}, Defense={Defense?.DefenseValue ?? 0}, Strategy={CombatStrategy?.GetType().Name}}}";
    }
}
