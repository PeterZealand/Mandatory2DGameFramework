using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    /// <summary>
    /// Combines multiple IDefenceItem objects into one logical defence unit.
    /// </summary>
    /// 

    //TODO
    //Skal jeg have armor med så jeg kan bruge en composite class nu jeg kun kan have et defense item på af gangen?
    //eller skal items kunne forbedres?
    // forsøger med Armor
    public class CompositeDefence : IDefenceItem
    {
        public string Name { get; set; } = "Composite Defence";
        private readonly List<IDefenceItem> _defences = new();

        public void Add(IDefenceItem defence) => _defences.Add(defence);

        public int DefenseValue
        {
            get
            {
                int total = 0;
                foreach (var d in _defences)
                    total += d.DefenseValue;
                return total;
            }
        }

        public int ReduceDamage(int incoming)
        {
            int reduced = incoming;
            foreach (var d in _defences)
                reduced = d.ReduceDamage(reduced);
            GameLogger.Instance.LogInfo($"{Name} reduced damage to {reduced}");
            return reduced;
        }
    }
}
