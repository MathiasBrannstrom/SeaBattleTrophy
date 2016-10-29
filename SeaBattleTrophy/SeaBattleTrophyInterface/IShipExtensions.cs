using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SeaBattleTrophyGame
{
    public static class IShipExtensions
    {
        public static bool IsOrderValid(this IShipReadOnly ship, IShipOrder shipOrder)
        {
            return shipOrder.GetTotalDistance().NearEquals(ship.CurrentSpeed);
        }
    }
}
