using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maths.Geometry;
using Maths;
using Utilities;
using System.ComponentModel;
using System.Collections.Specialized;

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
        float AngleInDegrees { get; }

        Point2D Position { get; }

        Polygon2D Shape { get; }

        SailLevel SailLevel { get; }

        float CurrentSpeed { get; }

        IShipOrderReadOnly CurrentShipOrder { get; }

        bool HasValidShipOrder { get; }

        IShipStatusReadOnly ShipStatus { get; }
    }

    public interface IShip : IShipReadOnly
    {
        void ApplyCurrentShipOrder(float t, bool isFinalChange);

        new IShipOrder CurrentShipOrder { get; }

        void SetShipOrder(IShipOrder order);

        new IShipStatus ShipStatus { get; }
    }

    internal class Ship : IShip
    {
        public float AngleInDegrees { get; set; }

        public Point2D Position { get; set; }

        public Polygon2D Shape { get; set; }
        
        public float CurrentSpeed
        {
            get { return 30.0f * SailLevelSpeedModifier(SailLevel); }
        }

        public SailLevel SailLevel { get; set; }

        public int Index { get; set; }

        public IShipOrder CurrentShipOrder { get; private set; }

        public bool HasValidShipOrder { get { return CurrentShipOrder.GetTotalDistance().NearEquals(CurrentSpeed); } }

        IShipOrderReadOnly IShipReadOnly.CurrentShipOrder { get { return CurrentShipOrder; } }

        private ShipStatus _shipStatus = new ShipStatus();

        public IShipStatus ShipStatus
        {
            get
            {
                return _shipStatus;
            }
        }

        IShipStatusReadOnly IShipReadOnly.ShipStatus
        {
            get
            {
                return _shipStatus;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private Vector2D GetDirection()
        {
            return new Vector2D((float)-Math.Sin(AngleInDegrees * Math.PI / 180.0), (float)Math.Cos(AngleInDegrees * Math.PI / 180.0));
        }

        private static Stack<MovementOrder> CreateMovementOrderStack(IShipOrderReadOnly shipOrder)
        {
            var copiedMovementOrders = shipOrder.MovementOrders.Select(order => order.Copy()).Reverse();

            return new Stack<MovementOrder>(copiedMovementOrders);
        }

        private Stack<MovementOrder> _movementOrderStack;
        private float _lastTimePoint;

        public void ApplyCurrentShipOrder(float t, bool isFinalChange)
        {
            if (!HasValidShipOrder)
                throw new InvalidOperationException("The order is not complete enough to send to this ship.");

            if(_movementOrderStack == null)
                _movementOrderStack = CreateMovementOrderStack(CurrentShipOrder);

            var distanceToTravelThisTimeStep = (t-_lastTimePoint) * CurrentSpeed;
            while(_movementOrderStack.Any())
            {
                var movementOrder = _movementOrderStack.Pop();

                var distanceToTravelForThisMovementOrder = Math.Min(movementOrder.Distance, distanceToTravelThisTimeStep);

                // If we don't manage to finish the movement order this time step we subtract the travelled distance and put it back
                // on the stack.
                if (distanceToTravelThisTimeStep < movementOrder.Distance)
                {
                    movementOrder.Distance -= distanceToTravelThisTimeStep;
                    _movementOrderStack.Push(movementOrder); 
                }

                if (movementOrder is ForwardMovementOrder)
                    ApplyMovementOrder((ForwardMovementOrder)movementOrder, distanceToTravelForThisMovementOrder);
                else if (movementOrder is YawMovementOrder)
                    ApplyMovementOrder((YawMovementOrder)movementOrder, distanceToTravelForThisMovementOrder);

                distanceToTravelThisTimeStep -= distanceToTravelForThisMovementOrder;
                if (distanceToTravelThisTimeStep.NearEquals(0f))
                    break;
            }

            _lastTimePoint = t;

            if (isFinalChange)
            {
                _movementOrderStack = null;
                _lastTimePoint = 0;

                if (CurrentShipOrder.ShipSailLevelIncrement.HasValue && CurrentShipOrder.ShipSailLevelIncrement != SailLevelChange.StayAtCurrentSailSpeed)
                    ChangeSailLevel(CurrentShipOrder.ShipSailLevelIncrement.Value);
            }

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

        private void ApplyMovementOrder(ForwardMovementOrder movementOrder, float distanceToTravel)
        {
            Position += GetDirection() * distanceToTravel;
            OnPropertyChanged("Position");
        }

        private void ApplyMovementOrder(YawMovementOrder movementOrder, float distanceToTravel)
        {
            var angleChange = (float)(distanceToTravel * 180 / (movementOrder.YawRadius * Math.PI));
            var xChange = (float)(movementOrder.YawRadius * (1 - Math.Cos(angleChange/180*Math.PI)));
            var yChange = (float)(movementOrder.YawRadius * Math.Sin(angleChange/180*Math.PI));

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
                CurrentShipOrder.MovementOrders.CollectionChanged -= HandleCurrentMovementOrdersCollectionChanged;

            CurrentShipOrder = order;

            CurrentShipOrder.MovementOrders.CollectionChanged += HandleCurrentMovementOrdersCollectionChanged;
            PropertyChanged.Raise(() => CurrentShipOrder);
            PropertyChanged.Raise(() => HasValidShipOrder);
        }

        private void HandleCurrentMovementOrdersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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
        private static float SailLevelSpeedModifier(SailLevel sailLevel)
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
