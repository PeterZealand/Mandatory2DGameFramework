using Mandatory2DGameFramework.Models;
using Mandatory2DGameFramework.Patterns;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandatory2DGameFramework.Interfaces;

namespace Mandatory2DGameFramework.Interfaces
{
    public interface IWorld
    {
        int Width { get; set; }
        int Height { get; set; }
        bool IsBlocked(int x, int y);
        void AddObject(IWorldObject obj);
        void RemoveObject(IWorldObject obj);
    }
}
