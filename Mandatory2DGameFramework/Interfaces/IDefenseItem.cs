using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    public interface IDefenseItem
    {
        int DefenseValue { get; }

        int ReduceDamage(int incoming);
    }
}
