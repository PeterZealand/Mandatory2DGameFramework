using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    public class GameLogger
    {
        private static GameLogger instance = new GameLogger();
        public static GameLogger Instance
        {
            get { return instance; }
        }
        private GameLogger()
        {
        }


        

    }
}
