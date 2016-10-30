using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaBattleTrophyGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SeaBattleTrophyGame.Tests
{
    [TestClass()]
    public class LandTests
    {
        [TestMethod()]
        public void DistanceToPointTest()
        {
            var points = new[] { new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1), new Point2D(1, 0) };

            var land = new Land(points);

            Assert.AreEqual(0, land.DistanceToPoint(new Point2D(0, 1)));
            Assert.AreEqual(1, land.DistanceToPoint(new Point2D(2, 1)));
            Assert.AreEqual(1, land.DistanceToPoint(new Point2D(2, 0.5f)));
            Assert.AreEqual(Math.Sqrt(2), land.DistanceToPoint(new Point2D(2, 2)));

        }
    }
}