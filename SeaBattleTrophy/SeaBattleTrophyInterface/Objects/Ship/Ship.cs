using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        // [0, 360[   0 angle points east, increasing angle turns CCW.
        float AngleInDegrees { get; }

        Point2D Position { get; }

        float Length { get; }

        float Width { get; }

        SailLevel SailLevel { get; }

        float CurrentSpeed { get; }

        IShipOrderReadOnly CurrentShipOrder { get; }

        bool HasValidShipOrder { get; }
    }

    public interface IShip : IShipReadOnly
    {
        void ApplyCurrentShipOrder();

        new IShipOrder CurrentShipOrder { get; }

        void SetShipOrder(IShipOrder order);
    }

    internal class Ship : IShipReadOnly, IShip
    {
        public float AngleInDegrees { get; set; }

        public Point2D Position { get; set; }

        public float Length { get; set; }

        public float Width { get; set; }

        public float CurrentSpeed
        {
            get { return 15.0f * SailLevelSpeedModifier(SailLevel); }
        }

        public SailLevel SailLevel { get; set; }

        public int Index { get; set; }

        public IShipOrder CurrentShipOrder { get; private set; }

        public bool HasValidShipOrder { get { return CurrentShipOrder.GetTotalDistance().NearEquals(CurrentSpeed); } }

        IShipOrderReadOnly IShipReadOnly.CurrentShipOrder { get { return CurrentShipOrder; } }

        public event PropertyChangedEventHandler PropertyChanged;


        private Vector2D GetDirection()
        {
            return new Vector2D { X = (float)Math.Cos(AngleInDegrees * Math.PI / 180.0), Y = (float)Math.Sin(AngleInDegrees * Math.PI / 180.0) };
        }

        public void ApplyCurrentShipOrder()
        {
            if (!HasValidShipOrder)
                throw new InvalidOperationException("The order is not complete enough to send to this ship.");

            foreach(var movementOrder in CurrentShipOrder.MovementOrders)
            {
                if (movementOrder is ForwardMovementOrder)
                    ApplyMovementOrder((ForwardMovementOrder)movementOrder);
                else if (movementOrder is YawMovementOrder)
                    ApplyMovementOrder((YawMovementOrder)movementOrder);
            }

            if (CurrentShipOrder.ShipSailLevelIncrement.HasValue && CurrentShipOrder.ShipSailLevelIncrement != SailLevelChange.StayAtCurrentSailSpeed)
                ChangeSailLevel(CurrentShipOrder.ShipSailLevelIncrement.Value);
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

        private void ApplyMovementOrder(ForwardMovementOrder movementOrder)
        {
            Position = Position + GetDirection() * movementOrder.Distance;
            OnPropertyChanged("Position");
        }

        private void ApplyMovementOrder(YawMovementOrder movementOrder)
        {
            var angleChange = (float)(movementOrder.Distance * 180 / (movementOrder.YawRadius * Math.PI));
            var xChange = (float)(movementOrder.YawRadius * (1 - Math.Cos(angleChange/180*Math.PI)));
            var yChange = (float)(movementOrder.YawRadius * Math.Sin(angleChange/180*Math.PI));

            var changeVector = new Vector2D(xChange, yChange);
            changeVector = changeVector.Rotate(AngleInDegrees - 90);

            switch (movementOrder.Direction)
            {
                case Direction.Port:
                    AngleInDegrees += angleChange;
                    break;
                case Direction.Starboard:
                    AngleInDegrees -= angleChange;
                    break;
            }
            Position = Position + changeVector;

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
