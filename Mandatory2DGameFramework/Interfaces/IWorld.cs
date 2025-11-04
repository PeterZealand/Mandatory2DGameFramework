using Mandatory2DGameFramework.Models;
using Mandatory2DGameFramework.Patterns;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    public interface IWorld
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public bool IsBlocked(int x, int y);
        public void AddObject(WorldObject obj);
        public void RemoveObject(WorldObject obj);
    }
}
