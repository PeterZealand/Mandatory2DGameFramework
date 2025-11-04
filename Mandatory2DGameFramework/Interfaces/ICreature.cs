using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    public interface ICreature : IWorldObject
    {
        int UnarmedDamage { get; set; }
        public int HitPoint { get; set; }
    }
}
