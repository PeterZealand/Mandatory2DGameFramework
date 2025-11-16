using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    /// <summary>Concrete defense item.</summary>
    public abstract class DefenseItem : IDefenseItem, IPositionable
    {
        public int DefenseValue { get; set; }
        public string Name { get; set; }
        public bool Lootable { get; set; }
        public bool Removable { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public DefenseItem()
        {
            Name = string.Empty;
            DefenseValue = 0;
        }

        public override string ToString()
            => $"{{{nameof(Name)}={Name}, {nameof(DefenseValue)}={DefenseValue}}}";

        public virtual int ReduceDamage(int incoming)
        {
            int remaining = incoming - DefenseValue;
            return remaining < 0 ? 0 : remaining;
        }
    }
}
