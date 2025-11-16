using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Represents a creature in the world.</summary>
    public interface ICreature : IWorldObject, IPositionable
    {
        /// <summary>Base damage dealt when unarmed.</summary>
        int UnarmedDamage { get; set; }
        /// <summary>Current hit points.</summary>
        int HitPoint { get; set; }
    }
}
