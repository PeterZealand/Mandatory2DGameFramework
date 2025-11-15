using Mandatory2DGameFramework.Interfaces;

namespace Mandatory2DGameFramework.Models.CombatStrategies
{
    /// <summary>Lower attack, some mitigation to incoming damage.</summary>
    public sealed class DefensiveCombatStrategy : CombatStrategyBase, ICombatStrategy
    {
        public override int CalculateAttackPower(ICreature creature, int baseDamage) => (int)(baseDamage * 0.8);
        public override int CalculateDamageTaken(ICreature creature, int incomingDamage)
            => incomingDamage < 0 ? 0 : (int)(incomingDamage * 0.9);
    }
}