using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using SeaBattleTrophyGame.Orders;
using System.ComponentModel;

namespace SeaBattleTrophyGame.Objects.Ship
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
        // [0, 360[   0 angle points east, increasing angle turns CCW.
        float AngleInDegrees { get; }

        Point2D Position { get; }

        float Length { get; }

        float Width { get; }

        SailLevel SailLevel { get; }
    }
    public interface IShip : IShipReadOnly
    {
        void ApplyShipOrder(IShipOrder order);
    }

    internal class Ship : IShip
    {
        public float AngleInDegrees { get; set; }

        public Point2D Position { get; set; }

        public float Length { get; set; }

        public float Width { get; set; }

        public SailLevel SailLevel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Vector2D GetDirection()
        {
            return new Vector2D { X = (float)Math.Cos(AngleInDegrees * Math.PI / 180.0), Y = (float)Math.Sin(AngleInDegrees * Math.PI / 180.0) };
        }

        public void ApplyShipOrder(IShipOrder order)
        {
            foreach(var movementOrder in order.MovementOrders)
            {
                if (movementOrder is ForwardMovementOrder)
                    ApplyMovementOrder((ForwardMovementOrder)movementOrder);
                else if (movementOrder is YawMovementOrder)
                    ApplyMovementOrder((YawMovementOrder)movementOrder);
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

        // Create the OnPropertyChanged method to raise the event
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
