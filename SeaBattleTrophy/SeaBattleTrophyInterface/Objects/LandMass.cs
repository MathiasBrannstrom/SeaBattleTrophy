using Maths.Geometry;

namespace SeaBattleTrophyGame
{
    public class LandMass
    {
        public Polygon LandPolygon { get { return _landPolygon; } }
        private Polygon _landPolygon;

        public LandMass(Polygon landPolygon)
        {
            _landPolygon = landPolygon;
        }
    }
}
