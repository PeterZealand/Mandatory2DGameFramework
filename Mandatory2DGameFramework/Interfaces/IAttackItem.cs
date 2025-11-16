using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Represents an item that can deal damage.</summary>
    public interface IAttackItem : IWorldObject
    {
        /// <summary>Damage this item gives when equipped.</summary>
        int Damage { get; set; }
    }
}
