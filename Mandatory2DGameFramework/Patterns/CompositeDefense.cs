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
    public abstract class CompositeDefense : IDefenseItem
    {
        public string Name { get; set; } = "Composite Defense";
        public bool Lootable { get; set; }
        public bool Removable { get; set; }
        private readonly List<IDefenseItem> _defenses = new();

        // Expose read-only view for diagnostics/printing
        public IReadOnlyList<IDefenseItem> Items => _defenses.AsReadOnly();

        public virtual void Add(IDefenseItem defense) => _defenses.Add(defense);

        public virtual int DefenseValue => _defenses.Sum(d => d.DefenseValue);

        public virtual int ReduceDamage(int incoming)
        {
            int reduced = incoming;
            foreach (var d in _defenses)
                reduced = d.ReduceDamage(reduced);
            GameLogger.Instance.LogInfo($"{Name} reduced damage to {reduced}");
            return reduced;
        }

        public override string ToString()
        {
            var parts = _defenses.Select(d => $"{d.Name}:{d.DefenseValue}");
            return $"{Name} (Total DEF: {DefenseValue}) [{string.Join(" + ", parts)}]";
        }
    }
}
