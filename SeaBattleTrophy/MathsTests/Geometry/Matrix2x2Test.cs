using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maths.Geometry;

namespace MathsTests.Geometry
{
    [TestClass]
    public class Matrix2x2Test
    {
        [TestMethod]
        public void MultiplicationWithIdentityTest()
        {
            var matrix = new Matrix2x2(1, 2, 3, 4);

            var resultL = matrix * Matrix2x2.Identity;
            var resultR = Matrix2x2.Identity * matrix;

            Assert.AreEqual(matrix, resultL);
            Assert.AreEqual(matrix, resultR);
        }

        [TestMethod]
        public void MultiplicationOfTwoMatricesTest()
        {
            var matrix0 = new Matrix2x2(1, 2, 3, 4);
            var matrix1 = new Matrix2x2(1, 10, 100, 1000);

            var resultL = matrix0 * matrix1;
            var resultR = matrix1 * matrix0;

            var expectedResultL = new Matrix2x2(201, 2010, 403, 4030);
            var expectedResultR = new Matrix2x2(31, 42, 3100, 4200);

            Assert.AreEqual(expectedResultL, resultL);
            Assert.AreEqual(expectedResultR, resultR);
        }
    }
}
