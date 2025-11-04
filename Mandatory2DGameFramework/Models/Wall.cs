using Mandatory2DGameFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    internal class Wall : IWorldObject
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Lootable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Removable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
