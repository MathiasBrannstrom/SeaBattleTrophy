using System;
using System.Collections.Generic;
using System.Linq;
using Maths.Geometry;
using Utilities;
using System.ComponentModel;

namespace SeaBattleTrophyGame
{
    public enum SailLevel
    {
        LowSails = 0,
        CombatSails = 1,
        FullSails = 2,
        FullSailsWithLeadSail = 3
    }

    public interface IShipReadOnly : INotifyPropertyChanged
    {
        int Index { get; }
        // [0, 360[   0 angle points north, increasing angle turns CCW.
        double AngleInDegrees { get; }

        Point2D Position { get; }

        Polygon2D Shape { get; }

        SailLevel SailLevel { get; }

        double CurrentSpeed { get; }

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
        public double AngleInDegrees { get; set; }

        public Point2D Position { get; set; }

        public Polygon2D Shape { get; set; }
        
        public double CurrentSpeed
        {
            get { return 10.0 * SailLevelSpeedModifier(SailLevel); }
        }

        public double DriftMultiplier
        {
            get { return 0.1; }
        }

        public SailLevel SailLevel { get; set; }

        public int Index { get; set; }

        public IShipOrder CurrentShipOrder { get; private set; }
        IShipOrderReadOnly IShipReadOnly.CurrentShipOrder { get { return CurrentShipOrder; } }
        public bool HasValidShipOrder { get { return CurrentShipOrder.IsValid; } }


        private ShipStatus _shipStatus = new ShipStatus();
        public IShipStatus ShipStatus { get { return _shipStatus; } }
        IShipStatusReadOnly IShipReadOnly.ShipStatus { get { return _shipStatus; } }

        public event PropertyChangedEventHandler PropertyChanged;

        private static Stack<MovementOrder> CreateMovementOrderStack(IShipOrderReadOnly shipOrder)
        {
            var copiedMovementOrders = shipOrder.MovementOrders.Select(order => order.Copy()).Reverse();

            return new Stack<MovementOrder>(copiedMovementOrders);
        }

        private Stack<MovementOrder> _movementOrderStack;

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

                var distanceToTravelForThisMovementOrder = timeSpentForThisMovementOrder.TotalSeconds * CurrentSpeed;

                if (movementOrder is ForwardMovementOrder)
                    ApplyMovementOrder((ForwardMovementOrder)movementOrder, distanceToTravelForThisMovementOrder);
                else if (movementOrder is YawMovementOrder)
                    ApplyMovementOrder((YawMovementOrder)movementOrder, distanceToTravelForThisMovementOrder);

                timeLeftToTravel -= timeSpentForThisMovementOrder;
                if (timeLeftToTravel.Ticks==0)
                    break;
            }

            ApplyWindDrift(currentWind, timeStep);

            if (isFinalChange)
            {
                _movementOrderStack = null;

                if (CurrentShipOrder.ShipSailLevelIncrement.HasValue && CurrentShipOrder.ShipSailLevelIncrement != SailLevelChange.StayAtCurrentSailSpeed)
                    ChangeSailLevel(CurrentShipOrder.ShipSailLevelIncrement.Value);
            }

        }

        private void ApplyWindDrift(IWind wind, TimeSpan timeStep)
        {
            var direction = Vector2D.DirectionFromAngle(wind.Angle);

            Position += direction * wind.Velocity * timeStep.TotalSeconds * DriftMultiplier;
        }

        private void ChangeSailLevel(SailLevelChange sailLevelChange)
        {
            switch (sailLevelChange)
            {
                case SailLevelChange.DecreaseSailLevel:
                    SailLevel = (SailLevel)Math.Max(0, (int)(SailLevel - 1));
                    break;
                case SailLevelChange.IncreaseSailLevel:
                    SailLevel = (SailLevel)Math.Min(3, (int)(SailLevel + 1));
                    break;
            }

            OnPropertyChanged("CurrentSpeed");
            OnPropertyChanged("SailLevel");
        }

        private void ApplyMovementOrder(ForwardMovementOrder movementOrder, double distanceToTravel)
        {
            Position += Vector2D.DirectionFromAngle(AngleInDegrees) * distanceToTravel;
            OnPropertyChanged("Position");
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
            Position += changeVector;

            OnPropertyChanged("Position");
            OnPropertyChanged("AngleInDegrees");
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

        // Create the OnPropertyChanged method to raise the event
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        // This should be moved somewhere
        private static double SailLevelSpeedModifier(SailLevel sailLevel)
        {
            switch (sailLevel)
            {
                case SailLevel.LowSails:
                    return 0.4f;
                case SailLevel.CombatSails:
                    return 0.6f;
                case SailLevel.FullSails:
                    return 0.8f;
                case SailLevel.FullSailsWithLeadSail:
                    return 1.0f;
                default:
                    throw new ArgumentOutOfRangeException("This sail speed is not supported!");
            }
        }
    }
}
