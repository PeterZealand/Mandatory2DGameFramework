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
    public class Creature : ICreatureStrategy
    {
        private const int UnarmedDamage = 5;

        public string Name { get; set; }
        public int HitPoint { get; set; }


        // Todo consider how many attack / defence weapons are allowed
        public AttackItem?   Attack { get; set; }
        public DefenceItem?  Defence { get; set; }

        public Creature()
        {
            Name = string.Empty;
            HitPoint = 100;

            Attack = null;
            Defence = null;
        }

        public int Hit()
        {
            // return the damage this creature produces (5 when unarmed)
            int damage = Attack?.Damage ?? UnarmedDamage;
            Instance.LogInfo($"{Name} attacks for {damage} damage{(Attack != null ? $" using {Attack.Name}" : " (unarmed)") }.");
            return damage;
        }

        public void ReceiveHit(int hit)
        {
            // reduce incoming hit by defence if present, clamp to non-negative and apply to HitPoint
            int reduction = Defence?.DefenseValue ?? 0;
            int DamageRecieved = hit - reduction;
            if (DamageRecieved < 0) DamageRecieved = 0;

            HitPoint -= DamageRecieved;
            if (HitPoint < 0) HitPoint = 0;

            Instance.LogInfo($"{Name} received {hit} damage, reduced by {reduction}, Damage Recieved {DamageRecieved}. Remaining HP: {HitPoint}.");
        }

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
                case AttackItem attack:
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

                case DefenceItem defence:
                    if (Defence == null || defence.DefenseValue > Defence.DefenseValue)
                    {
                        GameLogger.Instance.LogInfo($"{Name} equips new defence item '{defence.Name}' (Value: {defence.DefenseValue}).");
                        Defence = defence;
                        obj.Removable = true;
                    }
                    else
                    {
                        GameLogger.Instance.LogInfo($"{Name} ignores weaker defence item '{defence.Name}' (Value: {defence.DefenseValue}).");
                    }
                    break;
            }

        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(HitPoint)}={HitPoint.ToString()}, {nameof(Attack)}={Attack}, {nameof(Defence)}={Defence}}}";
        }

        public void PerformAction()
        {
            throw new NotImplementedException();
        }
    }
}
