using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Represents an item that reduces incoming damage.</summary>
    public interface IDefenseItem : IWorldObject
    {
        /// <summary>Total defense value provided.</summary>
        int DefenseValue { get; }
        /// <summary>Returns the remaining damage after applying defense.</summary>
        int ReduceDamage(int incoming);
    }
}
