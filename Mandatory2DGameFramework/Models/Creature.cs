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
    /// <summary>
    /// Base creature that can attack, receive hits, loot items and notify observers.
    /// creature følger ikke SOLID den kan alt for meget og skal være template
    /// </summary>
    public abstract class Creature : IWorldObject, ICreature,IPositionable, IMove
    {
        public int HitPoint { get; set; }
        // Todo consider how many attack / defense weapons are allowed
        //decided on 1 of each for simplicity
        //TODO skal creatuer overhoved have Lootable ? skal setup refraktureres til at creature ikke er worldobject? eller skal world object ændres så lootable ikke er med i base abstract class?
        //TODO skal creatures metoder være virtual? så brugere af systemet kan override dem og lave deres egne creature metoder som hit osv?
        public IAttackItem?   Attack { get; set; }
        public IDefenseItem?  Defense { get; set; }
        public string Name { get; set; }
        public bool Lootable { get; set; }
        public bool Removable { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        private readonly List<ICreatureObserver> _observer = new();

        public void RegisterObserver(ICreatureObserver observer)
        {
            if (!_observer.Contains(observer))
            {
                _observer.Add(observer);
            }
        }

        public void RemoveObserver(ICreatureObserver observer)
        {
            if (_observer.Contains(observer))
            {
                _observer.Remove(observer);
            }
        }

        public void NotifyObservers(int damage)
        {
            foreach (var observer in _observer)
            {
                observer.OnCreatureHit(Name, damage);
            }
        }

        public Creature()
        {
            Name = string.Empty;
            HitPoint = 100;

            Attack = null;
            Defense = null;
        }

        public virtual int Hit()
        {
            // return the damage this creature produces (5 when unarmed)
            int damage = Attack?.Damage ?? ICreature.UnarmedDamage;
            GameLogger.Instance.LogInfo($"{Name} attacks for {damage} damage{(Attack != null ? $" using {Attack.Name}" : " (unarmed)")}.");
            return damage;
        }

        public virtual void ReceiveHit(int hit)
        {
            // reduce incoming hit by defense if present, clamp to non-negative and apply to HitPoint
            int reduction = Defense?.DefenseValue ?? 0;
            int DamageRecieved = hit - reduction;
            if (DamageRecieved < 0) DamageRecieved = 0;

            HitPoint -= DamageRecieved;
            if (HitPoint < 0) HitPoint = 0;

            Instance.LogInfo($"{Name} received {hit} damage, reduced by {reduction}, Damage Recieved {DamageRecieved}. Remaining HP: {HitPoint}.");
            NotifyObservers(DamageRecieved);
        }

        public void Move(int dx, int dy, World world)
        {
            int newX = X + dx;
            int newY = Y + dy;

            // Stay within bounds
            if (newX < 0 || newY < 0 || newX >= world.MaxX || newY >= world.MaxY)
            {
                GameLogger.Instance.LogWarning($"{Name} cannot move outside the world bounds!");
                return;
            }

            // Check for obstacles
            if (world.IsBlocked(newX, newY))
            {
                GameLogger.Instance.LogWarning($"{Name} cannot move to ({newX}, {newY}) — blocked by wall!");
                return;
            }

            X = newX;
            Y = newY;
            GameLogger.Instance.LogInfo($"{Name} moved to ({X}, {Y}).");
        }
        //TODO strategy pattern via interface eller action?
        public void Loot(WorldObject obj)
        {
            if (obj == null)
            {
                Instance.LogWarning($"{Name} tried to loot a null object.");
                return;
            }

            if (!obj.Lootable)
            {
                Instance.LogWarning($"{Name} attempted to loot '{obj.Name}', but it is not lootable.");
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

                case IDefenseItem newDefence:
                    if (Defense == null || newDefence.DefenseValue > Defense.DefenseValue)
                    {
                        GameLogger.Instance.LogInfo($"{Name} equips new defense item '{obj.Name}' (Value: {newDefence.DefenseValue}).");
                        Defense = newDefence as DefenseItem; // or store as IDefenseItem – see below
                        obj.Removable = true;
                    }
                    else
                    {
                        GameLogger.Instance.LogInfo($"{Name} ignores weaker defense item '{obj.Name}' (Value: {newDefence.DefenseValue}).");
                    }
                    break;
            }

        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(HitPoint)}={HitPoint.ToString()}, {nameof(Attack)}={Attack}, {nameof(Defense)}={Defense}}}";
        }
    }
}
