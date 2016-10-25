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
            var order = ShipOrder.SingleMovementShipOrder(new MovementOrder { Direction = Direction.Forward, Distance = 10 });
            _ship.ApplyShipOrder(order);
        }

        public void TurnCCW()
        {
            var order = ShipOrder.SingleMovementShipOrder(new MovementOrder { Direction = Direction.Port, Distance = 15 });
            _ship.ApplyShipOrder(order);
        }

        public void TurnCW()
        {
            var order = ShipOrder.SingleMovementShipOrder(new MovementOrder { Direction = Direction.Starboard, Distance = 15 });
            _ship.ApplyShipOrder(order);
        }
    }
}
