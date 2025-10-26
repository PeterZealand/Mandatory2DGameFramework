using Mandatory2DGameFramework.worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models.Attack
{
    public class AttackItem : WorldObject
    {
        public string  Name { get; set; }
        public int Hit { get; set; }

        public AttackItem()
        {
            Name = string.Empty;
            Hit = 0;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Hit)}={Hit.ToString()}}}";
        }
    }
}
