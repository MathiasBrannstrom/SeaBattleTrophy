using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maths;
using Maths.Geometry;
using Utilities;

namespace SeaBattleTrophyGame
{
    public interface ITurnManager : INotifyPropertyChanged
    {
        bool ReadyToFinishCurrentPhase { get; }

        void FinishCurrentPhase();

        TimeSpan LengthOfTurn { get; } 
    }
    internal class TurnManager : ITurnManager
    {
        private ISeaMap _seaMap;
        private IShipOrderMovementPhaseManager _shipOrderMovementPhaseManager;
        private IEnumerable<IShip> _ships;
        private IWindManager _windManager;
        private TimeSpan _timeSinceStart = new TimeSpan();

        public event PropertyChangedEventHandler PropertyChanged;

        public TurnManager(IEnumerable<IShip> ships, ISeaMap seaMap, IShipOrderMovementPhaseManager shipOrderMovementPhaseManager, 
            IWindManager windManager)
        {
            _shipOrderMovementPhaseManager = shipOrderMovementPhaseManager;
            _shipOrderMovementPhaseManager.PropertyChanged += HandleShipOrderMovementPhaseManagerChanged;
            _windManager = windManager;
            _seaMap = seaMap;
            _ships = ships;

            _shipOrderMovementPhaseManager.ResetShipOrdersForNewTurn(LengthOfTurn);
        }

        private void HandleShipOrderMovementPhaseManagerChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IShipOrderMovementPhaseManager.DoesAllShipsHaveValidOrders))
                PropertyChanged.Raise(() => ReadyToFinishCurrentPhase);
        }

        public bool ReadyToFinishCurrentPhase { get { return _shipOrderMovementPhaseManager.DoesAllShipsHaveValidOrders; } }

        private TimeSpan _lengthOfTurn = TimeSpan.FromSeconds(5);
        public TimeSpan LengthOfTurn
        {
            get
            {
                return _lengthOfTurn;
            }
        }

        public void CheckCollisions()
        {
            foreach (var ship in _ships)
            {
                // if ever relevant the polygons can be cached (since later ships will be checked against other ships later)
                // Even more importantly, the polygons can have a lazily created bounding box that will be checked first. Should
                // give a big speed improvment.
                var adjustedShipPolygon = ship.Shape.Transform(Transformations.Rotation2D(ship.AngleInDegrees), 
                    new Vector2D(ship.ShipStatus.Position.X, ship.ShipStatus.Position.Y));

                var closestDistance = double.MaxValue;
                foreach (var landMass in _seaMap.LandMasses)
                {
                    var distance = landMass.LandPolygon.ShortestDistanceToOtherPolygon(adjustedShipPolygon);

                    if (distance < closestDistance)
                        closestDistance = distance;
                }

                ship.ShipStatus.DistanceFromLand = closestDistance;
            }
        }

        public void FinishCurrentPhase()
        {
            MovementTurn();
        }

        public async void MovementTurn()
        {
            var timeStep = LengthOfTurn.DivideBy(40);

            var partialTime = new TimeSpan();
            while(partialTime < LengthOfTurn)
            {
                var t = new TimeSpan(Math.Min(timeStep.Ticks, (LengthOfTurn - partialTime).Ticks));
                partialTime += t;

                var isFinalChange = partialTime.Ticks == LengthOfTurn.Ticks;

                _shipOrderMovementPhaseManager.ApplyShipOrders(t, isFinalChange, _windManager.CurrentWind);

                var time = DateTime.UtcNow;
                // Check collisions etc.
                CheckCollisions();

                var timeSpent = DateTime.UtcNow - time;

                await Task.Delay(Math.Max(20 - (int)timeSpent.TotalMilliseconds, 0));

                _windManager.UpdateWind(timeStep);
            }

            _timeSinceStart += LengthOfTurn;

            // for now all turns are equally long (maybe always like this?)
            var lengthOfNextTurn = LengthOfTurn;

            _shipOrderMovementPhaseManager.ResetShipOrdersForNewTurn(lengthOfNextTurn);
        }

    }
}
