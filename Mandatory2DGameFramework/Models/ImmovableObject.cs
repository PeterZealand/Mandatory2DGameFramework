using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandatory2DGameFramework.Worlds;

namespace Mandatory2DGameFramework.Models
{
    /// <summary>
    /// Base class for all immovable world objects.
    /// Automatically sets Lootable and Removable to false.
    /// </summary>
    public abstract class ImmovableObject : WorldObject, IImmovable
    {
        public int X { get; set; }
        public int Y { get; set; }
        protected ImmovableObject()
        {
            Lootable = false;
            Removable = false;
        }


    }
}
