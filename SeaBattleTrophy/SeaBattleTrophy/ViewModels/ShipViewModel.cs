using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame;
using System.ComponentModel;
using Utilities;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class ShipViewModel : INotifyPropertyChanged
    {
        private IShip _ship;
        private float _metersPerPixel;


        public ShipViewModel(IShip ship, float metersPerPixel)
        {
            _ship = ship;
            _ship.PropertyChanged += HandleShipPropertyChanged;
            _metersPerPixel = metersPerPixel;
        }

        private void HandleShipPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IShip.Position))
            {
                PropertyChanged.Raise(() => XPosInPixels);
                PropertyChanged.Raise(() => YPosInPixels);
            }

            if(e.PropertyName == nameof(IShip.AngleInDegrees))
            {
                PropertyChanged.Raise(() => RotationAngle);
            }

            if (e.PropertyName == nameof(IShip.CurrentSpeed))
                PropertyChanged.Raise(() => Speed);
        }

        public float WidthInPixels { get { return _ship.Width / _metersPerPixel; } }

        public float LengthInPixels { get { return _ship.Length / _metersPerPixel; } }

        public float XPosInPixels { get { return _ship.Position.X / _metersPerPixel; } }

        public float YPosInPixels { get { return SeaMap.SeaMapSizeInPixels - _ship.Position.Y / _metersPerPixel; } }

        public float RotationAngle { get { return -_ship.AngleInDegrees; } }

        public float Speed { get { return _ship.CurrentSpeed; } }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
