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
    public interface IShip : INotifyPropertyChanged
    {
        // [0, 360[   0 angle points east, increasing angle turns CCW.
        float AngleInDegrees { get; set; }

        Point2D Position { get; set; }
        
        float Length { get; set; }

        float Width { get; set; }

        void ApplyShipOrder(IShipOrder order);
    }

    internal class Ship : IShip
    {
        public float AngleInDegrees { get; set; }

        public Point2D Position { get; set; }

        public float Length { get; set; }

        public float Width { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Vector2D GetDirection()
        {
            return new Vector2D { X = (float)Math.Cos(AngleInDegrees * Math.PI / 180.0), Y = (float)Math.Sin(AngleInDegrees * Math.PI / 180.0) };
        }

        public void ApplyShipOrder(IShipOrder order)
        {
            foreach(var movementOrder in order.MovementOrders)
            {
                ApplyMovementOrder(movementOrder);
            }
        }

        private void ApplyMovementOrder(MovementOrder movementOrder)
        {
            switch (movementOrder.Direction)
            {
                case Direction.Forward:
                    Position = Position + GetDirection() * movementOrder.Distance;
                    OnPropertyChanged("Position");
                    break;
                case Direction.Port:
                    AngleInDegrees += movementOrder.Distance;
                    OnPropertyChanged("AngleInDegrees");
                    break;
                case Direction.Starboard:
                    AngleInDegrees -= movementOrder.Distance;
                    OnPropertyChanged("AngleInDegrees");
                    break;
            }
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
