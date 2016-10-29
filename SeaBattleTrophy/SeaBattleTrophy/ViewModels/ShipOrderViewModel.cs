using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame;
using System.ComponentModel;
using Utilities;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class ShipOrderViewModel : INotifyPropertyChanged
    {
        private IShipReadOnly _currentShip;
        private IValueHolderReadOnly<IShipReadOnly> _selectedShip;
        private IShipOrderManager _shipOrderManager;
        private IShipOrderReadOnly _currentShipOrder;

        
        public event PropertyChangedEventHandler PropertyChanged;

        public float CurrentMaxSpeed
        {
            get { return _selectedShip.Value.CurrentSpeed; }
        }

        public float CurrentDistanceRemaining
        {
            get { return _selectedShip.Value.CurrentSpeed - _currentShipOrder.MovementOrders.Sum(order => order.Distance); }
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

        internal void AddMovementOrder(Direction direction, float distance)
        {
            MovementOrder movementOrder;
            switch (direction)
            {
                case Direction.Forward:
                    movementOrder = new ForwardMovementOrder { Distance = distance };
                    break;
                case Direction.Port:
                case Direction.Starboard:
                    movementOrder = new YawMovementOrder { Direction = direction, Distance = distance, YawRadius = 20 };
                    break;
                default:
                    throw new InvalidEnumArgumentException("There is no support for this enum value");
            }

            _shipOrderManager.AddMovementOrder(_currentShip, movementOrder);
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
                if(_currentShipOrder != null)
                    ((INotifyCollectionChanged)_currentShipOrder.MovementOrders).CollectionChanged -= HandleCurrentMovementOrdersChanged;

                _currentShipOrder = _currentShip.CurrentShipOrder;

                ((INotifyCollectionChanged)_currentShipOrder.MovementOrders).CollectionChanged += HandleCurrentMovementOrdersChanged;
                HandleCurrentMovementOrdersChanged(this, null);
                PropertyChanged.Raise(() => CurrentMaxSpeed);
                PropertyChanged.Raise(() => CurrentMovementOrders);
                PropertyChanged.Raise(() => RequestedSailLevelChange);
            }
        }

        private void HandleCurrentMovementOrdersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged.Raise(() => CurrentDistanceRemaining);
        }

        private void HandleShipOrderManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ShipOrderManager.DoesAllShipsHaveValidOrders))
            {
                PropertyChanged.Raise(() => AreShipOrdersReadyToSend);
            }
        }
    }
}
