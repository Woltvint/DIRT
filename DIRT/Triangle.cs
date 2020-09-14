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
            }


            return t;
        }
    }
}
