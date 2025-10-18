using Mandatory2DGameFramework.model.Cretures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.worlds
{
    public class World
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }


        // world objects
        private List<WorldObject> _worldObjects;
        // world creatures
        private List<Creature> _creatures;

        public World(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            _worldObjects = new List<WorldObject>();
            _creatures = new List<Creature>();
        }

        public override string ToString()
        {
            return $"{{{nameof(MaxX)}={MaxX.ToString()}, {nameof(MaxY)}={MaxY.ToString()}}}";
        }
    }
}
