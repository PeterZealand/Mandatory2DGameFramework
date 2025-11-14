using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    /// <summary>
    /// Represents an immovable, non-lootable barrier that blocks creature movement.
    /// </summary>
    internal class Wall : ImmovableObject, IWorldObject, IPositionable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public Wall(int x, int y)
        {
            Name = "Wall";
            X = x;
            Y = y;
        }

        public override string ToString() => $"Wall({X},{Y})";
    }
}
