using Mandatory2DGameFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    //TODO
    //kan eventuelt være at hit fungerer anderledes når du er såret?

    /// <summary>
    /// Defines an abstraction for combat behavior.
    /// </summary>
    public interface ICombatStrategy
    {
        int CalculateAttackPower(Creature creature);
        int CalculateDamageTaken(Creature creature, int incomingDamage);
        void OnCombatEnd(Creature creature);
    }
}
