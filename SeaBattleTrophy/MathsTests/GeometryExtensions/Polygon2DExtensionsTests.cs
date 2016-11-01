using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maths.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths.Geometry.Tests
{
    [TestClass()]
    public class Polygon2DExtensionsTests
    {
        [TestMethod()]
        public void ShortestDistanceToPointTest()
        {
            var points = new[] { new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1), new Point2D(1, 0) };

            var polygon = new Polygon2D(points);
            Point2D pointOnPolygon;
            double distance;

            // Scenario 1
            distance = polygon.ShortestDistanceToPoint(new Point2D(0, 1), out pointOnPolygon);
            Assert.AreEqual(0, distance);
            Assert.AreEqual(new Point2D(0, 1), pointOnPolygon);

            // Scenario 2
            distance = polygon.ShortestDistanceToPoint(new Point2D(2, 1), out pointOnPolygon);
            Assert.AreEqual(1, distance);
            Assert.AreEqual(new Point2D(1, 1), pointOnPolygon);

            // Scenario 3
            distance = polygon.ShortestDistanceToPoint(new Point2D(2, 0.5f), out pointOnPolygon);
            Assert.AreEqual(1, distance);
            Assert.AreEqual(new Point2D(1, 0.5f), pointOnPolygon);

            // Scenario 4
            distance = polygon.ShortestDistanceToPoint(new Point2D(2, 2), out pointOnPolygon);
            Assert.AreEqual(Math.Sqrt(2), distance);
            Assert.AreEqual(new Point2D(1, 1), pointOnPolygon);

            // Test scenario for when it's inside the square.
        }

        [TestMethod()]
        public void ShortestSquaredDistanceToOtherPolygonTest()
        {
            var points = new[] { new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1), new Point2D(1, 0) };
            var points2 = new[] { new Point2D(0.2f, 1.4f), new Point2D(0, 2), new Point2D(2, 2) };

            var polygon = new Polygon2D(points);
            var polygon2 = new Polygon2D(points2);

            LineSegment2D shortestDistanceSegment;

            var shortestDistance = polygon.ShortestDistanceToOtherPolygon(polygon2, out shortestDistanceSegment);

            Assert.AreEqual(0.4, shortestDistance, 0.000001);
            Assert.AreEqual(new LineSegment2D(new Point2D(0.2f, 1), new Point2D(0.2f, 1.4f)), shortestDistanceSegment);
        }
    }
}