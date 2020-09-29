using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;

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

        public Vector normal
        {
            get
            {
                Vector u = points[1] - points[0];
                Vector v = points[2] - points[0];

                return Vector.cross(u,v).normalized;
            }
        }

        public Vector middle
        {
            get
            {
                return ((points[0] + points[1] + points[2]) / 3);
            }
        }

        public bool intersects(Vector origin,Vector dir)
        {
            Vector N = normal;
            
            if (Vector.angleDist(dir, N) > 0)
            {
                return false;
            }

            double NdotRayDir = Vector.dot(N,dir);
            
            if (Math.Abs(NdotRayDir) < 0.0001)
            {
                return false;
            }

            Vector A = points[0];
            Vector B = points[1];
            Vector C = points[2];

            double D = Vector.dot(N, A);

            double t = (Vector.dot(N, origin) - D) / -NdotRayDir;

            if (t < 0)
            {
                return false;
            }

            Vector P = origin + (dir * t);

            Vector c;

            Vector Edge0 = B - A;
            Vector AP = P - A;

            c = Vector.cross(Edge0,AP);

            if (Vector.dot(N,c) < 0)
            {
                return false;
            }


            Vector Edge1 = C - B;
            Vector BP = P - B;

            c = Vector.cross(Edge1, BP);

            if (Vector.dot(N, c) < 0)
            {
                return false;
            }


            Vector Edge2 = A - C;
            Vector CP = P - C;

            c = Vector.cross(Edge2, CP);

            if (Vector.dot(N, c) < 0)
            {
                return false;
            }

            return true;
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

            double b = Math.Round(((Vector.angleDist(Settings.light, t.normal) + 1) / 2) * 8);

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
