using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    public interface ICreatureObserver
    {
        void OnCreatureHit(string creatureName, int damage);
    }
}
