using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>
    /// Marker interface for world objects that cannot be moved.
    /// Inherit from IWorldObject (+ IPositionable if they have coordinates)
    /// so an immovable can be passed directly to methods expecting IWorldObject.
    /// </summary>
    public interface IImmovable : IWorldObject, IPositionable
    {
    }
}
