using Mandatory2DGameFramework.Models;
using Mandatory2DGameFramework.Interfaces;
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
    /// TODO skal være abstract?
    public abstract class GameObjectFactory
    {
        public abstract ICreature CreateCreature();

        public abstract IAttackItem CreateAttackItem();

        public abstract IDefenseItem CreateDefenseItem();
    }
}
