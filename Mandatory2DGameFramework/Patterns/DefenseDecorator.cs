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
    /// Abstract decorator for enhancing defence items dynamically.
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
            if (enchanted is WorldObject obj)
                Name = obj.Name;
        }

        public abstract int ReduceDamage(int incoming);
    }
}
