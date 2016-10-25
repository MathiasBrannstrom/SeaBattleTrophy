using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame.Objects.Ship;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class ShipViewModel
    {
        private IShip _ship;
        private float _metersPerPixel;

        public ShipViewModel(IShip ship, float metersPerPixel)
        {
            _ship = ship;
            _metersPerPixel = metersPerPixel;
        }

        public float WidthInPixels { get { return _ship.Width / _metersPerPixel; } }

        public float LengthInPixels { get { return _ship.Length / _metersPerPixel; } }

        public float XPosInPixels { get { return _ship.Position.X / _metersPerPixel; } }

        public float YPosInPixels { get { return _ship.Position.Y / _metersPerPixel; } }

        public float RotationAngle { get { return -_ship.AngleInDegrees; } }

    }
}
