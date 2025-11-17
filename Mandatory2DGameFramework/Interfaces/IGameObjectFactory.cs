using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Worlds;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>
    /// Abstraction for creating framework objects. Users of the library can implement this
    /// to plug in custom creation logic (e.g. randomized stats, difficulty scaling, DI).
    /// </summary>
    public interface IGameObjectFactory
    {
        ICreature CreateCreature();
        IAttackItem CreateAttackItem();
        IDefenseItem CreateDefenseItem();
        IImmovable CreateImmovableWorldObject();
        IWorld CreateWorld(int Width, int Height);
    }
}