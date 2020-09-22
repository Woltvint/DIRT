using System;

namespace DIRT
{
    public struct Vector
    {
        public double x, y, z, w;

        #region constructors

        public Vector(double _x)
        {
            x = _x;
            y = 0;
            z = 0;
            w = 1;
        }

        public Vector(double _x, double _y)
        {
            x = _x;
            y = _y;
            z = 0;
            w = 1;
        }

        public Vector(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
            w = 1;
        }

        public Vector(double _x, double _y, double _z, double _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public Vector(Vector v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
            w = v.w;
        }

        #endregion

        public double magnitude
        {
            get
            {
                return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2) + Math.Pow(w, 2));
            }
        }

        public Vector normalized
        {
            get
            {
                return this / magnitude;
            }
        }

        public static double distance(Vector from, Vector to)
        {
            return Math.Sqrt(Math.Pow(from.x - to.x,2) + Math.Pow(from.y - to.y, 2) + Math.Pow(from.z - to.z, 2) + Math.Pow(from.w - to.w, 2));
        }

        public static Vector cross(Vector v1, Vector v2)
        {
            Vector v = new Vector();

            v.x = (v1.y * v2.z) - (v1.z * v2.y);
            v.y = (v1.z * v2.x) - (v1.x * v2.z);
            v.z = (v1.x * v2.y) - (v1.y * v2.x);

            return v;
        }

        public static double dot(Vector v1, Vector v2)
        {
            return (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z) + (v1.w * v2.w);
        }

        public static double angleDist(Vector v1, Vector v2)
        {
            Vector A = v1.normalized;
            Vector B = v2.normalized;
            return dot(A, B);

            

            
        }

        #region operators

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.x, -v.y, -v.z, -v.w);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        public static Vector operator *(Vector v1, double m)
        {
            return new Vector(v1.x *m, v1.y * m, v1.z * m, v1.w * m);
        }

        public static Vector operator /(Vector v1, double m)
        {
            return new Vector(v1.x / m, v1.y / m, v1.z / m, v1.w / m);
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w);
        }

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

        public static Vector front = new Vector(0, 0, 1);
        public static Vector back = new Vector(0, 0, -1);
        public static Vector left = new Vector(-1, 0, 0);
        public static Vector right = new Vector(1, 0, 0);
        public static Vector up = new Vector(0, 1, 0);
        public static Vector down = new Vector(0, -1, 0);
        public static Vector zero = new Vector(0, 0, 0, 0);
    }
}
