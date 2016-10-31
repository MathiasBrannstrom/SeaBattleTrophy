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
    public class Vector2DExtensionsTests
    {
        [TestMethod()]
        public void IsParallelWithTest()
        {
            // Parallel vectors
            var v0 = new Vector2D(0, 1);
            var v1 = new Vector2D(0, 3);
            Assert.IsTrue(v0.IsParallelWith(v1));

            // Parallel but flipped vectors
            v0 = new Vector2D(0, 1);
            v1 = new Vector2D(0, -3);
            Assert.IsTrue(v0.IsParallelWith(v1));

            // Parallel vectors with values in both coordinates
            v0 = new Vector2D(1, 2);
            v1 = new Vector2D(2, 4);
            Assert.IsTrue(v0.IsParallelWith(v1));

            // Non-parallel vectors
            v0 = new Vector2D(1, 1);
            v1 = new Vector2D(2, 1);
            Assert.IsFalse(v0.IsParallelWith(v1));
        }
    }
}