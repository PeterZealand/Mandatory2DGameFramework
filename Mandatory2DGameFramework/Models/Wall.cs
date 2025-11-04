using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    internal class Wall : IWorldObject, IPositionable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        //se på at lave wall til kun get da den ikke skal være setable men altid være false
        public bool Lootable { get; set; }
        public bool Removable { get; set; }

        public Wall(int x, int y)
        {
            Name = "Wall";
            Lootable = false;
            Removable = false;
            X = x;
            Y = y;
        }

        public override string ToString() => $"Wall at ({X}, {Y})";
    }
}
