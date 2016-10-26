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
    public class ShipOrderViewModel : INotifyPropertyChanged
    {
        private IShip _ship;
        ShipOrder _order = new ShipOrder();
        ShipOrder? _lastOrder;
        private bool _isShipOrderReadyToSend;
        private SailLevelChange _requestedSailLevelChange;

        public event PropertyChangedEventHandler PropertyChanged;

        public SailLevelChange RequestedSailLevelChange
        {
            get { return _requestedSailLevelChange; }
            set
            {
                if(_requestedSailLevelChange != value)
                {
                    _requestedSailLevelChange = value;
                    PropertyChanged.Raise(() => RequestedSailLevelChange);
                }
            }
        }

        public bool IsShipOrderReadyToSend
        {
            get { return _isShipOrderReadyToSend; }
            set
            {
                if(_isShipOrderReadyToSend != value)
                {
                    _isShipOrderReadyToSend = value;
                    PropertyChanged.Raise(() => IsShipOrderReadyToSend);
                }
            }
        }

        public bool IsLastOrderValidToSendAgain
        {
            get { return _lastOrder.HasValue && _lastOrder.Value.GetTotalDistance().NearEquals(_ship.CurrentSpeed); }
        }



        public ShipOrderViewModel(IShip ship)
        {
            _ship = ship;
        }
        
        public void ForwardButton()
        {
            _order = ShipOrder.SingleMovementShipOrder(new ForwardMovementOrder { Distance = _ship.CurrentSpeed });
            IsShipOrderReadyToSend = true;
        }

        public void TurnCCW()
        {
            _order = ShipOrder.SingleMovementShipOrder(new YawMovementOrder { Direction = Direction.Port, Distance = _ship.CurrentSpeed, YawRadius = 20 });
            IsShipOrderReadyToSend = true;
        }

        public void TurnCW()
        {
            _order = ShipOrder.SingleMovementShipOrder(new YawMovementOrder { Direction = Direction.Starboard, Distance = _ship.CurrentSpeed, YawRadius = 20 });
            IsShipOrderReadyToSend = true;
        }

        public void Reset()
        {
            _order = new ShipOrder();
            IsShipOrderReadyToSend = false;
            RequestedSailLevelChange = SailLevelChange.StayAtCurrentSailSpeed;
        }

        public void SendOrder()
        {
            _order.ShipSailLevelIncrement = RequestedSailLevelChange;
            _ship.ApplyShipOrder(_order);
            _lastOrder = _order;
            PropertyChanged.Raise(() => IsLastOrderValidToSendAgain);
            Reset();
        }

        public void SendLastOrderAgain()
        {
            _ship.ApplyShipOrder(_lastOrder);
        }
    }
}
