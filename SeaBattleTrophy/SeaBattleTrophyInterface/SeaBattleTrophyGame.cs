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
            _ships.Add(new Ship { Width = 10, Length = 40, Position = new Point2D { X = 20, Y = 30 }, AngleInDegrees = 90, Index = 0, SailLevel = SailLevel.LowSails });
            _ships.Add(new Ship { Width = 10, Length = 40, Position = new Point2D { X = 50, Y = 30 }, AngleInDegrees = 90, Index = 1, SailLevel = SailLevel.FullSailsWithLeadSail });
            //_ships.Add(new Ship { Width = 20, Length = 30, Position = new Point2D { X = 70, Y = 30 }, AngleInDegrees = 90, Index = 2, SailLevel = SailLevel.FullSailsWithLeadSail });

            Ships = _ships;

            ShipOrderManager = new ShipOrderManager(_ships);

            SetupSeaMap();
        }

        public void SetupSeaMap()
        {
            var landMasses = new List<Land>();

            landMasses.Add(new Land(new[] { new Point2D(100, 100), new Point2D(150, 200), new Point2D(200, 100) }));
            landMasses.Add(new Land(new[] { new Point2D(50, 200), new Point2D(60, 210), new Point2D(60, 260), new Point2D(24, 240), new Point2D(20,210)}));
            SeaMap = new SeaMap { SizeInMeters = 300, LandMasses = landMasses };
        }

        public ISeaMap SeaMap { get; private set; }

        public IShipOrderManager ShipOrderManager { get; set; }


        public IEnumerable<IShipReadOnly> Ships { get; private set; }
    }
}
