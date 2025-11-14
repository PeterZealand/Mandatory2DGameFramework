using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Represents a creature in the world.</summary>
    public interface ICreature : IWorldObject
    {
        /// <summary>Base damage dealt when unarmed.</summary>
        const int UnarmedDamage = 5;
        /// <summary>Current hit points.</summary>
        int HitPoint { get; set; }
    }
}
