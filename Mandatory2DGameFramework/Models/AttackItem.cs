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
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Lootable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Removable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
