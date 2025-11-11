using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>
    /// Represents an object that can exist at a specific X/Y position in the world.
    /// </summary>
    public interface IPositionable
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
