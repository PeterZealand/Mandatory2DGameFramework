using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Patterns;
using System.Collections.Generic;
using System.Linq;
using static Mandatory2DGameFramework.Patterns.GameLogger;

namespace Mandatory2DGameFramework.Models
{
    /// <summary>
    /// Central world managing objects and enforcing rules:
    /// bounds, collision, combat and looting.
    /// Pure interface usage (no concrete type dependency).
    /// </summary>
    public class World : IWorld
    {
        /// <summary>World width (X max exclusive).</summary>
        public int Width { get; set; }

        /// <summary>World height (Y max exclusive).</summary>
        public int Height { get; set; }

        /// <summary>All world objects (creatures, items, immovables).</summary>
        public List<IWorldObject> WorldObjects { get; } = new();

        public World(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Adds an object to the world if placement rules allow it.
        /// </summary>
        public void AddObject(IWorldObject obj)
        {
            if (obj is IPositionable pos)
            {
                var existing = WorldObjects
                    .OfType<IPositionable>()
                    .FirstOrDefault(o => o.X == pos.X && o.Y == pos.Y);

                if (existing is IImmovable)
                {
                    GameLogger.Instance.LogWarning(
                        $"Cannot place '{obj.Name}' at ({pos.X},{pos.Y}) — space blocked by immovable.");
                    return;
                }
            }

            WorldObjects.Add(obj);
            GameLogger.Instance.LogInfo($"Added '{obj.Name}' to world.");
        }

        /// <summary>
        /// Removes an object if it is marked removable.
        /// </summary>
        public void RemoveObject(IWorldObject obj)
        {
            if (obj.Removable)
            {
                if (WorldObjects.Remove(obj))
                    GameLogger.Instance.LogInfo($"Removed '{obj.Name}'.");
            }
            else
            {
                GameLogger.Instance.LogWarning($"Tried to remove '{obj.Name}' but it is not removable.");
            }
        }

        /// <summary>
        /// Checks if coordinate is blocked by an immovable object.
        /// </summary>
        public bool IsBlocked(int x, int y)
            => WorldObjects
                .OfType<IPositionable>()
                .Any(o => o.X == x && o.Y == y && o is IImmovable);

        /// <summary>
        /// Unified resolution of movement, combat and looting.
        /// Returns true only if the creature ended its turn in the new cell (i.e. pure movement).
        /// </summary>
        public bool MoveCreature(ICreature creature, int newX, int newY)
        {
            // Bounds
            if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
            {
                GameLogger.Instance.LogWarning($"{creature.Name} tried to move out of bounds to ({newX},{newY}).");
                return false;
            }

            // Occupant check
            var occupant = WorldObjects
                .OfType<IPositionable>()
                .FirstOrDefault(o => o.X == newX && o.Y == newY);

            if (occupant != null)
            {
                // BLOCKING
                if (occupant is IImmovable)
                {
                    GameLogger.Instance.LogWarning($"{creature.Name} cannot move into immovable '{occupant}'.");
                    return false;
                }

                // COMBAT
                if (occupant is ICreature other)
                {
                    GameLogger.Instance.LogInfo($"{creature.Name} engages {other.Name} at ({newX},{newY}).");

                    creature.Fight(other);

                    RemoveIfDefeated(creature);
                    RemoveIfDefeated(other);

                    return false; // Combat consumes move
                }

                // LOOTING
                if (occupant is IWorldObject wo && wo.Lootable)
                {
                    GameLogger.Instance.LogInfo($"{creature.Name} loots '{wo.Name}'.");
                    creature.Loot(wo);
                    WorldObjects.Remove(wo);
                }
            }

            // Perform movement
            if (creature is IPositionable movable)
            {
                movable.X = newX;
                movable.Y = newY;
                GameLogger.Instance.LogInfo($"{creature.Name} moved to ({newX},{newY}).");
                return true;
            }

            return false;
        }

        private void RemoveIfDefeated(ICreature c)
        {
            if (c.HitPoint <= 0 && WorldObjects.Contains(c))
            {
                c.Removable = true;
                WorldObjects.Remove(c);
                GameLogger.Instance.LogInfo($"Removed defeated creature '{c.Name}'.");
            }
        }
    }
}