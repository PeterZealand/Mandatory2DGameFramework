using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Object with world coordinates.</summary>
    public interface IPositionable
    {
        int X { get; set; }
        int Y { get; set; }
    }
}
