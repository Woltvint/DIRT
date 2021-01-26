using System;

namespace DIRT.Types
{
    public struct Matrix4x4
    {
        public float[,] m;

        public Matrix4x4(float[,] mat)
        {
            m = mat;
        }


        public static Matrix4x4 projectionMatrix()
        {
            float far = Settings.zFar;
            float near = Settings.zNear;
            float a = Settings.aspectRatio;
            float f = 1 / MathF.Tan(Settings.fov / 2);
            float q = far / (far - near);

            Matrix4x4 m = new Matrix4x4(new float[4, 4]);

            m.m[0, 0] = a * f;
            m.m[1, 1] = f;
            m.m[2, 2] = q;
            m.m[2, 3] = -near * q;
            m.m[3, 2] = 1;

            return m;
            /*
            return new Matrix4x4(new float[,]
                {
                    {a * (1/MathF.Tan(fov/2)),0,0,0 },
                    {0 ,1/MathF.Tan(fov/2),0,0 },
                    {0 ,0 ,far / (far - near),1 },
                    {0 ,0 ,(-far*near) / (far - near),0 },
                });*/


        }

        public static Matrix4x4 rotationXMatrix(float rot)
        {
            Matrix4x4 rotMat = new Matrix4x4(new float[4, 4]);
            rotMat.m[0, 0] = 1;
            rotMat.m[1, 1] = (float)MathF.Cos(rot);
            rotMat.m[1, 2] = MathF.Sin(rot);
            rotMat.m[2, 1] =  -MathF.Sin(rot);
            rotMat.m[2, 2] = MathF.Cos(rot);
            rotMat.m[3, 3] = 1;

            return rotMat;
        }

        public static Matrix4x4 rotationYMatrix(float rot)
        {
            Matrix4x4 rotMat = new Matrix4x4(new float[4, 4]);
            rotMat.m[0, 0] = MathF.Cos(rot);
            rotMat.m[0, 2] = MathF.Sin(rot);
            rotMat.m[2, 0] = -MathF.Sin(rot);
            rotMat.m[1, 1] = 1;
            rotMat.m[2, 2] = MathF.Cos(rot);
            rotMat.m[3, 3] = 1;

            return rotMat;
        }

        public static Matrix4x4 rotationZMatrix(float rot)
        {
            Matrix4x4 rotMat = new Matrix4x4(new float[4, 4]);
            rotMat.m[0, 0] = MathF.Cos(rot);
            rotMat.m[0, 1] = MathF.Sin(rot);
            rotMat.m[1, 0] = -MathF.Sin(rot);
            rotMat.m[1, 1] = MathF.Cos(rot);
            rotMat.m[2, 2] = 1;
            rotMat.m[3, 3] = 1;

            return rotMat;
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 matrix = new Matrix4x4(new float[4, 4]);
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
