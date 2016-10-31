using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maths.Geometry.Tests
{
    [TestClass()]
    public class Line2DExtensionsTests
    {
        [TestMethod()]
        public void IntersectsOtherLineTest()
        {
            // Scenario 1
            var line0 = new Line2D(new Point2D(-1, 0), new Point2D(1, 0));
            var line1 = new Line2D(new Point2D(0, -1), new Point2D(0, 1));
            Point2D? intersectionPoint;
            var hasIntersection = line0.IntersectsOtherLine(line1, out intersectionPoint);
            Assert.IsTrue(hasIntersection);
            Assert.AreEqual(0, intersectionPoint.Value.X);
            Assert.AreEqual(0, intersectionPoint.Value.Y);

            // Scenario 2
            line0 = new Line2D(new Point2D(0, 1), new Point2D(1, 0));
            line1 = new Line2D(new Point2D(0, 0), new Point2D(1, 1));
            hasIntersection = line0.IntersectsOtherLine(line1, out intersectionPoint);
            Assert.IsTrue(hasIntersection);
            Assert.AreEqual(0.5f, intersectionPoint.Value.X);
            Assert.AreEqual(0.5f, intersectionPoint.Value.Y);

            // Scenario 3
            line0 = new Line2D(new Point2D(0, 0), new Point2D(1, 0));
            line1 = new Line2D(new Point2D(0, 1), new Point2D(1, 1));
            hasIntersection = line0.IntersectsOtherLine(line1, out intersectionPoint);
            Assert.IsFalse(hasIntersection);
            Assert.IsNull(intersectionPoint);

            // Scenario 4
            line0 = new Line2D(new Point2D(0, 0), new Point2D(1, 0));
            line1 = new Line2D(new Point2D(0.5f, 2), new Point2D(0.5f, 1));
            hasIntersection = line0.IntersectsOtherLine(line1, out intersectionPoint);
            Assert.IsTrue(hasIntersection);
            Assert.AreEqual(0.5f, intersectionPoint.Value.X);
            Assert.AreEqual(0, intersectionPoint.Value.Y);

        }
    }
}