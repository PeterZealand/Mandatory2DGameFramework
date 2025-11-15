using Mandatory2DGameFramework.Interfaces;

namespace Mandatory2DGameFramework.Models.CombatStrategies
{
    /// <summary>
    /// Strategy provider with injectable strategies and thresholds.
    /// healthRatio thresholds: > weakenedThreshold => optimal,
    /// > defensiveThreshold => weakened, else defensive.
    /// </summary>
    public sealed class DefaultCombatStrategyProvider : ICombatStrategyProvider
    {
        private readonly ICombatStrategy _optimal;
        private readonly ICombatStrategy _weakened;
        private readonly ICombatStrategy _defensive;
        private readonly double _weakenedThreshold;
        private readonly double _defensiveThreshold;

        public DefaultCombatStrategyProvider(
            ICombatStrategy optimal,
            ICombatStrategy weakened,
            ICombatStrategy defensive,
            double weakenedThreshold = 0.60,
            double defensiveThreshold = 0.30)
        {
            _optimal = optimal;
            _weakened = weakened;
            _defensive = defensive;
            _weakenedThreshold = weakenedThreshold;
            _defensiveThreshold = defensiveThreshold;
        }

        public ICombatStrategy Select(ICreature creature, double healthRatio)
        {
            if (healthRatio > _weakenedThreshold) return _optimal;
            if (healthRatio > _defensiveThreshold) return _weakened;
            return _defensive;
        }
    }
}