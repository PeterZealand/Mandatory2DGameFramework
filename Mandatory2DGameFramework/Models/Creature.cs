using Mandatory2DGameFramework.Worlds;
using Mandatory2DGameFramework.Patterns;
using static Mandatory2DGameFramework.Patterns.GameLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandatory2DGameFramework.Interfaces;

namespace Mandatory2DGameFramework.Models
{
    // Todo consider how many attack / defense weapons are allowed
    //decided on 1 of each for simplicity
    //TODO skal creatuer overhoved have Lootable ? skal setup refraktureres til at creature ikke er worldobject? eller skal world object ændres så lootable ikke er med i base abstract class?
    //TODO skal creatures metoder være virtual? så brugere af systemet kan override dem og lave deres egne creature metoder som hit osv?

    /// <summary>
    /// Base creature that can attack, receive hits, loot items and notify observers.
    /// creature følger ikke SOLID den kan alt for meget og skal være template
    /// </summary>
    public abstract class Creature : IWorldObject, ICreature, IPositionable, IMove
    {
        /// <summary>Current health points of the creature.</summary>
        public int HitPoint { get; set; }

        /// <summary>Equipped attack item (optional).</summary>
        public IAttackItem? Attack { get; set; }

        /// <summary>Equipped defense item (optional).</summary>
        public IDefenseItem? Defense { get; set; }

        /// <summary>Display name of the creature.</summary>
        public string Name { get; set; }

        /// <summary>Whether the creature can be looted.</summary>
        public bool Lootable { get; set; }

        /// <summary>Whether the creature can be removed from the world.</summary>
        public bool Removable { get; set; }

        /// <summary>X coordinate in the world.</summary>
        public int X { get; set; }

        /// <summary>Y coordinate in the world.</summary>
        public int Y { get; set; }

        private readonly List<ICreatureObserver> _observer = new();

        /// <summary>Registers an observer for hit notifications.</summary>
        public void RegisterObserver(ICreatureObserver observer)
        {
            if (!_observer.Contains(observer))
            {
                _observer.Add(observer);
            }
        }

        /// <summary>Removes a previously registered observer.</summary>
        public void RemoveObserver(ICreatureObserver observer)
        {
            if (_observer.Contains(observer))
            {
                _observer.Remove(observer);
            }
        }

        /// <summary>Notifies observers that the creature took damage.</summary>
        /// <param name="damage">The final damage received.</param>
        public void NotifyObservers(int damage)
        {
            foreach (var observer in _observer)
            {
                observer.OnCreatureHit(Name, damage);
            }
        }

        /// <summary>Initializes a new creature with default values.</summary>
        protected Creature()
        {
            Name = string.Empty;
            HitPoint = 100;

            Attack = null;
            Defense = null;
        }

        /// <summary>Calculates and returns outgoing damage (5 when unarmed).</summary>
        public virtual int Hit()
        {
            int damage = Attack?.Damage ?? ICreature.UnarmedDamage;
            GameLogger.Instance.LogInfo($"{Name} attacks for {damage} damage{(Attack != null ? $" using {Attack.Name}" : " (unarmed)")}.");
            return damage;
        }

        /// <summary>Applies an incoming hit after defense reduction and notifies observers.</summary>
        /// <param name="hit">The incoming hit amount.</param>
        public virtual void ReceiveHit(int hit)
        {
            int reduction = Defense?.DefenseValue ?? 0;
            int DamageRecieved = hit - reduction;
            if (DamageRecieved < 0) DamageRecieved = 0;

            HitPoint -= DamageRecieved;
            if (HitPoint < 0) HitPoint = 0;

            GameLogger.Instance.LogInfo($"{Name} received {hit} damage, reduced by {reduction}, Damage Recieved {DamageRecieved}. Remaining HP: {HitPoint}.");
            NotifyObservers(DamageRecieved);
        }

        /// <summary>Moves the creature by dx/dy if within bounds and not blocked.</summary>
        public void Move(int dx, int dy, IWorld world)
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

        /// <summary>Attempts to equip better items from a world object if lootable.</summary>
        public void Loot(IWorldObject obj)
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
                        GameLogger.Instance.LogInfo($"{Name} equips new attack item '{attack.Name}' (Damage: {attack.Damage}).");
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
                        GameLogger.Instance.LogInfo($"{Name} equips new defense item '{obj.Name}' (Value: {newDefense.DefenseValue}).");
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
        {
            return $"{{{nameof(Name)}={Name}, {nameof(HitPoint)}={HitPoint.ToString()}, {nameof(Attack)}={Attack}, {nameof(Defense)}={Defense}}}";
        }
    }
}
