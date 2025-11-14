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
    public abstract class World :IWorld
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public List<IWorldObject> WorldObjects { get; } = new();

        public World(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
        }

        /// <summary>
        /// Checks if a coordinate is blocked (e.g. by a Wall).
        /// </summary>
        public bool IsBlocked(int x, int y)
        {
            return WorldObjects.OfType<IPositionable>()
                          .Any(o => o.X == x && o.Y == y && o is Wall);
        }

        public void AddObject(IWorldObject obj)
        {
            // Check for duplicates by name (using operator overload)
            if (WorldObjects.Any(o => o == obj))
            {
                GameLogger.Instance.LogWarning($"Object '{obj.Name}' already exists in the world.");
                return;
            }

            // Check for overlapping positions if applicable
            if (obj is IPositionable pos)
            {
                bool occupied = WorldObjects
                    .OfType<IPositionable>()
                    .Any(o => o.X == pos.X && o.Y == pos.Y);

                if (occupied)
                {
                    GameLogger.Instance.LogWarning(
                        $"Cannot place '{obj.Name}' at ({pos.X}, {pos.Y}) — space already occupied."
                    );
                    return;
                }
            }

            // Enforce immovable rules - commented out for now - dont think its needed or correct here
            //if (obj is IImmovable)
            //{
            //    obj.Lootable = false;
            //    obj.Removable = false;
            //    GameLogger.Instance.LogInfo($"'{obj.Name}' is immovable and cannot be looted or removed.");
            //}

            WorldObjects.Add(obj);
            GameLogger.Instance.LogInfo($"Added '{obj.Name}' to the world.");
        }

        public void RemoveObject(IWorldObject obj)
        {
            if (WorldObjects.Contains(obj))
            {
                WorldObjects.Remove(obj);
                GameLogger.Instance.LogInfo($"Removed '{obj.Name}' from the world.");
            }
        }

        public override string ToString()
        {
            return $"{{{nameof(MaxX)}={MaxX.ToString()}, {nameof(MaxY)}={MaxY.ToString()}, ObjectsCount={WorldObjects.Count}}}";
        }
        //public int MaxX { get; set; }
        //public int MaxY { get; set; }
        //// world objects
        //private List<WorldObject> _worldObjects;
        //// world creatures
        //private List<Creature> _creatures;

        //public World(int maxX, int maxY)
        //{
        //    MaxX = maxX;
        //    MaxY = maxY;
        //    _worldObjects = new List<WorldObject>();
        //    _creatures = new List<Creature>();
        //}

        //public void AddWorldObject(WorldObject obj)
        //{
        //    _worldObjects.Add(obj);
        //}

        //public void AddCreature(Creature creature)
        //{
        //    _creatures.Add(creature);
        //}

        //public override string ToString()
        //{
        //    return $"{{{nameof(MaxX)}={MaxX.ToString()}, {nameof(MaxY)}={MaxY.ToString()}}}";
        //}
    }
}
