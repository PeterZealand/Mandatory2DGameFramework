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
    // NOTE: Acts as a template/root; World enforces rules, Creature initiates intent.

    /// <summary>
    /// Framework creature implementation using only interfaces (no concrete class coupling).
    /// Handles intent (move / fight); delegates rule enforcement to <see cref="IWorld"/>.
    /// </summary>
    public abstract class Creature : ICreature, IPositionable, IMovable
    {
        /// <summary>Display name.</summary>
        public string Name { get; set; } = "Unnamed Creature";

        /// <summary>Whether this creature can be looted after defeat.</summary>
        public bool Lootable { get; set; } = false;

        /// <summary>Whether this creature should be removed from world.</summary>
        public bool Removable { get; set; } = true;

        /// <summary>Current hit points.</summary>
        public int HitPoint { get; set; } = 100;

        /// <summary>Maximum hit points (used for percentage calculations).</summary>
        public int MaxHitPoint { get; protected set; } = 100;

        /// <summary>Unarmed base damage (fallback when no attack item).</summary>
        public int UnarmedDamage { get; set; } = 5;

        /// <summary>X coordinate.</summary>
        public int X { get; set; }

        /// <summary>Y coordinate.</summary>
        public int Y { get; set; }

        /// <summary>Equipped attack item (optional).</summary>
        public IAttackItem? Attack { get; set; }

        /// <summary>Equipped defense item (optional).</summary>
        public IDefenseItem? Defense { get; set; }

        /// <summary>Active combat strategy (selected via provider when fighting).</summary>
        public ICombatStrategy? CombatStrategy { get; private set; }

        private readonly ICombatStrategyProvider _strategyProvider;
        private readonly List<ICreatureObserver> _observers = new();

        /// <summary>
        /// Primary constructor for dependency injection.
        /// </summary>
        protected Creature(ICombatStrategyProvider strategyProvider)
        {
            _strategyProvider = strategyProvider;
            MaxHitPoint = HitPoint;
        }

        #region Observer
        /// <summary>Registers a damage observer.</summary>
        public void RegisterObserver(ICreatureObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        /// <summary>Removes a previously registered observer.</summary>
        public void RemoveObserver(ICreatureObserver observer)
        {
            if (_observers.Contains(observer))
                _observers.Remove(observer);
        }

        /// <summary>Notifies observers that damage was taken.</summary>
        protected void NotifyObservers(int damage)
        {
            foreach (var o in _observers)
                o.OnCreatureHit(Name, damage);
        }
        #endregion

        #region Movement (Intent)
        /// <summary>
        /// Creature intent to move; actual resolution (blocking, combat, loot) is handled by <see cref="IWorld.MoveCreature"/>.
        /// </summary>
        public virtual void Move(int dx, int dy, IWorld world)
        {
            int targetX = X + dx;
            int targetY = Y + dy;

            if (!world.MoveCreature(this, targetX, targetY))
            {
                GameLogger.Instance.LogWarning($"{Name} could not move to ({targetX},{targetY}).");
            }
        }
        #endregion

        #region Combat
        /// <summary>
        /// Calculates outgoing raw damage before opponent mitigation.
        /// </summary>
        protected virtual int CalculateAttackDamage()
        {
            int baseDamage = Attack?.Damage ?? UnarmedDamage;
            var strategy = _strategyProvider.Select(this, HealthRatio());
            CombatStrategy = strategy;
            return strategy.CalculateAttackPower(this, baseDamage);
        }

        /// <summary>
        /// Applies incoming damage after defense & strategy mitigation.
        /// </summary>
        protected virtual void ApplyDamage(int rawIncoming)
        {
            int afterDefense = Defense?.ReduceDamage(rawIncoming) ?? rawIncoming;
            if (afterDefense < 0) afterDefense = 0;

            var strategy = _strategyProvider.Select(this, HealthRatio());
            int final = strategy.CalculateDamageTaken(this, afterDefense);

            HitPoint -= final;
            if (HitPoint < 0) HitPoint = 0;

            GameLogger.Instance.LogInfo($"{Name} took {final} damage (raw {rawIncoming}, defense -> {afterDefense}). HP {HitPoint}/{MaxHitPoint}.");
            NotifyObservers(final);

            if (HitPoint == 0)
            {
                Removable = true;
                GameLogger.Instance.LogInfo($"{Name} has been defeated.");
            }
        }

        /// <summary>
        /// Public combat routine between this creature and an opponent.
        /// World triggers this when two creatures collide.
        /// </summary>
        public virtual void Fight(ICreature opponent)
        {
            if (opponent == null || opponent.HitPoint <= 0 || HitPoint <= 0)
                return;

            OnBeforeFight(opponent);

            // Attacker damage
            int myDamage = CalculateAttackDamage();

            // Opponent damage
            var opponentStrategy = _strategyProvider.Select(opponent, OpponentHealthRatio(opponent));
            int opponentBase = (opponent is Creature oc && oc.Attack != null) ? oc.Attack.Damage : opponent.UnarmedDamage;
            int opponentAttackPower = opponentStrategy.CalculateAttackPower(opponent, opponentBase);

            // Apply results
            if (opponent is Creature concreteOpponent)
                concreteOpponent.ApplyDamage(myDamage);
            else
                opponent.HitPoint -= myDamage; // fallback (no defense mitigation if not concrete)

            ApplyDamage(opponentAttackPower);

            GameLogger.Instance.LogInfo($"{Name} dealt {myDamage} to {opponent.Name}; {opponent.Name} dealt {opponentAttackPower} to {Name}.");

            OnAfterFight(opponent, myDamage, opponentAttackPower);

            CombatStrategy?.OnCombatEnd(this);
        }

        /// <summary>Hook executed before fight resolution.</summary>
        protected virtual void OnBeforeFight(ICreature opponent) { }

        /// <summary>Hook executed after fight resolution.</summary>
        protected virtual void OnAfterFight(ICreature opponent, int dealt, int received) { }

        private double HealthRatio() => MaxHitPoint > 0 ? (double)HitPoint / MaxHitPoint : 0.0;
        private double OpponentHealthRatio(ICreature c) => c is Creature cc && cc.MaxHitPoint > 0
            ? (double)cc.HitPoint / cc.MaxHitPoint
            : 0.0;
        #endregion

        #region Loot
        /// <summary>
        /// Attempts to improve equipment based on a lootable world object.
        /// </summary>
        public virtual void Loot(IWorldObject obj)
        {
            if (obj == null)
            {
                GameLogger.Instance.LogWarning($"{Name} tried to loot null.");
                return;
            }

            if (!obj.Lootable)
            {
                GameLogger.Instance.LogWarning($"{Name} attempted to loot '{obj.Name}' but it is not lootable.");
                return;
            }

            switch (obj)
            {
                case IAttackItem attack:
                    if (Attack == null || attack.Damage > Attack.Damage)
                    {
                        Attack = attack;
                        obj.Removable = true;
                        GameLogger.Instance.LogInfo($"{Name} equipped attack item '{attack.Name}' (Damage {attack.Damage}).");
                    }
                    else
                    {
                        GameLogger.Instance.LogInfo($"{Name} ignored weaker attack item '{attack.Name}'.");
                    }
                    break;

                case IDefenseItem defense:
                    if (Defense == null || defense.DefenseValue > Defense.DefenseValue)
                    {
                        Defense = defense;
                        obj.Removable = true;
                        GameLogger.Instance.LogInfo($"{Name} equipped defense item '{defense.Name}' (Value {defense.DefenseValue}).");
                    }
                    else
                    {
                        GameLogger.Instance.LogInfo($"{Name} ignored weaker defense item '{defense.Name}'.");
                    }
                    break;
            }
        }
        #endregion

        public override string ToString()
            => $"{Name} HP:{HitPoint}/{MaxHitPoint} Pos({X},{Y}) Atk:{Attack?.Damage ?? UnarmedDamage} Def:{Defense?.DefenseValue ?? 0}";
    }
}
