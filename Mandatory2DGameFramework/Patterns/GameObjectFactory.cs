using Mandatory2DGameFramework.Models;
using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    /// <summary>
    /// Base factory implementing <see cref="IGameObjectFactory"/>.
    /// Extend this abstract class or implement the interface directly to customize object creation.
    /// </summary>
    public abstract class GameObjectFactory : IGameObjectFactory
    {
        /// <summary>Create a creature instance (override for custom stats).</summary>
        public abstract ICreature CreateCreature();

        /// <summary>Create an attack item (override for custom stats).</summary>
        public abstract IAttackItem CreateAttackItem();

        /// <summary>Create a defense item (override for custom stats).</summary>
        public abstract IDefenseItem CreateDefenseItem();

        /// <summary>Create an immovable world object (e.g. wall, obstacle).</summary>
        public abstract IImmovable CreateImmovableWorldObject();

        /// <summary>Create a world. Default implementation returns a basic derived World; override for terrain generation.</summary>
        public abstract IWorld CreateWorld(int Width, int );
    }
}
