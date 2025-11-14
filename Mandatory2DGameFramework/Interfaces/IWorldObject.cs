using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Base contract for all objects existing in the world.</summary>
    public interface IWorldObject
    {
        string Name { get; set; }
        bool Lootable { get; set; }
        bool Removable { get; set; }
    }
}
