using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame.Objects.Ship;
using System.ComponentModel;

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
            if(e.PropertyName == "Position")
            {
                OnPropertyChanged("XPosInPixels");
                OnPropertyChanged("YPosInPixels");
            }

            if(e.PropertyName == "AngleInDegrees")
            {
                OnPropertyChanged("RotationAngle");
            }
        }

        public float WidthInPixels { get { return _ship.Width / _metersPerPixel; } }

        public float LengthInPixels { get { return _ship.Length / _metersPerPixel; } }

        public float XPosInPixels { get { return _ship.Position.X / _metersPerPixel; } }

        public float YPosInPixels { get { return SeaMap.SeaMapSizeInPixels - _ship.Position.Y / _metersPerPixel; } }

        public float RotationAngle { get { return -_ship.AngleInDegrees; } }

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
