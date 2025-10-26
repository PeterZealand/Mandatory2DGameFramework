using Mandatory2DGameFramework.model.defence;
using Mandatory2DGameFramework.Models.Attack;
using Mandatory2DGameFramework.worlds;
using Mandatory2DGameFramework.Patterns;
using static Mandatory2DGameFramework.Patterns.GameLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.model.Cretures
{
    public class Creature
    {
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
            // return the damage this creature produces (0 when unarmed)
            int damage = Attack?.Hit ?? 0;
            GameLogger.Instance.LogInfo($"{Name} attacks for {damage} damage{(Attack != null ? $" using {Attack.Name}" : " (unarmed)") }.");
            return damage;
        }

        public void ReceiveHit(int hit)
        {
            // reduce incoming hit by defence if present, clamp to non-negative and apply to HitPoint
            int reduction = Defence?.ReduceHitPoint ?? 0;
            int net = hit - reduction;
            if (net < 0) net = 0;

            HitPoint -= net;
            if (HitPoint < 0) HitPoint = 0;

            GameLogger.Instance.LogInfo($"{Name} received {hit} damage, reduced by {reduction}, net {net}. Remaining HP: {HitPoint}.");
        }

        public void Loot(WorldObject obj)
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
                case AttackItem attack:
                    {
                        var previous = Attack;
                        Attack = attack;
                        obj.Removeable = true;
                        GameLogger.Instance.LogInfo($"{Name} looted attack item '{attack.Name}' (Hit={attack.Hit}, Range={attack.Range}). Replaced '{previous?.Name ?? "none"}'.");
                        break;
                    }
                case DefenceItem defence:
                    {
                        var previous = Defence;
                        Defence = defence;
                        obj.Removeable = true;
                        GameLogger.Instance.LogInfo($"{Name} looted defence item '{defence.Name}' (ReduceHitPoint={defence.ReduceHitPoint}). Replaced '{previous?.Name ?? "none"}'.");
                        break;
                    }
                default:
                    {
                        // generic lootable object
                        obj.Removeable = true;
                        GameLogger.Instance.LogInfo($"{Name} looted object '{obj.Name}'.");
                        break;
                    }
            }
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(HitPoint)}={HitPoint.ToString()}, {nameof(Attack)}={Attack}, {nameof(Defence)}={Defence}}}";
        }
    }
}
