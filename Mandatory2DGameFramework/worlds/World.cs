using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Models;
using Mandatory2DGameFramework.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Worlds
{
    /// <summary>
    /// Represents the game world and tracks all world objects.
    /// </summary>
    public class World : IWorld
    {
        public int Width { get; }
        public int Height { get; }

        public List<IWorldObject> WorldObjects { get; }

        public World(int width, int height)
        {
            Width = width;
            Height = height;
            WorldObjects = new List<IWorldObject>();
        }

        /// <summary>
        /// Adds an object to the world if it does not violate collision rules.
        /// </summary>
        public void AddObject(IWorldObject obj)
        {
            if (obj is IPositionable pos)
            {
                // Check for occupants in same cell
                var existing = WorldObjects
                    .OfType<IPositionable>()
                    .FirstOrDefault(o => o.X == pos.X && o.Y == pos.Y);

                if (existing != null)
                {
                    // BLOCK: IImmovable objects cannot be shared
                    if (existing is IImmovable)
                    {
                        GameLogger.Instance.LogWarning(
                            $"Cannot place '{obj.Name}' at ({pos.X},{pos.Y}) — space is blocked by '{existing.Name}'."
                        );
                        return;
                    }
                }
            }

            WorldObjects.Add(obj);
            GameLogger.Instance.LogInfo($"Added '{obj.Name}' to the world.");
        }

        /// <summary>
        /// Attempts to move a creature in the world.
        /// </summary>
        public bool MoveCreature(ICreature creature, int newX, int newY)
        {
            if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
            {
                GameLogger.Instance.LogWarning("Creature tried to move out of bounds.");
                return false;
            }

            var occupant = WorldObjects
                .OfType<IPositionable>()
                .FirstOrDefault(o => o.X == newX && o.Y == newY);

            if (occupant != null)
            {
                // BLOCK: IImmovable cannot be passed through
                if (occupant is IImmovable)
                {
                    GameLogger.Instance.LogWarning(
                        $"Cannot move into '{occupant.Name}' at ({newX},{newY}) — space is blocked."
                    );
                    return false;
                }

                // COMBAT: if another creature is here, they fight
                if (occupant is ICreature otherCreature)
                {
                    GameLogger.Instance.LogInfo(
                        $"{creature.Name} encountered {otherCreature.Name} at ({newX},{newY}). Combat begins!"
                    );

                    creature.Fight(otherCreature);

                    // Remove defeated creatures
                    if (otherCreature.HitPoint <= 0)
                        WorldObjects.Remove(otherCreature);

                    if (creature.HitPoint <= 0)
                        WorldObjects.Remove(creature);

                    return false;
                }

                // LOOT: lootable objects are removed after pickup
                if (occupant is IWorldObject wo && wo.Lootable)
                {
                    GameLogger.Instance.LogInfo($"{creature.Name} picks up {wo.Name}.");
                    WorldObjects.Remove(wo);
                }
            }

            // Move the creature
            if (creature is IPositionable movable)
            {
                movable.X = newX;
                movable.Y = newY;
                GameLogger.Instance.LogInfo($"{creature.Name} moved to ({newX},{newY}).");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes an object if marked removable.
        /// </summary>
        public void RemoveObject(IWorldObject obj)
        {
            if (obj.Removable)
            {
                WorldObjects.Remove(obj);
                GameLogger.Instance.LogInfo($"Removed '{obj.Name}' from the world.");
            }
            else
            {
                GameLogger.Instance.LogWarning(
                    $"Attempted to remove '{obj.Name}' but it is not removable."
                );
            }
        }
    }
}
