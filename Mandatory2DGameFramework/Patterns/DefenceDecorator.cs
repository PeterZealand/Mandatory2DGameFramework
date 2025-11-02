using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    public abstract class DefenceDecorator : IDefenceItem
    {
        protected readonly IDefenceItem Enchanted;
        public int DefenseValue => Enchanted.DefenseValue;

        protected DefenceDecorator(IDefenceItem enchanted)
        {
            Enchanted = enchanted;
            //TODO The decorator itself should carry over the base Name - doesnt work atm
            if (enchanted is WorldObject obj)
                Name = obj.Name;
        }

        public abstract int ReduceDamage(int incoming);
    }
}
