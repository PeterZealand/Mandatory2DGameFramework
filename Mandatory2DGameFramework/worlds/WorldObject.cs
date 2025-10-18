using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.worlds
{
    public class WorldObject
    {
        public string Name { get; set; }
        public bool Lootable { get; set; }
        public bool Removeable { get; set; }

        public WorldObject()
        {
            Name = string.Empty;
            Lootable = false;
            Removeable = false;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Lootable)}={Lootable.ToString()}, {nameof(Removeable)}={Removeable.ToString()}}}";
        }
    }
}
