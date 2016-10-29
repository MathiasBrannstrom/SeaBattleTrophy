using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SeaBattleTrophyGame
{

    public interface ISeaBattleTrophyGame
    {
        IEnumerable<IShipReadOnly> Ships { get; }

        ISeaMap SeaMap { get; }

        IShipOrderManager ShipOrderManager { get; }
    }

    public class SeaBattleTrophyGameManager : ISeaBattleTrophyGame
    {
        private List<Ship> _ships = new List<Ship>();

        public SeaBattleTrophyGameManager()
        {
            //_ships.Add(new Ship { Width = 10, Length = 40, Position = new Point2D { X = 0, Y = 0 }, AngleInDegrees = 90, Index = 0, SailLevel = SailLevel.FullSails });
            _ships.Add(new Ship { Width = 10, Length = 40, Position = new Point2D { X = 50, Y = 50 }, AngleInDegrees = 40, Index = 1, SailLevel = SailLevel.FullSails });

            Ships = _ships;

            ShipOrderManager = new ShipOrderManager(_ships);

            SeaMap = new SeaMap { SizeInMeters = 200 };
        }

        public ISeaMap SeaMap { get; private set; }

        public IShipOrderManager ShipOrderManager { get; set; }


        public IEnumerable<IShipReadOnly> Ships { get; private set; }
    }
}
