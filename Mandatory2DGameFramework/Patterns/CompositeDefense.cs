using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    /// <summary>
    /// Combines multiple IdefenseItem objects into one logical defense unit.
    /// </summary>
    /// 

    //TODO
    //Skal jeg have armor med så jeg kan bruge en composite class nu jeg kun kan have et defense item på af gangen?
    //eller skal items kunne forbedres?
    // forsøger med Armor
    public class CompositeDefense : IDefenseItem
    {
        public string Name { get; set; } = "Composite defense";
        private readonly List<IDefenseItem> _defenses = new();

        public void Add(IDefenseItem defense) => _defenses.Add(defense);

        public int DefenseValue
        {
            get
            {
                int total = 0;
                foreach (var d in _defenses)
                    total += d.DefenseValue;
                return total;
            }
        }

        public int ReduceDamage(int incoming)
        {
            int reduced = incoming;
            foreach (var d in _defenses)
                reduced = d.ReduceDamage(reduced);
            GameLogger.Instance.LogInfo($"{Name} reduced damage to {reduced}");
            return reduced;
        }
    }
}
