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
        private IValueHolderReadOnly<IShipReadOnly> _ship;
        private IShipOrderManager _shipOrderManager;

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

                    var partialOrder = new ShipOrder { ShipSailLevelIncrement = RequestedSailLevelChange };
                    _shipOrderManager.UpdateWithPartialShipOrder(_ship.Value, partialOrder);

                    PropertyChanged.Raise(() => RequestedSailLevelChange);
                }
            }
        }

        public bool IsShipOrderReadyToSend
        {
            get { return _shipOrderManager.DoesAllShipsHaveFinishedOrders(); }
        }

        public void SendShipOrders()
        {
            _shipOrderManager.SendShipOrders();
        }

        //public bool IsLastOrderValidToSendAgain
        //{
        //    get { return _lastOrder.HasValue && _lastOrder.Value.GetTotalDistance().NearEquals(_ship.CurrentSpeed); }
        //}



        public ShipOrderViewModel(IValueHolderReadOnly<IShipReadOnly> ship, IShipOrderManager shipOrderManager)
        {
            _ship = ship;
            _shipOrderManager = shipOrderManager;
        }
        
        public void ForwardButton()
        {
            var partialOrder = ShipOrder.SingleMovementShipOrder(new ForwardMovementOrder { Distance = _ship.Value.CurrentSpeed });
            _shipOrderManager.UpdateWithPartialShipOrder(_ship.Value, partialOrder);
        }

        public void TurnCCW()
        {
            var partialOrder = ShipOrder.SingleMovementShipOrder(new YawMovementOrder { Direction = Direction.Port, Distance = _ship.Value.CurrentSpeed, YawRadius = 20 });
            _shipOrderManager.UpdateWithPartialShipOrder(_ship.Value, partialOrder);
        }

        public void TurnCW()
        {
            var partialOrder = ShipOrder.SingleMovementShipOrder(new YawMovementOrder { Direction = Direction.Starboard, Distance = _ship.Value.CurrentSpeed, YawRadius = 20 });
            _shipOrderManager.UpdateWithPartialShipOrder(_ship.Value, partialOrder);
        }


        //public void SetLastOrderAgain()
        //{
        //    _shipOrderManager.SetShipOrderForShip(_ship, _lastOrder);
        //}
    }
}
