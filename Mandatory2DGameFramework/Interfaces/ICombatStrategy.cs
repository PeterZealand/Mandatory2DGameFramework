using Mandatory2DGameFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>DIP abstraction for combat behavior. Uses ICreature to avoid concrete coupling.</summary>
    public interface ICombatStrategy
    {
        /// <summary>Given creature and base damage (weapon or unarmed), returns final attack power.</summary>
        int CalculateAttackPower(ICreature creature, int baseDamage);

        /// <summary>Given post-defense incoming damage, returns final damage taken.</summary>
        int CalculateDamageTaken(ICreature creature, int incomingDamage);

        /// <summary>Hook called after a combat exchange.</summary>
        void OnCombatEnd(ICreature creature);
    }
}
