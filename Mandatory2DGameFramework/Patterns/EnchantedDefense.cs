using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mandatory2DGameFramework.Patterns
{
    /// <summary>
    /// Decorator adding a flat bonus to defense.
    /// </summary>
    public class EnchantedDefense : DefenseDecorator
    {
        private readonly int _bonus;

        /// <summary>
        /// Creates a new enchanted defense, enhancing an existing defense item.
        /// </summary>
        /// <param name="enchanted">The defense item being magically enhanced.</param>
        /// <param name="bonus">The defense bonus applied by the enchantment (default +5).</param>
        public EnchantedDefense(IDefenseItem enchanted, int bonus = 5)
            : base(enchanted)
        {
            _bonus = bonus;
            Name += " (Enchanted)";
        }

        public override int DefenseValue => Enchanted.DefenseValue + _bonus;

        public override int ReduceDamage(int incoming)
        {
            int remaining = incoming - DefenseValue;
            return remaining < 0 ? 0 : remaining;
        }
    }
}
