using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    public class AttackItem : IWorldObject,  IAttackItem
    {
        public int Damage { get; set; }
        public string Name { get; set; }
        public bool Lootable { get; set; }
        public bool Removable { get; set; }

        public AttackItem()
        {
            Name = string.Empty;
            Damage = 0;
            Lootable = true;
            Removable = true;

        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Damage)}={Damage.ToString()}}}";
        }
    }
}
