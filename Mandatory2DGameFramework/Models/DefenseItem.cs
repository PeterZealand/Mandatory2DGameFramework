using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    public class DefenseItem : IDefenseItem
    {
        public int DefenseValue { get; set; }
        public string Name { get; set; }
        public bool Lootable { get; set; }
        public bool Removable { get; set; }

        public DefenseItem()
        {
            Name = string.Empty;
            DefenseValue = 0;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(DefenseValue)}={DefenseValue.ToString()}}}";
        }

        public int ReduceDamage(int incoming)
        {
            int reduce = incoming - DefenseValue;
            if (reduce < 0) reduce = 0;
            return reduce;
        }
    }
}
