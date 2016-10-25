using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SeaBattleTrophyGame.Objects.Ship
{
    public interface IShip
    {
        // [0, 360[   0 angle points east, increasing angle turns CCW.
        float AngleInDegrees { get; set; }

        Point2D Position { get; set; }
        
        float Length { get; set; }

        float Width { get; set; }
    }

    internal class Ship : IShip
    {
        public float AngleInDegrees { get; set; }

        public Point2D Position { get; set; }

        public float Length { get; set; }

        public float Width { get; set; }
    }
}
