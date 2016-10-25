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
        Vector2D Direction { get; set; }

        Point2D Position { get; set; }
        
        float Length { get; set; }

        float Width { get; set; }
    }

    internal class Ship : IShip
    {
        public Vector2D Direction { get; set; }

        public Point2D Position { get; set; }

        public float Length { get; set; }

        public float Width { get; set; }
    }
}
