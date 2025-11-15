using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    /// <summary>Base combat strategy (pass-through defaults).</summary>
    public abstract class CombatStrategyBase : ICombatStrategy
    {
        public virtual int CalculateAttackPower(ICreature creature, int baseDamage) => baseDamage;
        public virtual int CalculateDamageTaken(ICreature creature, int incomingDamage) => incomingDamage < 0 ? 0 : incomingDamage;
        public virtual void OnCombatEnd(ICreature creature) { }
    }
}
