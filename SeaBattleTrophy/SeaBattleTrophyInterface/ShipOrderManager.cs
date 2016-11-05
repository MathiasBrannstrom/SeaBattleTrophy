using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Maths.Geometry;
using Maths;

namespace SeaBattleTrophyGame
{
    public interface IShipOrderEditor : INotifyPropertyChanged
    {
        void UpdateWithPartialShipOrder(IShipReadOnly ship, IShipOrder shipOrder);

        void RemoveMovementOrderFromShip(IShipReadOnly value, MovementOrder movementOrder);

        void AddMovementOrder(IShipReadOnly value, MovementOrder movementOrder);
    }

    public interface IShipOrderMovementPhaseManager : INotifyPropertyChanged
    {
        void ApplyShipOrders(float t, bool isFinalChange);

        void ClearShipOrders();

        bool DoesAllShipsHaveValidOrders { get; }
    }

    public interface IShipOrderManager : IShipOrderMovementPhaseManager, IShipOrderEditor { }

    public class ShipOrderManager : IShipOrderManager
    {

        private Dictionary<int, IShip> _shipsByIndex = new Dictionary<int, IShip>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ShipOrderManager(IEnumerable<IShip> ships)
        {
            foreach(var ship in ships)
            {
                ship.PropertyChanged += HandleShipPropertyChanged;
                ship.SetShipOrder(new ShipOrder());
                _shipsByIndex.Add(ship.Index, ship);

            }
        }

        private void HandleShipPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Ship.HasValidShipOrder))
            {
                PropertyChanged.Raise(() => DoesAllShipsHaveValidOrders);
            }
        }

        public void ApplyShipOrders(float t, bool isFinalChange)
        {
            foreach (var ship in _shipsByIndex.Values)
                ship.ApplyCurrentShipOrder(t, isFinalChange);
        }

        public void ClearShipOrders()
        {
            foreach (var ship in _shipsByIndex.Values)
                ship.SetShipOrder(new ShipOrder());
        }

        public void UpdateWithPartialShipOrder(IShipReadOnly ship, IShipOrder newShipOrders)
        {
            _shipsByIndex[ship.Index].CurrentShipOrder.UpdateWithNewOrders(newShipOrders);
        }

        public void RemoveMovementOrderFromShip(IShipReadOnly ship, MovementOrder movementOrder)
        {
            _shipsByIndex[ship.Index].CurrentShipOrder.MovementOrders.Remove(movementOrder);
        }

        public void AddMovementOrder(IShipReadOnly ship, MovementOrder movementOrder)
        {
            _shipsByIndex[ship.Index].CurrentShipOrder.MovementOrders.Add(movementOrder);
        }

        public bool DoesAllShipsHaveValidOrders
        {
            get { return _shipsByIndex.All(kvp => kvp.Value.HasValidShipOrder); }
        }
    }
}
