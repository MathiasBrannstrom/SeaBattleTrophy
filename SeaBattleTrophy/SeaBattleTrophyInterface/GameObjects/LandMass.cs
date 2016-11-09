using Maths.Geometry;

namespace SeaBattleTrophyGame
{
    public class LandMass
    {
        public Polygon2D LandPolygon { get { return _landPolygon; } }
        private Polygon2D _landPolygon;

        public LandMass(Polygon2D landPolygon)
        {
            _landPolygon = landPolygon;
        }
    }
}
