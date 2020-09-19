using System;

namespace DIRT
{
    class Triangle
    {
        public Vector[] points = new Vector[3];

        public Triangle()
        {
        }

        public Triangle(Vector p1, Vector p2, Vector p3)
        {
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
        }

        public Triangle(Vector[] ps)
        {
            points = ps;
        }

        public Vector nomal
        {
            get
            {
                Vector normal = new Vector(0, 0, 0);
                Vector u = points[1] - points[0];
                Vector v = points[2] - points[0];

                normal.x = (u.y * v.z) - (u.z * v.y);
                normal.y = (u.z * v.x) - (u.x * v.z);
                normal.z = (u.x * v.y) - (u.y * v.x);

                return normal.normalized;
            }
        }

        public bool intersects(Vector p1,Vector p2)
        {
            Vector dir = p2 - p1;
            Vector A = p1 + (dir * 1000);
            Vector B = p1 - (dir * 1000);

            if (signedVolume(A,points[0],points[1],points[2]) != signedVolume(B, points[0], points[1], points[2]))
            {
                if (signedVolume(A,B,points[0],points[1]) == signedVolume(A, B, points[1], points[2]) && signedVolume(A, B, points[0], points[1]) == signedVolume(A, B, points[0], points[2]))
                {
                    return true;
                }
            }
            return false;
        }

        private int signedVolume(Vector A, Vector B, Vector C, Vector D)
        {
            return Math.Sign(Vector.dot(Vector.cross(B - A, C - A), D - A));
        }

        public Triangle renderTriangle(Vector pos, Vector rot)
        {
            Triangle t = new Triangle();

            for (int i = 0; i < 3; i++)
            {
                t.points[i] = points[i];

                t.points[i] *= Settings.scale;

                t.points[i] *= Matrix4x4.rotationXMatrix(rot.x);
                t.points[i] *= Matrix4x4.rotationYMatrix(rot.y);
                t.points[i] *= Matrix4x4.rotationZMatrix(rot.z);
                
                t.points[i] *= Matrix4x4.projectionMatrix();

                t.points[i] += pos;

                /*t.points[i] *= Matrix4x4.rotationXMatrix(Settings.globalRot.x);
                t.points[i] *= Matrix4x4.rotationYMatrix(Settings.globalRot.y);
                t.points[i] *= Matrix4x4.rotationZMatrix(Settings.globalRot.z);*/
            }



            return t;

        }

        public static bool operator ==(Triangle t1, Triangle t2)
        {
            return (t1.points[0] == t2.points[0]) && (t1.points[1] == t2.points[1]) && (t1.points[2] == t2.points[2]);
        }

        public static bool operator !=(Triangle t1, Triangle t2)
        {
            return (t1.points[0] != t2.points[0]) || (t1.points[1] != t2.points[1]) || (t1.points[2] != t2.points[2]);
        }
    }
}
