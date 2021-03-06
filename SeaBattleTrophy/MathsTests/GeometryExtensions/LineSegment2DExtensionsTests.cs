﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maths.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths.Geometry.Tests
{
    [TestClass()]
    public class LineSegment2DExtensionsTests
    {
        [TestMethod()]
        public void IntersectsOtherLineSegmentTest()
        {
            // Scenario 1
            var ls0 = new LineSegment2D(new Point2D(-1, 0), new Point2D(1, 0));
            var ls1 = new LineSegment2D(new Point2D(0, -1), new Point2D(0, 1));
            Point2D? intersectionPoint;
            var hasIntersection = ls0.IntersectsOtherLineSegment(ls1, out intersectionPoint);
            Assert.IsTrue(hasIntersection);
            Assert.AreEqual(0, intersectionPoint.Value.X);
            Assert.AreEqual(0, intersectionPoint.Value.Y);

            // Scenario 2
            ls0 = new LineSegment2D(new Point2D(0, 1), new Point2D(1, 0));
            ls1 = new LineSegment2D(new Point2D(0, 0), new Point2D(1, 1));
            hasIntersection = ls0.IntersectsOtherLineSegment(ls1, out intersectionPoint);
            Assert.IsTrue(hasIntersection);
            Assert.AreEqual(0.5f, intersectionPoint.Value.X);
            Assert.AreEqual(0.5f, intersectionPoint.Value.Y);

            // Scenario 3
            ls0 = new LineSegment2D(new Point2D(0, 0), new Point2D(1, 0));
            ls1 = new LineSegment2D(new Point2D(0, 1), new Point2D(1, 1));
            hasIntersection = ls0.IntersectsOtherLineSegment(ls1, out intersectionPoint);
            Assert.IsFalse(hasIntersection);
            Assert.IsNull(intersectionPoint);

            // Scenario 4
            ls0 = new LineSegment2D(new Point2D(0, 0), new Point2D(2, 0));
            ls1 = new LineSegment2D(new Point2D(0, 2), new Point2D(0, 1));
            hasIntersection = ls0.IntersectsOtherLineSegment(ls1, out intersectionPoint);
            Assert.IsFalse(hasIntersection);
            Assert.IsNull(intersectionPoint);

            // Scenario 5
            ls0 = new LineSegment2D(new Point2D(0, 0), new Point2D(2, 0));
            ls1 = new LineSegment2D(new Point2D(0, 1), new Point2D(0, 3));
            hasIntersection = ls0.IntersectsOtherLineSegment(ls1, out intersectionPoint);
            Assert.IsFalse(hasIntersection);
            Assert.IsNull(intersectionPoint);

        }

        [TestMethod()]
        public void ShortestDistanceToOtherLineSegmentTest()
        {
            // Scenario 1
            var ls0 = new LineSegment2D(new Point2D(-1, 0), new Point2D(1, 0));
            var ls1 = new LineSegment2D(new Point2D(0, -1), new Point2D(0, 1));
            LineSegment2D shortestDistanceSegment;
            var shortestDistance = ls0.ShortestDistanceToOtherLineSegment(ls1, out shortestDistanceSegment);
            Assert.AreEqual(0, shortestDistance);
            Assert.AreEqual(new LineSegment2D(), shortestDistanceSegment);

            // Scenario 3
            ls0 = new LineSegment2D(new Point2D(0, 0), new Point2D(1, 0));
            ls1 = new LineSegment2D(new Point2D(0, 1), new Point2D(1, 1));
            shortestDistance = ls0.ShortestDistanceToOtherLineSegment(ls1, out shortestDistanceSegment);
            Assert.AreEqual(1, shortestDistance);
            Assert.AreEqual(0, shortestDistanceSegment.PointA.Y);
            Assert.AreEqual(1, shortestDistanceSegment.PointB.Y);

            // Scenario 4
            ls0 = new LineSegment2D(new Point2D(0, 0), new Point2D(2, 0));
            ls1 = new LineSegment2D(new Point2D(1, 3), new Point2D(1, 2));
            shortestDistance = ls0.ShortestDistanceToOtherLineSegment(ls1, out shortestDistanceSegment);
            Assert.AreEqual(2, shortestDistance);
            Assert.AreEqual(new LineSegment2D(new Point2D(1,0), new Point2D(1,2)), shortestDistanceSegment);
        }
    }
}