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
    public interface IMovable
    {
        /// <summary>
        /// Moves the object by the given delta values (dx, dy) within the provided world.
        /// </summary>
        /// <param name="dx">Delta X.</param>
        /// <param name="dy">Delta Y.</param>
        /// <param name="world">World context for bounds and collision checks.</param>
        void Move(int dx, int dy, IWorld world);
    }
}
