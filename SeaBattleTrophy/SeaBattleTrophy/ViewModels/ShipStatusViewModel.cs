using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame;
using Utilities;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class ShipStatusViewModel : INotifyPropertyChanged
    {
        private IValueHolderReadOnly<IShipReadOnly> _selectedShip;

        private IShipReadOnly _currentShip;

        public ShipStatusViewModel(IValueHolderReadOnly<IShipReadOnly> selectedShip)
        {
            _selectedShip = selectedShip;
            _selectedShip.PropertyChanged += HandleSelectedShipPropertyChanged;

            HandleSelectedShipPropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        public double DistanceToLand
        {
            get { return _currentShip.ShipStatus.DistanceFromLand; }
        }

        public string Velocity
        {
            get { return _currentShip.ShipStatus.Velocity.ToString(); }
        }

        public string Position
        {
            get { return _currentShip.ShipStatus.Position.ToString(); }
        }

        public string SailLevel
        {
            get { return _currentShip.ShipStatus.SailLevel.ToString(); }
        }

        private void HandleSelectedShipPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_currentShip != null)
                _currentShip.ShipStatus.PropertyChanged -= HandleCurrentShipShipStatusChanged;

            _currentShip = _selectedShip.Value;

            _currentShip.ShipStatus.PropertyChanged += HandleCurrentShipShipStatusChanged;

            HandleCurrentShipShipStatusChanged(this, new PropertyChangedEventArgs(null));
        }

        private void HandleCurrentShipShipStatusChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IShipStatusReadOnly.DistanceFromLand))
                PropertyChanged.Raise(() => DistanceToLand);
            if (e.PropertyName == nameof(IShipStatusReadOnly.Velocity))
                PropertyChanged.Raise(() => Velocity);
            if (e.PropertyName == nameof(IShipStatusReadOnly.Position))
                PropertyChanged.Raise(() => Position);
            if (e.PropertyName == nameof(IShipStatusReadOnly.SailLevel))
                PropertyChanged.Raise(() => SailLevel);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
