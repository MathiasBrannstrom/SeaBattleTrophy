using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Maths.Geometry.Tests
{
    [TestClass()]
    public class PolygonExtensionsTests
    {
        [TestMethod()]
        public void ShortestDistanceToPointTest()
        {
            var points = new[] { new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1), new Point2D(1, 0) };

            var polygon = new Polygon(points);
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
    }
}