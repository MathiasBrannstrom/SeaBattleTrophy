using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame;
using System.ComponentModel;
using Utilities;
using System.Collections.ObjectModel;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class ShipOrderViewModel : INotifyPropertyChanged
    {
        private IShipReadOnly _currentShip;
        private IValueHolderReadOnly<IShipReadOnly> _selectedShip;
        private IShipOrderManager _shipOrderManager;
        private IShipOrderReadOnly _currentShipOrder;

        
        public event PropertyChangedEventHandler PropertyChanged;

        public double CurrentMaxSpeed
        {
            get { return _selectedShip.Value.CurrentSpeed; }
        }

        public ReadOnlyObservableCollection<MovementOrder> CurrentMovementOrders
        {
            get { return _currentShipOrder.MovementOrders; }
        }

        public SailLevelChange RequestedSailLevelChange
        {
            get { return _currentShipOrder.ShipSailLevelIncrement.GetValueOrDefault(); }
            set
            {
                if(_currentShipOrder.ShipSailLevelIncrement != value)
                {
                    var partialOrder = new ShipOrder { ShipSailLevelIncrement = value };
                    _shipOrderManager.UpdateWithPartialShipOrder(_selectedShip.Value, partialOrder);

                    PropertyChanged.Raise(() => RequestedSailLevelChange);
                }
            }
        }

        public bool AreShipOrdersReadyToSend
        {
            get { return _shipOrderManager.DoesAllShipsHaveValidOrders; }
        }

        public void SendShipOrders()
        {
            _shipOrderManager.SendShipOrders();
        }

        public void RemoveMovementOrder(MovementOrder movementOrder)
        {
            _shipOrderManager.RemoveMovementOrderFromShip(_selectedShip.Value, movementOrder);
        }

        public ShipOrderViewModel(IValueHolderReadOnly<IShipReadOnly> selectedShip, IShipOrderManager shipOrderManager)
        {
            _selectedShip = selectedShip;
            _selectedShip.PropertyChanged += HandleSelectedShipPropertyChanged;
            _shipOrderManager = shipOrderManager;
            _shipOrderManager.PropertyChanged += HandleShipOrderManagerPropertyChanged;
            HandleSelectedShipPropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        private void HandleSelectedShipPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_currentShip != null)
                _currentShip.PropertyChanged -= HandleCurrentShipPropertyChanged;

            _currentShip = _selectedShip.Value;
            _currentShip.PropertyChanged += HandleCurrentShipPropertyChanged;

            HandleCurrentShipPropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        private void HandleCurrentShipPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IShip.CurrentShipOrder) || e.PropertyName == null)
            {
                _currentShipOrder = _currentShip.CurrentShipOrder;

                PropertyChanged.Raise(() => CurrentMaxSpeed);
                PropertyChanged.Raise(() => CurrentMovementOrders);
                PropertyChanged.Raise(() => RequestedSailLevelChange);
            }
        }

        private void HandleShipOrderManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ShipOrderManager.DoesAllShipsHaveValidOrders))
            {
                PropertyChanged.Raise(() => AreShipOrdersReadyToSend);
            }
        }

        public void ForwardButton()
        {
            var partialOrder = ShipOrder.SingleMovementShipOrder(new ForwardMovementOrder { Distance = _selectedShip.Value.CurrentSpeed });
            _shipOrderManager.UpdateWithPartialShipOrder(_selectedShip.Value, partialOrder);
        }

        public void TurnCCW()
        {
            var partialOrder = ShipOrder.SingleMovementShipOrder(new YawMovementOrder { Direction = Direction.Port, Distance = _selectedShip.Value.CurrentSpeed, YawRadius = 20 });
            _shipOrderManager.UpdateWithPartialShipOrder(_selectedShip.Value, partialOrder);
        }

        public void TurnCW()
        {
            var partialOrder = ShipOrder.SingleMovementShipOrder(new YawMovementOrder { Direction = Direction.Starboard, Distance = _selectedShip.Value.CurrentSpeed, YawRadius = 20 });
            _shipOrderManager.UpdateWithPartialShipOrder(_selectedShip.Value, partialOrder);
        }
    }
}
