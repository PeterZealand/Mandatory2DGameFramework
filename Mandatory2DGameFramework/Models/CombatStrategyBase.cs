using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    /// <summary>
    /// Base class defining how a creature behaves in combat.
    /// Implements ICombatStrategy for full dependency inversion.
    /// </summary>
    public abstract class CombatStrategyBase : ICombatStrategy
    {
        public virtual int CalculateAttackPower(Creature creature)
        {
            double healthRatio = (double)creature.Health / creature.MaxHealth;

            if (healthRatio >= 0.8)
                return creature.BaseAttackPower;
            else if (healthRatio >= 0.5)
                return (int)(creature.BaseAttackPower * 0.9);
            else
                return (int)(creature.BaseAttackPower * 0.8);
        }

        public virtual int CalculateDamageTaken(Creature creature, int incomingDamage)
        {
            double healthRatio = (double)creature.Health / creature.MaxHealth;

            if (healthRatio < 0.5)
                incomingDamage = (int)(incomingDamage * 0.9);

            return incomingDamage;
        }

        public virtual void OnCombatEnd(Creature creature)
        {
            // Default behavior: no adaptation
        }
    }
}
