using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using DIRT.Types;

namespace DIRT.Types
{
    public struct Triangle
    {
        public Vector[] points;
        public Vector[] uv;

        /// <summary>textureless triangle constructor</summary>
        public Triangle(Vector p1, Vector p2, Vector p3)
        {
            points = new Vector[3];
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;

            uv = new Vector[3];
            uv[0] = new Vector(-1f, -1f);
            uv[1] = new Vector(-1f, -1f);
            uv[2] = new Vector(-1f, -1f);
        }

        /// <summary>textured triangle constructor</summary>
        public Triangle(Vector p1, Vector p2, Vector p3, Vector t1, Vector t2, Vector t3)
        {
            points = new Vector[3];
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;

            uv = new Vector[3];
            uv[0] = t1;
            uv[1] = t2;
            uv[2] = t3;
        }

        /// <summary>textureless triangle constructor</summary>
        public Triangle(Vector[] ps)
        {
            points = ps;
            uv = new Vector[3];
            uv[0] = new Vector(-1f, -1f);
            uv[1] = new Vector(-1f, -1f);
            uv[2] = new Vector(-1f, -1f);
        }

        /// <summary>textured triangle constructor</summary>
        public Triangle(Vector[] ps, Vector[] uvs)
        {
            points = ps;
            uv = uvs;
        }

        /// <summary>the normal of the triangle</summary>
        public Vector normal
        {
            get
            {
                Vector u = points[1] - points[0];
                Vector v = points[2] - points[0];

                return Vector.cross(u,v).normalized;
            }
        }

        /// <summary>the middle point of the triangle</summary>
        public Vector middle
        {
            get
            {
                return ((points[0] + points[1] + points[2]) / 3);
            }
        }

        /// <summary>triangles equal</summary>
        public static bool operator ==(Triangle t1, Triangle t2)
        {
            return (t1.points[0] == t2.points[0]) && (t1.points[1] == t2.points[1]) && (t1.points[2] == t2.points[2]);
        }

        /// <summary>triangles different</summary>
        public static bool operator !=(Triangle t1, Triangle t2)
        {
            return (t1.points[0] != t2.points[0]) || (t1.points[1] != t2.points[1]) || (t1.points[2] != t2.points[2]);
        }

        //create an GPU acceptable struct from the standard triangle struct
        internal tris toTris()
        {
            tris o = new tris();
            o.p1 = points[0].toVec();
            o.p2 = points[1].toVec();
            o.p3 = points[2].toVec();

            o.t1 = uv[0].toVec();
            o.t2 = uv[1].toVec();
            o.t3 = uv[2].toVec();

            o.dist = Vector.zero.toVec();

            o.visible = 1;
            return o;
        }
    }

    //internal structure for the GPU
    internal struct tris
    {
        public vec p1;
        public vec p2;
        public vec p3;

        public vec t1;
        public vec t2;
        public vec t3;

        public vec dist;

        public float visible;
    }
}