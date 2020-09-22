using Microsoft.VisualBasic;
using System;
using System.Runtime.InteropServices.ComTypes;

namespace DIRT
{
    public struct Triangle
    {
        public Vector[] points;

        public Triangle(Vector p1, Vector p2, Vector p3)
        {
            points = new Vector[3];
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

        private static bool sameSide(Vector p1,Vector p2, Vector A,Vector B)
        {
            Vector cp1 = Vector.cross(B - A, p1 - A);
            Vector cp2 = Vector.cross(B - A, p2 - A);
            return Vector.dot(cp1, cp2) >= 0;
        }

        public bool inside(Vector p)
        {
            Vector A = new Vector(points[0].x, points[0].y, 0);
            Vector B = new Vector(points[1].x, points[1].y, 0);
            Vector C = new Vector(points[2].x, points[2].y, 0);

            return sameSide(p, A, B, C) && sameSide(p, B, A, C) && sameSide(p, C, A, B);
        }

        public void renderTriangle(Vector pos, Vector rot)
        {
            Triangle t = new Triangle();
            t.points = new Vector[3];


            for (int i = 0; i < 3; i++)
            {
                t.points[i] = new Vector(points[i]);

                //t.points[i] *= Settings.scale;

                t.points[i] *= Matrix4x4.rotationXMatrix(rot.x);
                t.points[i] *= Matrix4x4.rotationYMatrix(rot.y);
                t.points[i] *= Matrix4x4.rotationZMatrix(rot.z);

                t.points[i] += pos;

                t.points[i] *= Matrix4x4.rotationXMatrix(Settings.globalRot.x);
                t.points[i] *= Matrix4x4.rotationYMatrix(Settings.globalRot.y);
                t.points[i] *= Matrix4x4.rotationZMatrix(Settings.globalRot.z);

                //t.points[i] += new Vector(0, 0, 3, 0);
            }

            double z = (t.points[0].z + t.points[1].z + t.points[2].z)/3;

            if (z < 0)
            {
                return;
            }

            for (int i = 0; i < 3; i++)
            {

                t.points[i] *= Matrix4x4.projectionMatrix();

                if (t.points[i].w != 0)
                {
                    t.points[i].x /= t.points[i].w / 5;
                    t.points[i].y /= t.points[i].w / 5;
                    t.points[i].z /= t.points[i].w / 5;
                }

                t.points[i] += new Vector(1, 0, 0, 0);
                t.points[i].x *= 0.5 * Settings.width;
                t.points[i].y *= 0.5 * Settings.height;


            }

            double b = Math.Round(((Vector.angleDist(Settings.light, t.nomal) + 1) / 2) * 8);

            Screen.drawTriangle(t, b);

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
