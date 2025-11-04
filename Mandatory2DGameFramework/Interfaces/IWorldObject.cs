using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    public interface IWorldObject
    {
        string Name { get; set; }
        bool Lootable { get; set; }
        bool Removable { get; set; }
    }
}
