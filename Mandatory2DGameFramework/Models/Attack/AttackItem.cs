using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models.Attack
{
    public class AttackItem : IAttackItem
    {
        public string  Name { get; set; }
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
