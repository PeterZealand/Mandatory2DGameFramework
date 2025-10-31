using Mandatory2DGameFramework.model.Cretures;
using Mandatory2DGameFramework.model.defence;
using Mandatory2DGameFramework.Models.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    public static class GameObjectFactory
    {
        public static Creature CreateCreature<T>() where T : Creature, new()
            => new();

        public static AttackItem CreateAttackItem(string name, int damage)
            => new() { Name = name, Damage = damage };

        public static DefenceItem CreateDefenceItem(string name, int defence)
            => new() { Name = name, Defense = defence };
    }
}
