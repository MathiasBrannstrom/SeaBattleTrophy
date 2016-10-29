using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SeaBattleTrophyGame
{
    public interface IShipOrderManager : INotifyPropertyChanged
    {
        void UpdateWithPartialShipOrder(IShipReadOnly ship, IShipOrder shipOrder);

        void SendShipOrders();

        bool DoesAllShipsHaveValidOrders { get; }

        void RemoveMovementOrderFromShip(IShipReadOnly value, MovementOrder movementOrder);
    }

    public class ShipOrderManager : IShipOrderManager
    {

        private Dictionary<int, IShip> _shipsByIndex = new Dictionary<int, IShip>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ShipOrderManager(IEnumerable<IShip> ships)
        {
            foreach(var ship in ships)
            {
                ship.SetShipOrder(new ShipOrder());
                _shipsByIndex.Add(ship.Index, ship);

            }
        }

        public void SendShipOrders()
        {
            // For now we send them sequentially to each ship. Later we will step forward.
            foreach(var kvp in _shipsByIndex)
            {
                var ship = kvp.Value;

                ship.ApplyCurrentShipOrder();
                ship.SetShipOrder(new ShipOrder());
            }

            PropertyChanged.Raise(() => DoesAllShipsHaveValidOrders);
        }

        public void UpdateWithPartialShipOrder(IShipReadOnly ship, IShipOrder newShipOrders)
        {
            _shipsByIndex[ship.Index].CurrentShipOrder.UpdateWithNewOrders(newShipOrders);

            //TODO: This could be optimized to not have to recheck all ships each time one ship has it's orders updated.
            // Well, now it has been optimized so that the ships store a bool on them whether or not it is updated, but I still would like to clean this up a bit.
            PropertyChanged.Raise(() => DoesAllShipsHaveValidOrders);
        }

        public void RemoveMovementOrderFromShip(IShipReadOnly ship, MovementOrder movementOrder)
        {
            _shipsByIndex[ship.Index].CurrentShipOrder.MovementOrders.Remove(movementOrder);
            PropertyChanged.Raise(() => DoesAllShipsHaveValidOrders);
        }

        public bool DoesAllShipsHaveValidOrders
        {
            get
            { 
                return _shipsByIndex.All(kvp => kvp.Value.HasValidShipOrder);
            }
        }
    }
}
