using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>
    /// Represents an object capable of movement within a world.
    /// </summary>
    public interface IMove
    {
        /// <summary>
        /// Moves the object by the given delta values (dx, dy).
        /// </summary>
        void Move(int dx, int dy, World world);
    }
}
