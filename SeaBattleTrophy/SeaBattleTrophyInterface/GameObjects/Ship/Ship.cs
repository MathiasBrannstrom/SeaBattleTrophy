using System;
using System.Collections.Generic;
using System.Linq;
using Maths.Geometry;
using Utilities;
using System.ComponentModel;
using Maths;

namespace SeaBattleTrophyGame
{
    public enum SailLevel
    {
        NoSails = 0,
        LowSails = 1,
        CombatSails = 2,
        FullSails = 3,
        FullSailsWithLeadSail = 4,
    }

    public interface IShipReadOnly : INotifyPropertyChanged
    {
        int Index { get; }
        // [0, 360[   0 angle points north, increasing angle turns CCW.
        double AngleInDegrees { get; }

        // XY coordinates are in meters.
        Polygon2D Shape { get; }

        // In kilograms
        double Mass { get; }

        IShipOrderReadOnly CurrentShipOrder { get; }

        bool HasValidShipOrder { get; }

        IShipStatusReadOnly ShipStatus { get; }
    }

    public interface IShip : IShipReadOnly
    {
        void ApplyCurrentShipOrder(TimeSpan t, bool isFinalChange, IWind currentWind);

        new IShipOrder CurrentShipOrder { get; }

        void SetShipOrder(IShipOrder order);

        new IShipStatus ShipStatus { get; }
    }

    internal class Ship : IShip
    {
        private double _angleInDegrees;
        public double AngleInDegrees { get { return _angleInDegrees; }
            set
            {
                _angleInDegrees = value.Modulo(360);
            }
        }

        public Polygon2D Shape { get; set; }

        public double Mass { get; set; }

        //public double SpeedMultiplierFromWind(IWind wind)
        //{
        //    var angleDiff = 180 - Math.Abs(180 - (AngleInDegrees - wind.Angle));
        //    Console.WriteLine(angleDiff);
        //    if (angleDiff < 30)
        //        return wind.Velocity * 0.08;
        //    if(angleDiff < 150)
        //        return wind.Velocity*0.1;

        //    return wind.Velocity*0.03;
        //}

        public int Index { get; set; }

        public IShipOrder CurrentShipOrder { get; private set; }
        IShipOrderReadOnly IShipReadOnly.CurrentShipOrder { get { return CurrentShipOrder; } }
        public bool HasValidShipOrder { get { return CurrentShipOrder.IsValid; } }

        private ShipStatus _shipStatus;
        public IShipStatus ShipStatus { get { return _shipStatus; } }
        IShipStatusReadOnly IShipReadOnly.ShipStatus { get { return _shipStatus; } }

        #region ShipOrders

        private static Stack<MovementOrder> CreateMovementOrderStack(IShipOrderReadOnly shipOrder)
        {
            var copiedMovementOrders = shipOrder.MovementOrders.Select(order => order.Copy()).Reverse();

            return new Stack<MovementOrder>(copiedMovementOrders);
        }

        private Stack<MovementOrder> _movementOrderStack;

        public Ship(ShipStatus status)
        {
            _shipStatus = status;
        }

        public void ApplyCurrentShipOrder(TimeSpan timeStep, bool isFinalChange, IWind currentWind)
        {
            if (!HasValidShipOrder)
                throw new InvalidOperationException("The order is not complete enough to send to this ship.");

            if (_movementOrderStack == null)
                _movementOrderStack = CreateMovementOrderStack(CurrentShipOrder);

            var timeLeftToTravel = timeStep;

            while(_movementOrderStack.Any())
            {
                var movementOrder = _movementOrderStack.Pop();

                // If we don't manage to finish the movement order this time step we subtract the spent time and put it back on the stack.
                if (timeStep < movementOrder.TimeSpan)
                {
                    movementOrder.TimeSpan -= timeStep;
                    _movementOrderStack.Push(movementOrder); 
                }

                var timeSpentForThisMovementOrder = new TimeSpan(Math.Min(timeLeftToTravel.Ticks, movementOrder.TimeSpan.Ticks));

                var forcesOnShip = this.CalculateForceOnShip(currentWind);
                var acceleration = forcesOnShip / Mass;

                
                ShipStatus.Velocity += acceleration * timeStep.TotalSeconds;

                ShipStatus.Position += ShipStatus.Velocity * timeStep.TotalSeconds;

                //var distanceToTravelForThisMovementOrder = timeSpentForThisMovementOrder.TotalSeconds * CurrentSpeed * SpeedMultiplierFromWind(currentWind);

                //if (movementOrder is ForwardMovementOrder)
                //    ApplyMovementOrder((ForwardMovementOrder)movementOrder, distanceToTravelForThisMovementOrder);
                //else if (movementOrder is YawMovementOrder)
                //    ApplyMovementOrder((YawMovementOrder)movementOrder, distanceToTravelForThisMovementOrder);

                timeLeftToTravel -= timeSpentForThisMovementOrder;
                if (timeLeftToTravel.Ticks==0)
                    break;
            }

            if (isFinalChange)
            {
                _movementOrderStack = null;

                if (CurrentShipOrder.ShipSailLevelIncrement.HasValue && CurrentShipOrder.ShipSailLevelIncrement != SailLevelChange.StayAtCurrentSailSpeed)
                    ChangeSailLevel(CurrentShipOrder.ShipSailLevelIncrement.Value);
            }

        }

        private void ChangeSailLevel(SailLevelChange sailLevelChange)
        {
            switch (sailLevelChange)
            {
                case SailLevelChange.DecreaseSailLevel:
                    ShipStatus.SailLevel = (SailLevel)Math.Max(0, (int)(ShipStatus.SailLevel - 1));
                    break;
                case SailLevelChange.IncreaseSailLevel:
                    ShipStatus.SailLevel = (SailLevel)Math.Min(3, (int)(ShipStatus.SailLevel + 1));
                    break;
            }
        }

        private void ApplyMovementOrder(ForwardMovementOrder movementOrder, double distanceToTravel)
        {
            //Position += Vector2D.DirectionFromAngle(AngleInDegrees) * distanceToTravel;
            //PropertyChanged.Raise(() => Position);
        }

        private void ApplyMovementOrder(YawMovementOrder movementOrder, double distanceToTravel)
        {
            var angleChange = distanceToTravel * 180 / (movementOrder.YawRadius * Math.PI);
            var xChange = movementOrder.YawRadius * (1 - Math.Cos(angleChange/180*Math.PI));
            var yChange = movementOrder.YawRadius * Math.Sin(angleChange/180*Math.PI);

            var changeVector = new Vector2D(movementOrder.Direction == Direction.Starboard? xChange : -xChange, yChange);
            changeVector = changeVector.Rotate(AngleInDegrees);

            switch (movementOrder.Direction)
            {
                case Direction.Port:
                    AngleInDegrees += angleChange;
                    break;
                case Direction.Starboard:
                    AngleInDegrees -= angleChange;
                    break;
            }
            //Position += changeVector;
            //PropertyChanged.Raise(() => Position);
            PropertyChanged.Raise(() => AngleInDegrees);
        }

        public void SetShipOrder(IShipOrder order)
        {
            if(CurrentShipOrder != null)
                CurrentShipOrder.PropertyChanged -= HandleCurrentMovementOrdersCollectionChanged;

            CurrentShipOrder = order;

            CurrentShipOrder.PropertyChanged += HandleCurrentMovementOrdersCollectionChanged;
            PropertyChanged.Raise(() => CurrentShipOrder);
            PropertyChanged.Raise(() => HasValidShipOrder);
        }

        private void HandleCurrentMovementOrdersCollectionChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IShipOrderReadOnly.IsValid))
                PropertyChanged.Raise(() => HasValidShipOrder);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
