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
        private IShipReadOnly _ship;
        private float _metersPerPixel;
        private IValueHolder<IShipReadOnly> _selectedShip;

        public ShipViewModel(IShipReadOnly ship, float metersPerPixel)
        {
            _ship = ship;
            _ship.PropertyChanged += HandleShipPropertyChanged;
            _metersPerPixel = metersPerPixel;
        }

        public ShipViewModel(IShipReadOnly ship, float metersPerPixel, IValueHolder<IShipReadOnly> selectedShip) 
            : this(ship, metersPerPixel)
        {
            _selectedShip = selectedShip;
            _selectedShip.PropertyChanged += HandleSelectedShipPropertyChanged;
        }

        private void HandleSelectedShipPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged.Raise(() => IsSelected);
        }

        private void HandleShipPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IShipReadOnly.Position))
            {
                PropertyChanged.Raise(() => XPosInPixels);
                PropertyChanged.Raise(() => YPosInPixels);
            }

            if(e.PropertyName == nameof(IShipReadOnly.AngleInDegrees))
            {
                PropertyChanged.Raise(() => RotationAngle);
            }

            if (e.PropertyName == nameof(IShipReadOnly.CurrentSpeed))
                PropertyChanged.Raise(() => Speed);
        }

        public float WidthInPixels { get { return _ship.Width / _metersPerPixel; } }

        public float LengthInPixels { get { return _ship.Length / _metersPerPixel; } }

        public float XPosInPixels { get { return _ship.Position.X / _metersPerPixel; } }

        public float YPosInPixels { get { return SeaMap.SeaMapSizeInPixels - _ship.Position.Y / _metersPerPixel; } }

        public float RotationAngle { get { return -_ship.AngleInDegrees; } }

        public float Speed { get { return _ship.CurrentSpeed; } }

        public bool IsSelected { get { return _selectedShip.Value == _ship; } }

        public void SelectThisShip()
        {
            _selectedShip.Value = _ship;
        }

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
