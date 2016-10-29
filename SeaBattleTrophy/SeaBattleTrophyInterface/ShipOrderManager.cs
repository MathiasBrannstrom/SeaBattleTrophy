using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTrophyGame
{
    public interface IShipOrderManager
    {
        void UpdateWithPartialShipOrder(IShipReadOnly ship, IShipOrder shipOrder);

        void SendShipOrders();

        bool DoesAllShipsHaveFinishedOrders();
    }

    public class ShipOrderManager : IShipOrderManager
    {

        private Dictionary<int, IShipOrder> _shipOrdersByIndex = new Dictionary<int, IShipOrder>();
        private IEnumerable<IShipOrderReceiver> _shipOrderReceivers;

        public ShipOrderManager(IEnumerable<IShipOrderReceiver> shipOrderReceivers)
        {
            _shipOrderReceivers = shipOrderReceivers;
            foreach(var receiver in _shipOrderReceivers)
            {
                _shipOrdersByIndex.Add(receiver.Index, new ShipOrder());
            }
        }

        public void SendShipOrders()
        {
            // For now we send them sequentially to each ship. Later we will step forward.
            foreach(var shipOrderReceiver in _shipOrderReceivers)
            {
                shipOrderReceiver.SendShipOrder(_shipOrdersByIndex[shipOrderReceiver.Index]);
                _shipOrdersByIndex[shipOrderReceiver.Index] = new ShipOrder();
            }
        }

        public void UpdateWithPartialShipOrder(IShipReadOnly ship, IShipOrder newShipOrders)
        {
            _shipOrdersByIndex[ship.Index].UpdateWithNewOrders(newShipOrders);
        }

        public bool DoesAllShipsHaveFinishedOrders()
        {
            //Fix this, we need access to the speed of the ships... Maybe I should send in something with more info than a IShipOrderReceiver...
            return true;
        }
    }
}
