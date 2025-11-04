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
    public class World :IWorld
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public List<WorldObject> WorldObjects { get; } = new();

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

        public void AddObject(WorldObject obj)
        {
            if (WorldObjects.Any(o => o == obj))
            {
                GameLogger.Instance.LogWarning($"Object '{obj.Name}' already exists in the world.");
                return;
            }

            WorldObjects.Add(obj);
            GameLogger.Instance.LogInfo($"Added '{obj.Name}' to the world.");
        }

        public void RemoveObject(WorldObject obj)
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
