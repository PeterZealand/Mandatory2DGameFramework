using Mandatory2DGameFramework.Models;
using Mandatory2DGameFramework.Patterns;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandatory2DGameFramework.Interfaces;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>
    /// Represents a game world that manages objects and enforces rules
    /// such as bounds, collisions, combat and looting.
    /// </summary>
    public interface IWorld
    {
        /// <summary>World width (X max exclusive).</summary>
        int Width { get; set; }

        /// <summary>World height (Y max exclusive).</summary>
        int Height { get; set; }

        /// <summary>All world objects (creatures, items, immovables).</summary>
        List<IWorldObject> WorldObjects { get; }

        /// <summary>Adds an object to the world if placement rules allow it.</summary>
        void AddObject(IWorldObject obj);

        /// <summary>Removes an object if it is marked removable.</summary>
        void RemoveObject(IWorldObject obj);

        /// <summary>
        /// Returns true if the given coordinate is blocked by an immovable object.
        /// </summary>
        bool IsBlocked(int x, int y);

        /// <summary>
        /// Unified resolution of movement, combat and looting.
        /// Returns true only if the creature ends its turn in the new cell.
        /// </summary>
        /// <param name="creature">The creature attempting to move.</param>
        /// <param name="newX">Target X coordinate.</param>
        /// <param name="newY">Target Y coordinate.</param>
        /// <returns>True if the creature ended up in the new cell (pure movement), false if blocked or consumed by combat.</returns>
        bool MoveCreature(ICreature creature, int newX, int newY);
    }
}
