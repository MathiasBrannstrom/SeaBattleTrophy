using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame.Objects.Ship;

namespace SeaBattleTrophyGame.Orders
{
    public enum SailLevelChange
    {
        StayAtCurrentSailSpeed,
        IncreaseSailLevel,
        DecreaseSailLevel
    }

    public interface IShipOrder
    {
        List<MovementOrder> MovementOrders { get; }

        SailLevelChange ShipSailLevelIncrement { get; }
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

    public struct ShipOrder : IShipOrder
    {
        public static ShipOrder SingleMovementShipOrder(MovementOrder movementOrder)
        {
            var list = new List<MovementOrder>();
            list.Add(movementOrder);
            return new ShipOrder { MovementOrders = list };
        }

        public List<MovementOrder> MovementOrders { get; set; }

        public SailLevelChange ShipSailLevelIncrement { get; set; }
    }
}
