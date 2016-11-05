using System;
using System.Collections.Generic;
using Maths.Geometry;

namespace SeaBattleTrophyGame
{

    public interface ISeaBattleTrophyGame
    {
        IEnumerable<IShipReadOnly> Ships { get; }

        ISeaMap SeaMap { get; }

        IShipOrderEditor ShipOrderEditor { get; }

        ITurnManager TurnManager { get; }

        IWindManager WindManager { get; }
    }

    public class SeaBattleTrophyGameManager : ISeaBattleTrophyGame
    {
        private List<Ship> _ships = new List<Ship>();
        
        public SeaBattleTrophyGameManager()
        {
            //var smallPolygon = new Polygon2D(new[] { new Point2D(-0.1f, -0.1f), new Point2D(-0.1f, 0.1f), new Point2D(0.1f, 0.1f), new Point2D(0.1f, -0.1f) });

            var rectanglePolygon = new Polygon2D(new[] { new Point2D(-5, -20), new Point2D(-5, 20), new Point2D(0, 22), new Point2D(5, 20), new Point2D(5, -20) });
            //_ships.Add(new Ship { Shape = rectanglePolygon, Position = new Point2D(20, 80), AngleInDegrees = -30, Index = 0, SailLevel = SailLevel.LowSails });
            _ships.Add(new Ship { Shape = rectanglePolygon, Position = new Point2D(20, 40), AngleInDegrees = 0, Index = 1, SailLevel = SailLevel.FullSailsWithLeadSail });
            //_ships.Add(new Ship { Width = 20, Length = 30, Position = new Point2D { X = 70, Y = 30 }, AngleInDegrees = 90, Index = 2, SailLevel = SailLevel.FullSailsWithLeadSail });

            Ships = _ships;

            WindManager = new WindManager();

            SetupSeaMap();

            var shipOrderManager = new ShipOrderManager(_ships);
            ShipOrderEditor = shipOrderManager;

            TurnManager = new TurnManager(_ships, SeaMap, shipOrderManager, WindManager);
        }

        public void SetupSeaMap()
        {
            var landMasses = new List<LandMass>();

            landMasses.Add(new LandMass(new Polygon2D(new[] { new Point2D(100, 0), new Point2D(110, 180), new Point2D(150, 200), new Point2D(190,180), new Point2D(200, 0) })));
            landMasses.Add(new LandMass(new Polygon2D(new[] { new Point2D(50, 200), new Point2D(60, 210), new Point2D(60, 260), new Point2D(24, 240), new Point2D(20,210)})));
            SeaMap = new SeaMap { SizeInMeters = 300, LandMasses = landMasses };
        }

        public ISeaMap SeaMap { get; private set; }

        public IShipOrderEditor ShipOrderEditor { get; set; }

        public IEnumerable<IShipReadOnly> Ships { get; private set; }

        public IWindManager WindManager { get; private set; }

        public ITurnManager TurnManager { get; private set; }
    }
}
