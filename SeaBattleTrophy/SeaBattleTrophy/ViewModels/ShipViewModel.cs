using System.Linq;
using SeaBattleTrophyGame;
using System.ComponentModel;
using Utilities;
using System.Windows.Media;
using System.Windows;
using System;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class ShipViewModel : INotifyPropertyChanged
    {
        private IShipReadOnly _ship;
        private IValueHolderReadOnly<double> _metersPerPixel;
        private IValueHolderReadOnly<double> _seaMapSizeInPixels;
        private IValueHolder<IShipReadOnly> _selectedShip;


        public ShipViewModel(IShipReadOnly ship, IValueHolderReadOnly<double> metersPerPixel, 
            IValueHolderReadOnly<double> seaMapSizeInPixels, IValueHolder<IShipReadOnly> selectedShip)
        {
            _ship = ship;
            _ship.PropertyChanged += HandleShipPropertyChanged;
            _ship.ShipStatus.PropertyChanged += HandleShipStatusPropertyChanged;
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

        private void HandleShipStatusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IShipStatusReadOnly.Position))
            {
                PropertyChanged.Raise(() => XPosInPixels);
                PropertyChanged.Raise(() => YPosInPixels);
            }
        }

        private void HandleShipPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IShipReadOnly.AngleInDegrees))
            {
                PropertyChanged.Raise(() => RotationAngle);
            }
        }

        private void UpdateShape()
        {
            ShapeInPixels = new PointCollection(_ship.Shape.Select(p => new Point(p.X / _metersPerPixel.Value, -p.Y / _metersPerPixel.Value)));
            PropertyChanged.Raise(() => ShapeInPixels);

        }

        public PointCollection ShapeInPixels { get; private set; } 

        public double XPosInPixels { get { return _ship.ShipStatus.Position.X / _metersPerPixel.Value; } }

        public double YPosInPixels { get { return _seaMapSizeInPixels.Value - _ship.ShipStatus.Position.Y / _metersPerPixel.Value; } }

        public double RotationAngle { get { return -_ship.AngleInDegrees; } }

        public bool IsSelected { get { return _selectedShip.Value == _ship; } }

        public void SelectThisShip()
        {
            _selectedShip.Value = _ship;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
