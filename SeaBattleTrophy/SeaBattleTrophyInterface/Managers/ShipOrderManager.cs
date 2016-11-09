using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Utilities;

namespace SeaBattleTrophyGame
{
    public interface IShipOrderEditor : INotifyPropertyChanged
    {
        void RemoveMovementOrderFromShip(IShipReadOnly value, MovementOrder movementOrder);

        void AddMovementOrder(IShipReadOnly value, MovementOrder movementOrder);
        void SetRequestedSailLevel(IShipReadOnly value1, SailLevelChange value2);
    }

    public interface IShipOrderMovementPhaseManager : INotifyPropertyChanged
    {
        void ResetShipOrdersForNewTurn(TimeSpan turnTimeSpan);

        bool DoesAllShipsHaveValidOrders { get; }

        void ApplyShipOrders(TimeSpan t, bool isFinalChange, IWind currentWind);
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

        public void ApplyShipOrders(TimeSpan t, bool isFinalChange, IWind currentWind)
        {
            if (!DoesAllShipsHaveValidOrders)
            {
                Console.WriteLine("Tried to send ship orders even though all of them were not ready...");
                return;
            }
                
            foreach (var ship in _shipsByIndex.Values)
                ship.ApplyCurrentShipOrder(t, isFinalChange, currentWind);
        }

        public void ResetShipOrdersForNewTurn(TimeSpan turnTimeSpan)
        {
            foreach (var ship in _shipsByIndex.Values)
                ship.SetShipOrder(new ShipOrder(turnTimeSpan));
        }

        public void RemoveMovementOrderFromShip(IShipReadOnly ship, MovementOrder movementOrder)
        {
            _shipsByIndex[ship.Index].CurrentShipOrder.MovementOrders.Remove(movementOrder);
        }

        public void AddMovementOrder(IShipReadOnly ship, MovementOrder movementOrder)
        {
            _shipsByIndex[ship.Index].CurrentShipOrder.MovementOrders.Add(movementOrder);
        }

        public void SetRequestedSailLevel(IShipReadOnly ship, SailLevelChange sailLevelChange)
        {
            _shipsByIndex[ship.Index].CurrentShipOrder.SetSailLevelChange(sailLevelChange);
        }

        public bool DoesAllShipsHaveValidOrders
        {
            get { return _shipsByIndex.All(kvp => kvp.Value.HasValidShipOrder); }
        }
    }
}
