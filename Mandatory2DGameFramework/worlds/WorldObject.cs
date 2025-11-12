using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandatory2DGameFramework.Interfaces;

namespace Mandatory2DGameFramework.Worlds
{

    //TODO ret på klassen da den overlapper med interfaces nu.
   /// <summary>
   /// Represents an abstract base class for objects in the game world, providing common properties and behavior.
   /// </summary>
   /// <remarks>The <see cref="WorldObject"/> class serves as a foundation for all objects that exist within
   /// the game world. It includes properties such as <see cref="Name"/>, <see cref="Lootable"/>, and <see
   /// cref="Removable"/> to define the object's characteristics. Equality and comparison operations are based on the
   /// <see cref="Name"/> property, allowing objects with the same name to be considered equal.</remarks>
    public abstract class WorldObject : IWorldObject
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
        //tjekker nu game identity ikke objekt referance
        //TODO giver nu problemer med fx Wall, der skal kunne være flere ens objekter i verden af, måske adde andre faktorer som x + y ?
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
