using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Models;
using Mandatory2DGameFramework.worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    public class Armor : WorldObject, IDefenceItem
    {
        public int DefenseValue { get; set; }

        public Armor()
        {
            Name = string.Empty;
            DefenseValue = 0;
        }
    }
}
