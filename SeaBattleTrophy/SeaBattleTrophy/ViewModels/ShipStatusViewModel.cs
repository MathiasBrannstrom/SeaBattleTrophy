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

        public float DistanceToLand
        {
            get { return _currentShip.ShipStatus.DistanceFromLand; }
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
            PropertyChanged.Raise(() => DistanceToLand);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
