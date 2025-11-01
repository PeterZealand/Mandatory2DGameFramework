using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Worlds
{

    //TODO ret på klassen da den overlapper med interfaces nu.
    public abstract class WorldObject
    {
        public string Name { get; set; }
        public bool Lootable { get; set; }
        public bool Removable { get; set; }

        public WorldObject()
        {
            Name = string.Empty;
            Lootable = false;
            Removable = false;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Lootable)}={Lootable.ToString()}, {nameof(Removable)}={Removable.ToString()}}}";
        }
    }
}
