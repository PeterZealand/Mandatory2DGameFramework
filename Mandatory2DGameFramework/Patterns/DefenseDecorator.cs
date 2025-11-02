using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    public abstract class DefenseDecorator : WorldObject, IDefenseItem
    {
        protected readonly IDefenseItem Enchanted;
        public virtual int DefenseValue => Enchanted.DefenseValue;

        protected DefenseDecorator(IDefenseItem enchanted)
        {
            Enchanted = enchanted;
            if (enchanted is WorldObject obj)
                Name = obj.Name;
        }

        public abstract int ReduceDamage(int incoming);
    }
}
