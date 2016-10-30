using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTrophyGame
{
    public interface ISeaMap
    {
        float SizeInMeters { get; }

        IEnumerable<LandMass> LandMasses {get;}
    }

    internal class SeaMap : ISeaMap
    {
        public IEnumerable<LandMass> LandMasses { get; set; }

        // Currently only square maps
        public float SizeInMeters { get; set; }
    }
}
