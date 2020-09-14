using System;
using System.ComponentModel.DataAnnotations;

namespace DIRT
{
    public class Matrix4x4
    {
        public double[,] m;
        
        public Matrix4x4()
        {
            m = new double[4, 4];
        }

        public Matrix4x4(double[,] mat)
        {
            m = mat;
        }


        public static Matrix4x4 projectionMatrix()
        {
            double far = Settings.zFar;
            double near = Settings.zNear;
            double a = Settings.aspectRatio;
            double fov = Settings.fov;

            Matrix4x4 m = new Matrix4x4();

            //m.m[0, 0] = a * (1 / Math.Tan(fov / 2));

            return new Matrix4x4(new double[,]
                {
                    {a * (1/Math.Tan(fov/2)),0,0,0 },
                    {0 ,1/Math.Tan(fov/2),0,0 },
                    {0 ,0 ,far / (far - near),1 },
                    {0 ,0 ,(-far*near) / (far - near),0 },
                });


        }

        public static Matrix4x4 rotationXMatrix(double rot)
        {
            Matrix4x4 rotMat = new Matrix4x4();
            rotMat.m[0, 0] = 1;
            rotMat.m[1, 1] = Math.Cos(rot);
            rotMat.m[1, 2] = Math.Sin(rot);
            rotMat.m[2, 1] =  -Math.Sin(rot);
            rotMat.m[2, 2] = Math.Cos(rot);
            rotMat.m[3, 3] = 1;

            return rotMat;
        }

        public static Matrix4x4 rotationYMatrix(double rot)
        {
            Matrix4x4 rotMat = new Matrix4x4();
            rotMat.m[0, 0] = Math.Cos(rot);
            rotMat.m[0, 2] = Math.Sin(rot);
            rotMat.m[2, 0] = -Math.Sin(rot);
            rotMat.m[1, 1] = 1;
            rotMat.m[2, 2] = Math.Cos(rot);
            rotMat.m[3, 3] = 1;

            return rotMat;
        }

        public static Matrix4x4 rotationZMatrix(double rot)
        {
            Matrix4x4 rotMat = new Matrix4x4();
            rotMat.m[0, 0] = Math.Cos(rot);
            rotMat.m[0, 1] = Math.Sin(rot);
            rotMat.m[1, 0] = -Math.Sin(rot);
            rotMat.m[1, 1] = Math.Cos(rot);
            rotMat.m[2, 2] = 1;
            rotMat.m[3, 3] = 1;

            return rotMat;
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
