using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTrophyGame
{
    public enum SailLevelChange
    {
        IncreaseSailLevel,
        DecreaseSailLevel,
        StayAtCurrentSailSpeed
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

    public interface IShipOrder
    {
        List<MovementOrder> MovementOrders { get; }

        SailLevelChange? ShipSailLevelIncrement { get; }

        void UpdateWithNewOrders(IShipOrder newShipOrders);
    }

    public enum Direction
    {
        Port,
        Starboard
    }

    public abstract class MovementOrder
    {
        public float Distance { get; set; }
    }

    public class ForwardMovementOrder : MovementOrder { }

    public class YawMovementOrder : MovementOrder
    {
        public Direction Direction { get; set; }
        public float YawRadius { get; set; }
    }

    public class ShipOrder : IShipOrder
    {
        public static ShipOrder SingleMovementShipOrder(MovementOrder movementOrder)
        {
            var list = new List<MovementOrder>();
            list.Add(movementOrder);
            return new ShipOrder { MovementOrders = list };
        }

        public void UpdateWithNewOrders(IShipOrder newShipOrders)
        {
            if (newShipOrders.MovementOrders != null)
                MovementOrders = newShipOrders.MovementOrders;

            if (newShipOrders.ShipSailLevelIncrement != null)
                ShipSailLevelIncrement = newShipOrders.ShipSailLevelIncrement;
        }

        public List<MovementOrder> MovementOrders { get; set; }

        public SailLevelChange? ShipSailLevelIncrement { get; set; }
    }
}
