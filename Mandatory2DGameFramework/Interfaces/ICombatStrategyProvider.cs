namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Provides the proper combat strategy for the given creature and its current health ratio.</summary>
    public interface ICombatStrategyProvider
    {
        /// <param name="creature">The creature needing a strategy.</param>
        /// <param name="healthRatio">A value between 0 and 1 (current HP / max HP).</param>
        ICombatStrategy Select(ICreature creature, double healthRatio);
    }
}