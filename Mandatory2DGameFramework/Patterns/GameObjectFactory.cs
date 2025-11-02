using Mandatory2DGameFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    /// <summary>
    /// Factory for creating game objects without hardcoded data.
    /// </summary>
    public static class GameObjectFactory
    {
        public static Creature CreateCreature<T>() where T : Creature, new()
            => new();

        public static AttackItem CreateAttackItem(string name, int damage)
            => new() { Name = name, Damage = damage };

        public static DefenseItem CreateDefenseItem(string name, int defense)
            => new() { Name = name, DefenseValue = defense };

        public static Armor CreateArmour(string name, int defense)
            => new() { Name = name, DefenseValue = defense };
    }
}
