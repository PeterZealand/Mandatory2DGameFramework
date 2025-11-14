using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    //TODO
    //kan eventuelt være at hit fungerer anderledes når du er såret?

    /// <summary>Defines an action for a creature (Strategy pattern).</summary>
    public interface ICreatureStrategy
    {
        /// <summary>Executes the configured creature action.</summary>
        void PerformAction();
    }
}
