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

        // operator overload så man kan tjekke fx om Steel Sword == Steel Sword - tjekker på navn ikke objeckt i hukommelsen
        // ikke sikker på det er denne metode jeg vil overload, giver mening men er lidt kompliceret
        public static bool operator ==(WorldObject? a, WorldObject? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Name == b.Name;
        }

        public static bool operator !=(WorldObject? a, WorldObject? b) => !(a == b);

        public override bool Equals(object? obj) => obj is WorldObject w && w.Name == Name;
        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Lootable)}={Lootable.ToString()}, {nameof(Removable)}={Removable.ToString()}}}";
        }
    }
}
