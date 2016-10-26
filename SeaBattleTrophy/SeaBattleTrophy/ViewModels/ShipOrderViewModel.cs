using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame.Objects.Ship;
using SeaBattleTrophyGame.Orders;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class ShipOrderViewModel
    {
        private IShip _ship;

        public ShipOrderViewModel(IShip ship)
        {
            _ship = ship;
        }
        
        public void ForwardButton()
        {
            var order = ShipOrder.SingleMovementShipOrder(new ForwardMovementOrder { Distance = 5 });
            _ship.ApplyShipOrder(order);
        }

        public void TurnCCW()
        {
            var order = ShipOrder.SingleMovementShipOrder(new YawMovementOrder { Direction = Direction.Port, Distance = 5, YawRadius = 20 });
            _ship.ApplyShipOrder(order);
        }

        public void TurnCW()
        {
            var order = ShipOrder.SingleMovementShipOrder(new YawMovementOrder { Direction = Direction.Starboard, Distance = 5, YawRadius = 20 });
            _ship.ApplyShipOrder(order);
        }
    }
}
