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
    }

    internal class SeaMap : ISeaMap
    {
        // Currently only square maps
        public float SizeInMeters { get; set; }
    }
}
