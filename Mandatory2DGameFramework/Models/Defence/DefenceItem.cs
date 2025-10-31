using Mandatory2DGameFramework.Interfaces;
using Mandatory2DGameFramework.worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.model.defence
{
    public class DefenceItem : IDefenceItem
    {
        public String Name { get; set; }
        public int Defense { get; set; }

        public DefenceItem()
        {
            Name = string.Empty;
            Defense = 0;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Defense)}={Defense.ToString()}}}";
        }
    }
}
