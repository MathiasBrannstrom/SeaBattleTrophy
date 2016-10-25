using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTrophyGame.Orders
{
    public interface IShipOrder
    {
        List<MovementOrder> MovementOrders { get; }
    }

    public enum Direction
    {
        Forward,
        Port,
        Starboard
    }

    public struct MovementOrder
    {
        public Direction Direction;
        public float Distance;
    }

    public struct ShipOrder : IShipOrder
    {
        public static ShipOrder SingleMovementShipOrder(MovementOrder movementOrder)
        {
            var list = new List<MovementOrder>();
            list.Add(movementOrder);
            return new ShipOrder { MovementOrders = list };
        }

        public List<MovementOrder> MovementOrders { get; set; } }
}
