using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Models;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    //TODO Armor og defense er næsten det samme, skal jeg have begge? skal armor kunnne mere? resists?
    public class Armor : IWorldObject, IDefenseItem, IPositionable
    {
        public int DefenseValue { get; set; }
        public string Name { get; set; }
        public bool Lootable { get; set; }
        public bool Removable { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Armor()
        {
            Name = string.Empty;
            DefenseValue = 0;
        }

        public int ReduceDamage(int incoming)
        {
            int reduce = incoming - DefenseValue;
            if (reduce < 0) reduce = 0;
            return reduce;
        }
    }
}
