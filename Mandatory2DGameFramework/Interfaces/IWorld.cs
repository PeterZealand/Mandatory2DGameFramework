using Mandatory2DGameFramework.Models;
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
        int MaxX { get; set; }
        int MaxY { get; set; }
        public void AddWorldObject(WorldObject obj);
        public void AddCreature(Creature creature); 
    }
}
