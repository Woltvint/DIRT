using System;

namespace DIRT.Types
{
    public struct Vector
    {
        public float x, y, z, w;

        #region constructors

        /// <summary>Creates a new 1D Vector</summary>
        public Vector(float _x)
        {
            x = _x;
            y = 0;
            z = 0;
            w = 0;
        }

        /// <summary>Creates a new 2D Vector</summary>
        public Vector(float _x, float _y)
        {
            x = _x;
            y = _y;
            z = 0;
            w = 0;
        }

        /// <summary>Creates a new 3D Vector</summary>
        public Vector(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
            w = 0;
        }

        /// <summary>Creates a new 4D Vector</summary>
        public Vector(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        #endregion

        /// <summary>the length of the vector</summary>
        public float magnitude
        {
            get
            {
                return MathF.Sqrt((x*x) + (y*y) + (z*z) + (w*w));
            }
        }

        /// <summary>normalized value of the vector</summary>
        public Vector normalized
        {
            get
            {
                return this / magnitude;
            }
        }

        /// <summary>calculates the distance between two points represented by two vectors</summary>
        /// <param name="from">the first point</param>
        /// <param name="to">the second point</param>
        public static float distance(Vector from, Vector to)
        {
            float _x = from.x - to.x;
            float _y = from.y - to.y;
            float _z = from.z - to.z;
            float _w = from.w - to.w;

            return MathF.Sqrt((_x*_x) + (_y*_y) + (_z*_z) + (_w*_w));
        }

        /// <summary>calculates the cross product of two vectors</summary>
        /// <param name="v1">the first vector</param>
        /// <param name="v2">the second vector</param>
        public static Vector cross(Vector v1, Vector v2)
        {
            Vector v = new Vector();

            v.x = (v1.y * v2.z) - (v1.z * v2.y);
            v.y = (v1.z * v2.x) - (v1.x * v2.z);
            v.z = (v1.x * v2.y) - (v1.y * v2.x);

            return v;
        }

        /// <summary>calculates the dot product of two vectors</summary>
        /// <param name="v1">the first vector</param>
        /// <param name="v2">the second vector</param>
        public static float dot(Vector v1, Vector v2)
        {
            return (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z) + (v1.w * v2.w);
        }

        /// <summary>calculates the angle distance between two vectors</summary>
        /// <param name="v1">the first vector</param>
        /// <param name="v2">the second vector</param>
        /// <returns>a number from -1 to 1. (-1 = 180°) (0 = 90°) (1 = 0°)</returns>
        public static float angleDist(Vector v1, Vector v2)
        {
            Vector A = v1.normalized;
            Vector B = v2.normalized;
            return dot(A, B);
        }

        #region operators

        /// <summary>vector addition</summary>
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        /// <summary>vector negative</summary>
        public static Vector operator -(Vector v)
        {
            return new Vector(-v.x, -v.y, -v.z, -v.w);
        }

        /// <summary>vector subtraction</summary>
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        /// <summary>vector scalar multiplication</summary>
        public static Vector operator *(Vector v1, float m)
        {
            return new Vector(v1.x *m, v1.y * m, v1.z * m, v1.w * m);
        }

        /// <summary>vector scalar division</summary>
        public static Vector operator /(Vector v1, float m)
        {
            return new Vector(v1.x / m, v1.y / m, v1.z / m, v1.w / m);
        }

        /// <summary>vectors equal</summary>
        public static bool operator ==(Vector v1, Vector v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);
        }

        /// <summary>vectors different</summary>
        public static bool operator !=(Vector v1, Vector v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w);
        }

        /// <summary>vector matrix multiplication</summary>
        public static Vector operator *(Vector i, Matrix4x4 m)
        {
            Vector o = new Vector(0, 0, 0, 0);

            o.x = i.x * m.m[0, 0] + i.y * m.m[1, 0] + i.z * m.m[2, 0] + i.w * m.m[3, 0];
            o.y = i.x * m.m[0, 1] + i.y * m.m[1, 1] + i.z * m.m[2, 1] + i.w * m.m[3, 1];
            o.z = i.x * m.m[0, 2] + i.y * m.m[1, 2] + i.z * m.m[2, 2] + i.w * m.m[3, 2];
            o.w = i.x * m.m[0, 3] + i.y * m.m[1, 3] + i.z * m.m[2, 3] + i.w * m.m[3, 3];
            
            return o;
        }

        #endregion
        /// <summary>(0, 0, 1)</summary>
        public static Vector front = new Vector(0, 0, 1);
        /// <summary>(0, 0,-1)</summary>
        public static Vector back = new Vector(0, 0, -1);
        /// <summary>(-1, 0, 0)</summary>
        public static Vector left = new Vector(-1, 0, 0);
        /// <summary>(1, 0, 0)</summary>
        public static Vector right = new Vector(1, 0, 0);
        /// <summary>(0, 1, 0)</summary>
        public static Vector up = new Vector(0, 1, 0);
        /// <summary>(0,-1, 0)</summary>
        public static Vector down = new Vector(0, -1, 0);
        /// <summary>(0, 0, 0)</summary>
        public static Vector zero = new Vector(0, 0, 0, 0);

        //create an GPU acceptable struct from the standard vector struct
        internal vec toVec()
        {
            vec o = new vec();
            o.x = x;
            o.y = y;
            o.z = z;

            return o;
        }
    }

    //internal structure for the GPU
    internal struct vec
    {
        public float x;
        public float y;
        public float z;
    }
}