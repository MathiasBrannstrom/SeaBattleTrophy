using System;
using System.Collections.ObjectModel;
using System.Linq;

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
        public static float GetTotalDistance(this IShipOrder order)
        {
            if (order.MovementOrders == null)
                return 0;
            else return order.MovementOrders.Sum(movementOrder => movementOrder.Distance);
        }
    }

    public interface IShipOrderReadOnly
    {
        SailLevelChange? ShipSailLevelIncrement { get; }

        ReadOnlyObservableCollection<MovementOrder> MovementOrders { get; }
    }
    
    public interface IShipOrder : IShipOrderReadOnly
    {
        new ObservableCollection<MovementOrder> MovementOrders { get; }

        
        void UpdateWithNewOrders(IShipOrder newShipOrders);
    }

    public enum Direction
    {
        Forward,
        Port,
        Starboard
    }

    public abstract class MovementOrder
    {
        public float Distance { get; set; }

        public abstract MovementOrder Copy();
    }

    public class ForwardMovementOrder : MovementOrder
    {
        public override MovementOrder Copy()
        {
            return new ForwardMovementOrder { Distance = Distance };
        }
    }

    public class YawMovementOrder : MovementOrder
    {
        public Direction Direction { get; set; }
        public float YawRadius { get; set; }

        public override MovementOrder Copy()
        {
            return new YawMovementOrder { Distance = Distance, Direction = Direction, YawRadius = YawRadius };
        }
    }

    public class ShipOrder : IShipOrder
    {
        private ReadOnlyObservableCollection<MovementOrder> _movementOrdersReadOnly;

        public static ShipOrder SingleMovementShipOrder(MovementOrder movementOrder)
        {
            var shipOrder = new ShipOrder();
            shipOrder.MovementOrders.Add(movementOrder);

            return shipOrder;
        }

        public void UpdateWithNewOrders(IShipOrder newShipOrders)
        {
            if (newShipOrders.MovementOrders.Any())
            {
                MovementOrders.Clear();
                foreach (var movementOrder in newShipOrders.MovementOrders)
                    MovementOrders.Add(movementOrder);
            }

            if (newShipOrders.ShipSailLevelIncrement != null)
                ShipSailLevelIncrement = newShipOrders.ShipSailLevelIncrement;
        }

        public ShipOrder()
        {
            MovementOrders = new ObservableCollection<MovementOrder>();
            _movementOrdersReadOnly = new ReadOnlyObservableCollection<MovementOrder>(MovementOrders);
        }

        public ObservableCollection<MovementOrder> MovementOrders { get; private set; }

        public SailLevelChange? ShipSailLevelIncrement { get; set; }

        ReadOnlyObservableCollection<MovementOrder> IShipOrderReadOnly.MovementOrders { get { return _movementOrdersReadOnly; } }
    }
}
