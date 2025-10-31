using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    public interface ICreatureObserver
    {
        void OnCreatureHit(string creatureName, int damage);
    }
}
