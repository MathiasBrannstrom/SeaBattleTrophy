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
    }

    public class ShipOrderManager : IShipOrderManager
    {

        private Dictionary<int, IShipOrder> _shipOrdersByIndex = new Dictionary<int, IShipOrder>();
        private IEnumerable<IShip> _ships;

        public event PropertyChangedEventHandler PropertyChanged;

        public ShipOrderManager(IEnumerable<IShip> ships)
        {
            _ships = ships;
            foreach(var ship in _ships)
            {
                _shipOrdersByIndex.Add(ship.Index, new ShipOrder());
            }
        }

        public void SendShipOrders()
        {
            // For now we send them sequentially to each ship. Later we will step forward.
            foreach(var ship in _ships)
            {
                ship.SendShipOrder(_shipOrdersByIndex[ship.Index]);
                _shipOrdersByIndex[ship.Index] = new ShipOrder();
            }

            PropertyChanged.Raise(() => DoesAllShipsHaveValidOrders);
        }

        public void UpdateWithPartialShipOrder(IShipReadOnly ship, IShipOrder newShipOrders)
        {
            _shipOrdersByIndex[ship.Index].UpdateWithNewOrders(newShipOrders);

            //TODO: This could be optimized to not have to recheck all ships each time one ship has it's orders updated.
            PropertyChanged.Raise(() => DoesAllShipsHaveValidOrders);
        }

        public bool DoesAllShipsHaveValidOrders
        {
            get
            { 
                return _ships.All(ship => ship.IsOrderValid(_shipOrdersByIndex[ship.Index]));
            }
        }
    }
}
