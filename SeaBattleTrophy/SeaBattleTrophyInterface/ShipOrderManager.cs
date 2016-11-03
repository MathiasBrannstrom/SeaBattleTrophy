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
        private IEnumerable<LandMass> _landMasses;

        public event PropertyChangedEventHandler PropertyChanged;

        public ShipOrderManager(IEnumerable<IShip> ships, IEnumerable<LandMass> landMasses)
        {
            _landMasses = landMasses;

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

        //public async void SendShipOrders()
        //{
        //    // For now we send them sequentially to each ship. Later we will step forward.
        //    var timeStep = 1.0f / 40;
        //    var t = timeStep;
        //    var finalStepDone = false;
        //    while (!finalStepDone)
        //    {
        //        var isFinalChange = t.NearEquals(1.0f);
        //        //foreach (var ship in _shipsByIndex.Values)
        //        //    ship.ApplyCurrentShipOrder(t, isFinalChange);

        //        var time = DateTime.UtcNow;
        //        // Check collisions etc.
        //        foreach(var ship in _shipsByIndex.Values)
        //        {
        //            // if ever relevant the polygons can be cached (since later ships will be checked against other ships later)
        //            // Even more importantly, the polygons can have a lazily created bounding box that will be checked first. Should
        //            // give a big speed improvment.
        //            var adjustedShipPolygon = ship.Shape.Transform(Transformations.Rotation2D(ship.AngleInDegrees), new Vector2D(ship.Position.X, ship.Position.Y));
        //            var closestDistance = double.MaxValue;
        //            foreach(var landMass in _landMasses)
        //            {
        //                var distance = landMass.LandPolygon.ShortestDistanceToOtherPolygon(adjustedShipPolygon);

        //                if (distance < closestDistance)
        //                    closestDistance = distance;
        //            }

        //            ship.ShipStatus.DistanceFromLand = (float)closestDistance;
        //        }

        //        var timeSpent = DateTime.UtcNow - time;

        //        await Task.Delay(Math.Max(20 - (int)timeSpent.TotalMilliseconds,0));

        //        finalStepDone = isFinalChange;
        //        t = Math.Min(1.0f, t + timeStep);
        //    }
        //}

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
