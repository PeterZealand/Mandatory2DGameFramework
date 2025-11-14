using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    /// <summary>
    /// Base decorator for dynamically enhancing defense items.
    /// </summary>
    public abstract class DefenseDecorator : IDefenseItem
    {
        protected readonly IDefenseItem Enchanted;
        public virtual int DefenseValue => Enchanted.DefenseValue;

        public string Name { get; set; }
        public bool Lootable { get; set; }
        public bool Removable { get; set; }

        protected DefenseDecorator(IDefenseItem enchanted)
        {
            Enchanted = enchanted;
            Name = enchanted.Name;
            Lootable = enchanted.Lootable;
            Removable = enchanted.Removable;
        }

        public abstract int ReduceDamage(int incoming);
    }
}
