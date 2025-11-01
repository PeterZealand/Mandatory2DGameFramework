using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    public class DefenceItem : WorldObject, IDefenceItem
    {
        public int DefenseValue { get; set; }

        public DefenceItem()
        {
            Name = string.Empty;
            DefenseValue = 0;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(DefenseValue)}={DefenseValue.ToString()}}}";
        }
    }
}
