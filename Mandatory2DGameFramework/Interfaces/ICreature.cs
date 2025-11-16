using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Represents a creature in the world.</summary>
    public interface ICreature : IWorldObject, IPositionable
    {
        /// <summary>Base damage dealt when unarmed.</summary>
        int UnarmedDamage { get; set; }

        /// <summary>Current hit points.</summary>
        int HitPoint { get; set; }

        /// <summary>
        /// Attempts to move the creature by (dx, dy) within the given world.
        /// The world is responsible for enforcing bounds, collisions, combat and looting.
        /// </summary>
        /// <param name="dx">Delta X.</param>
        /// <param name="dy">Delta Y.</param>
        /// <param name="world">World that enforces rules for the move.</param>
        void Move(int dx, int dy, IWorld world);

        /// <summary>
        /// Calculates outgoing attack damage against a target creature,
        /// typically using its current combat strategy and equipment.
        /// </summary>
        /// <param name="target">The creature being attacked.</param>
        /// <returns>Final attack power.</returns>
        int Hit(ICreature target);

        /// <summary>
        /// Applies incoming damage to this creature, including defense and combat strategy effects.
        /// </summary>
        /// <param name="hit">Raw incoming damage.</param>
        void ReceiveHit(int hit);

        /// <summary>
        /// Performs a combat exchange with another creature (e.g. attack and possible retaliation).
        /// </summary>
        /// <param name="opponent">The opponent creature.</param>
        void Fight(ICreature opponent);

        /// <summary>
        /// Attempts to loot and possibly equip items from a world object.
        /// </summary>
        /// <param name="obj">World object to loot.</param>
        void Loot(IWorldObject obj);
    }
}
