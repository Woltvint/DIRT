using System;

namespace DIRT
{
    class Matrix4x4
    {
        public double[,] m;

        public Matrix4x4()
        {
            m = new double[4, 4];
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 matrix = new Matrix4x4();
            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    matrix.m[r, c] = m1.m[r, 0] * m2.m[0, c] + m1.m[r, 1] * m2.m[1, c] + m1.m[r, 2] * m2.m[2, c] + m1.m[r, 3] * m2.m[3, c];
                }
            }
            return matrix;
        }
    }
}
