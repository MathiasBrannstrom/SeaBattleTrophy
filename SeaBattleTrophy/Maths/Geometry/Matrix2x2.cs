﻿using System;

namespace Maths.Geometry
{
    public struct Matrix2x2 : IEquatable<Matrix2x2>
    {
        public double V00 { get; }
        public double V10 { get; }
        public double V01 { get; }
        public double V11 { get; }

        public Matrix2x2(double v00, double v01, double v10, double v11)
        {
            V00 = v00;
            V10 = v10;
            V01 = v01;
            V11 = v11;
        }

        public static Matrix2x2 operator *(Matrix2x2 matrixL, Matrix2x2 matrixR)
        {
            return new Matrix2x2(
                matrixL.V00 * matrixR.V00 + matrixL.V01 * matrixR.V10,
                matrixL.V00 * matrixR.V01 + matrixL.V01 * matrixR.V11,
                matrixL.V10 * matrixR.V00 + matrixL.V11 * matrixR.V10,
                matrixL.V10 * matrixR.V01 + matrixL.V11 * matrixR.V11);
        }

        public double Determinant()
        {
            return V00 * V11 - V10 * V01;
        }

        public static Matrix2x2 Identity { get { return new Matrix2x2(1, 0, 0, 1); } }

        public bool Equals(Matrix2x2 other)
        {
            return V00.Equals(other.V00) && V01.Equals(other.V01) && V10.Equals(other.V10) && V11.Equals(other.V11);
        }

    }
}
