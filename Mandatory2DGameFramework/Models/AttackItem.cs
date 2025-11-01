using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    public class AttackItem : WorldObject,  IAttackItem
    {
        public int Damage { get; set; }

        public AttackItem()
        {
            Name = string.Empty;
            Damage = 0;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Damage)}={Damage.ToString()}}}";
        }
    }
}
