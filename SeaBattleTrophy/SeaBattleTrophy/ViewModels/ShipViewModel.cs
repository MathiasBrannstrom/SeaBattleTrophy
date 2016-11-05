using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame;
using System.ComponentModel;
using Utilities;
using System.Windows.Media;
using System.Windows;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class ShipViewModel : INotifyPropertyChanged
    {
        private IShipReadOnly _ship;
        private IValueHolderReadOnly<float> _metersPerPixel;
        private IValueHolderReadOnly<float> _seaMapSizeInPixels;
        private IValueHolder<IShipReadOnly> _selectedShip;


        public ShipViewModel(IShipReadOnly ship, IValueHolderReadOnly<float> metersPerPixel, 
            IValueHolderReadOnly<float> seaMapSizeInPixels, IValueHolder<IShipReadOnly> selectedShip)
        {
            _ship = ship;
            _ship.PropertyChanged += HandleShipPropertyChanged;

            _metersPerPixel = metersPerPixel;
            _metersPerPixel.PropertyChanged += HandleMetersPerPixelPropertyChanged;
            _seaMapSizeInPixels = seaMapSizeInPixels;

            _selectedShip = selectedShip;
            _selectedShip.PropertyChanged += HandleSelectedShipPropertyChanged;

            UpdateShape();
        }

        private void HandleMetersPerPixelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateShape();
            PropertyChanged.Raise(() => ShapeInPixels);
            PropertyChanged.Raise(() => XPosInPixels);
            PropertyChanged.Raise(() => YPosInPixels);
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

        private void UpdateShape()
        {
            ShapeInPixels = new PointCollection(_ship.Shape.Select(p => new Point(p.X / _metersPerPixel.Value, p.Y / _metersPerPixel.Value)));
            PropertyChanged.Raise(() => ShapeInPixels);

        }

        public PointCollection ShapeInPixels { get; private set; } 

        public float XPosInPixels { get { return _ship.Position.X / _metersPerPixel.Value; } }

        public float YPosInPixels { get { return _seaMapSizeInPixels.Value - _ship.Position.Y / _metersPerPixel.Value; } }

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
