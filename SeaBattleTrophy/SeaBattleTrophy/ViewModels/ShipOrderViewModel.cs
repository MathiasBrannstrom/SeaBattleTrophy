using System;
using System.Linq;
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
        private IShipOrderEditor _shipOrderManager;
        private IShipOrderReadOnly _currentShipOrder;

        public event PropertyChangedEventHandler PropertyChanged;

        public double TotalTimeForOrder
        {
            get { return _currentShipOrder.TotalTimeSpanForOrder.TotalSeconds; }
        }

        public double CurrentTimeRemaining
        {
            get { return _currentShipOrder.TotalTimeSpanForOrder.TotalSeconds - _currentShipOrder.MovementOrders.Sum(order => order.TimeSpan.TotalSeconds); }
        }

        public bool AnyDistanceRemaining
        {
            get { return CurrentTimeRemaining > float.Epsilon; }
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

                    _shipOrderManager.SetRequestedSailLevel(_selectedShip.Value, value);
                    
                    PropertyChanged.Raise(() => RequestedSailLevelChange);
                }
            }
        }

        public void RemoveMovementOrder(MovementOrder movementOrder)
        {
            _shipOrderManager.RemoveMovementOrderFromShip(_selectedShip.Value, movementOrder);
        }

        public ShipOrderViewModel(IValueHolderReadOnly<IShipReadOnly> selectedShip, IShipOrderEditor shipOrderManager)
        {
            _selectedShip = selectedShip;
            _selectedShip.PropertyChanged += HandleSelectedShipPropertyChanged;
            _shipOrderManager = shipOrderManager;
            HandleSelectedShipPropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        internal void AddMovementOrder(Direction direction, double timeInSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(timeInSeconds);
            MovementOrder movementOrder;
            switch (direction)
            {
                case Direction.Forward:
                    movementOrder = new ForwardMovementOrder { TimeSpan = timeSpan };
                    break;
                case Direction.Port:
                case Direction.Starboard:
                    movementOrder = new YawMovementOrder { Direction = direction, TimeSpan = timeSpan, YawRadius = 40 };
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
                PropertyChanged.Raise(() => TotalTimeForOrder);
                PropertyChanged.Raise(() => CurrentMovementOrders);
                PropertyChanged.Raise(() => RequestedSailLevelChange);
            }
        }

        private void HandleCurrentMovementOrdersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged.Raise(() => CurrentTimeRemaining);
            PropertyChanged.Raise(() => AnyDistanceRemaining);
        }
    }
}
