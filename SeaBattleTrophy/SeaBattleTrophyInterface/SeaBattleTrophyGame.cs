using System.Collections.Generic;
using Maths.Geometry;

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
            _ships.Add(new Ship { Width = 10, Length = 40, Position = new Point2D(20, 30), AngleInDegrees = 90, Index = 0, SailLevel = SailLevel.LowSails });
            _ships.Add(new Ship { Width = 10, Length = 40, Position = new Point2D(50, 30), AngleInDegrees = 90, Index = 1, SailLevel = SailLevel.FullSailsWithLeadSail });
            //_ships.Add(new Ship { Width = 20, Length = 30, Position = new Point2D { X = 70, Y = 30 }, AngleInDegrees = 90, Index = 2, SailLevel = SailLevel.FullSailsWithLeadSail });

            Ships = _ships;

            SetupSeaMap();


            ShipOrderManager = new ShipOrderManager(_ships, SeaMap.LandMasses);
        }

        public void SetupSeaMap()
        {
            var landMasses = new List<LandMass>();

            landMasses.Add(new LandMass(new Polygon2D(new[] { new Point2D(100, 0), new Point2D(110, 180), new Point2D(150, 200), new Point2D(190,180), new Point2D(200, 0) })));
            landMasses.Add(new LandMass(new Polygon2D(new[] { new Point2D(50, 200), new Point2D(60, 210), new Point2D(60, 260), new Point2D(24, 240), new Point2D(20,210)})));
            SeaMap = new SeaMap { SizeInMeters = 300, LandMasses = landMasses };
        }

        public ISeaMap SeaMap { get; private set; }

        public IShipOrderManager ShipOrderManager { get; set; }


        public IEnumerable<IShipReadOnly> Ships { get; private set; }
    }
}
