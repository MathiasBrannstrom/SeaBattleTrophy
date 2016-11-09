using Maths.Geometry;
using System.ComponentModel;
using Utilities;

namespace SeaBattleTrophyGame
{
    public interface IShipStatusReadOnly : INotifyPropertyChanged
    {
        double DistanceFromLand { get; }

        Point2D Position { get; }

        Vector2D Velocity { get; }

        SailLevel SailLevel { get; }
    }

    public interface IShipStatus : IShipStatusReadOnly
    {
        new double DistanceFromLand { get; set; }

        new Point2D Position { get; set; }

        new Vector2D Velocity { get; set; }

        new SailLevel SailLevel { get; set; }
    }

    public class ShipStatus : IShipStatus
    {
        private double _distanceFromLand;
        private Point2D _position;
        private Vector2D _velocity;
        private SailLevel _sailLevel;

        public double DistanceFromLand
        {
            get
            {
                return _distanceFromLand;
            }

            set
            {
                if(_distanceFromLand != value)
                {
                    _distanceFromLand = value;
                    PropertyChanged.Raise(() => DistanceFromLand);
                }
            }
        }

        double IShipStatusReadOnly.DistanceFromLand
        {
            get
            {
                return DistanceFromLand;
            }
        }

        public Point2D Position
        {
            get
            {
                return _position;
            }
            set
            {
                if(!_position.Equals(value))
                {
                    _position = value;
                    PropertyChanged.Raise(() => Position);
                }
            }
        }

        public Vector2D Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                if (!_velocity.Equals(value))
                {
                    _velocity = value;
                    PropertyChanged.Raise(() => Velocity);
                }
            }
        }

        public SailLevel SailLevel
        {
            get
            {
                return _sailLevel;
            }
            set
            {
                if (!_sailLevel.Equals(value))
                {
                    _sailLevel = value;
                    PropertyChanged.Raise(() => SailLevel);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
