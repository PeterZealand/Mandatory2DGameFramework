using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Observer notified when a creature takes damage.</summary>
    public interface ICreatureObserver
    {
        /// <summary>Called after a creature has received damage.</summary>
        void OnCreatureHit(string creatureName, int damage);
    }
}
