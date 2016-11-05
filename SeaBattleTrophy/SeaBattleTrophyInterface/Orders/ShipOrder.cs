using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Utilities;

namespace SeaBattleTrophyGame
{
    public enum SailLevelChange
    {
        StayAtCurrentSailSpeed,
        IncreaseSailLevel,
        DecreaseSailLevel,
    }

    public static class IShipOrderExtensions
    {
        public static TimeSpan GetTotalTime(this IShipOrder order)
        {
            if (order.MovementOrders == null)
                return new TimeSpan();
            else return new TimeSpan(order.MovementOrders.Sum(movementOrder => movementOrder.TimeSpan.Ticks));
        }
    }

    public interface IShipOrderReadOnly : INotifyPropertyChanged
    {
        SailLevelChange? ShipSailLevelIncrement { get; }

        ReadOnlyObservableCollection<MovementOrder> MovementOrders { get; }

        TimeSpan TotalTimeSpanForOrder { get; }

        bool IsValid { get; }
    }
    
    public interface IShipOrder : IShipOrderReadOnly
    {
        new ObservableCollection<MovementOrder> MovementOrders { get; }

        void SetSailLevelChange(SailLevelChange sailLevelChange);
    }

    public enum Direction
    {
        Forward,
        Port,
        Starboard
    }

    public abstract class MovementOrder
    {
        public TimeSpan TimeSpan { get; set; }

        public abstract MovementOrder Copy();
    }

    public class ForwardMovementOrder : MovementOrder
    {
        public override MovementOrder Copy()
        {
            return new ForwardMovementOrder { TimeSpan = TimeSpan };
        }
    }

    public class YawMovementOrder : MovementOrder
    {
        public Direction Direction { get; set; }
        public float YawRadius { get; set; }

        public override MovementOrder Copy()
        {
            return new YawMovementOrder { TimeSpan = TimeSpan, Direction = Direction, YawRadius = YawRadius };
        }
    }

    public class ShipOrder : IShipOrder
    {
        private ReadOnlyObservableCollection<MovementOrder> _movementOrdersReadOnly;

        public event PropertyChangedEventHandler PropertyChanged;

        public ShipOrder(TimeSpan totalTimeSpanForOrder)
        {
            MovementOrders = new ObservableCollection<MovementOrder>();
            _movementOrdersReadOnly = new ReadOnlyObservableCollection<MovementOrder>(MovementOrders);
            MovementOrders.CollectionChanged += HandleMovementOrdersCollectionChanged;
            TotalTimeSpanForOrder = totalTimeSpanForOrder;
        }

        private void HandleMovementOrdersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged.Raise(() => IsValid);
        }

        public void SetSailLevelChange(SailLevelChange sailLevelChange)
        {
            ShipSailLevelIncrement = sailLevelChange;
        }

        public ObservableCollection<MovementOrder> MovementOrders { get; private set; }

        public SailLevelChange? ShipSailLevelIncrement { get; set; }

        ReadOnlyObservableCollection<MovementOrder> IShipOrderReadOnly.MovementOrders { get { return _movementOrdersReadOnly; } }

        public TimeSpan TotalTimeSpanForOrder { get; private set; }

        public bool IsValid { get { return MovementOrders.Sum(order => order.TimeSpan.Ticks) == TotalTimeSpanForOrder.Ticks; } }
    }
}
