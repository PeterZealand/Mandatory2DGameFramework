using Mandatory2DGameFramework.Interfaces;

namespace Mandatory2DGameFramework.Models.CombatStrategies
{
    /// <summary>Slight reduction in attack power.</summary>
    public sealed class WeakenedCombatStrategy : CombatStrategyBase, ICombatStrategy
    {
        public override int CalculateAttackPower(ICreature creature, int baseDamage) => (int)(baseDamage * 0.9);
    }
}