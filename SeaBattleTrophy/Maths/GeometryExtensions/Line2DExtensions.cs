namespace Maths.Geometry
{
    public static class Line2DExtensions
    {
        public static bool IntersectsOtherLine(this Line2D line, Line2D otherLine, out Point2D? intersectionPoint)
        {
            intersectionPoint = null;

            var denominator = 
                (line.PointA.X - line.PointB.X) * (otherLine.PointA.Y - otherLine.PointB.Y) - 
                (line.PointA.Y - line.PointB.Y) * (otherLine.PointA.X - otherLine.PointB.X);

            if (denominator.NearEquals(0))
                return false;

            var xNumerator =
                (line.PointA.X * line.PointB.Y - line.PointA.Y * line.PointB.X) * (otherLine.PointA.X - otherLine.PointB.X) -
                (otherLine.PointA.X * otherLine.PointB.Y - otherLine.PointA.Y * otherLine.PointB.X) * (line.PointA.X - line.PointB.X);

            var yNumerator =
                (line.PointA.X * line.PointB.Y - line.PointA.Y * line.PointB.X) * (otherLine.PointA.Y - otherLine.PointB.Y) -
                (otherLine.PointA.X * otherLine.PointB.Y - otherLine.PointA.Y * otherLine.PointB.X) * (line.PointA.Y - line.PointB.Y);

            intersectionPoint = new Point2D(xNumerator, yNumerator) / denominator;
            return true;
        }
    }
}
