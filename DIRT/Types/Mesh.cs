﻿using System;
using System.Collections.Generic;
using System.IO;

namespace DIRT.Types
{
    public class Mesh
    {
        public Vector position;
        public Vector rotation;
        public List<Triangle> tris;

        /// <summary>Creates a new mesh with position pos and rotation rot</summary>
        /// <param name="pos">the position vector for the new mesh</param>
        /// <param name="rot">the rotation vector for the new mesh</param>
        public Mesh(Vector pos,Vector rot)
        {
            tris = new List<Triangle>();
            position = pos;
            rotation = rot;
        }

        /// <summary>creates a cuboid of the size specified</summary>
        /// <param name="sizeX">width</param>
        /// <param name="sizeY">height</param>
        /// <param name="sizeZ">depth</param>
        public void makeCube(float sizeX, float sizeY, float sizeZ)
        {
            //front
            tris.Add(new Triangle(new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), -(sizeZ / 2))));
            tris.Add(new Triangle(new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), -(sizeY / 2), -(sizeZ / 2))));

            //back
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), (sizeZ / 2))));
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), (sizeZ / 2))));

            //left
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2))));
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2))));

            //right
            tris.Add(new Triangle(new Vector(-(sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2))));
            tris.Add(new Triangle(new Vector(-(sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2))));

            //top
            tris.Add(new Triangle(new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2))));
            tris.Add(new Triangle(new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), -(sizeZ / 2))));

            //bottom
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2))));
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), -(sizeY / 2), -(sizeZ / 2))));
        }

        /// <summary>creates a textured cuboid of the size specified</summary>
        /// <param name="sizeX">width</param>
        /// <param name="sizeY">height</param>
        /// <param name="sizeZ">depth</param>
        /// <param name="texSX">texture start X</param>
        /// <param name="texSY">texture start Y</param>
        /// <param name="texEX">texture end X</param>
        /// <param name="texEY">texture end Y</param>
        public Mesh makeCubeTextured(float sizeX, float sizeY, float sizeZ,float texSX, float texSY, float texEX, float texEY)
        {
            //front
            tris.Add(new Triangle(new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), -(sizeZ / 2))
                , new Vector(texSX, texSY), new Vector(texSX, texEY), new Vector(texEX, texEY)
            ));
            tris.Add(new Triangle(new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), -(sizeY / 2), -(sizeZ / 2))
                , new Vector(texSX, texSY), new Vector(texEX, texEY), new Vector(texEX, texSY)
            ));

            //back
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), (sizeZ / 2))
                , new Vector(texEX, texSY), new Vector(texEX, texEY), new Vector(texSX, texEY)
            ));
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), (sizeZ / 2))
                , new Vector(texEX, texSY), new Vector(texSX, texEY), new Vector(texSX, texSY)
            ));

            //left
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2))
                , new Vector(texSX, texSY), new Vector(texEX, texSY), new Vector(texEX, texEY)
            ));
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2))
                , new Vector(texSX, texSY), new Vector(texEX, texEY), new Vector(texSX, texEY)
            ));

            //right
            tris.Add(new Triangle(new Vector(-(sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2))
                , new Vector(texSX, texEY), new Vector(texEX, texEY), new Vector(texEX, texSY)
            ));
            tris.Add(new Triangle(new Vector(-(sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2))
                , new Vector(texSX, texEY), new Vector(texEX, texSY), new Vector(texSX, texSY)
            ));

            //top
            tris.Add(new Triangle(new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector(-(sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2))
                , new Vector(texSX, texSY), new Vector(texSX, texEY), new Vector(texEX, texEY)
            ));
            tris.Add(new Triangle(new Vector(-(sizeX / 2), (sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), (sizeZ / 2)), new Vector((sizeX / 2), (sizeY / 2), -(sizeZ / 2))
                , new Vector(texSX, texSY), new Vector(texEX, texEY), new Vector(texEX, texSY)
            ));

            //bottom
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2))
                , new Vector(texEX, texEY), new Vector(texSX, texEY), new Vector(texSX, texSY)
            ));
            tris.Add(new Triangle(new Vector((sizeX / 2), -(sizeY / 2), (sizeZ / 2)), new Vector(-(sizeX / 2), -(sizeY / 2), -(sizeZ / 2)), new Vector((sizeX / 2), -(sizeY / 2), -(sizeZ / 2))
                , new Vector(texEX, texEY), new Vector(texSX, texSY), new Vector(texEX, texSY)
            ));

            return this;
        }

        public Mesh makeCubeTexturedOffset(Vector size, float texSX, float texSY, float texEX, float texEY, Vector off)
        {
            List<Triangle> tri = new List<Triangle>();
            //front
            tri.Add(new Triangle(new Vector(-(size.x / 2), -(size.y / 2), -(size.z / 2)), new Vector(-(size.x / 2), (size.y / 2), -(size.z / 2)), new Vector((size.x / 2), (size.y / 2), -(size.z / 2))
                , new Vector(texSX, texSY), new Vector(texSX, texEY), new Vector(texEX, texEY)
            ));
            tri.Add(new Triangle(new Vector(-(size.x / 2), -(size.y / 2), -(size.z / 2)), new Vector((size.x / 2), (size.y / 2), -(size.z / 2)), new Vector((size.x / 2), -(size.y / 2), -(size.z / 2))
                , new Vector(texSX, texSY), new Vector(texEX, texEY), new Vector(texEX, texSY)
            ));

            //back
            tri.Add(new Triangle(new Vector((size.x / 2), -(size.y / 2), (size.z / 2)), new Vector((size.x / 2), (size.y / 2), (size.z / 2)), new Vector(-(size.x / 2), (size.y / 2), (size.z / 2))
                , new Vector(texEX, texSY), new Vector(texEX, texEY), new Vector(texSX, texEY)
            ));
            tri.Add(new Triangle(new Vector((size.x / 2), -(size.y / 2), (size.z / 2)), new Vector(-(size.x / 2), (size.y / 2), (size.z / 2)), new Vector(-(size.x / 2), -(size.y / 2), (size.z / 2))
                , new Vector(texEX, texSY), new Vector(texSX, texEY), new Vector(texSX, texSY)
            ));

            //left
            tri.Add(new Triangle(new Vector((size.x / 2), -(size.y / 2), -(size.z / 2)), new Vector((size.x / 2), (size.y / 2), -(size.z / 2)), new Vector((size.x / 2), (size.y / 2), (size.z / 2))
                , new Vector(texSX, texSY), new Vector(texEX, texSY), new Vector(texEX, texEY)
            ));
            tri.Add(new Triangle(new Vector((size.x / 2), -(size.y / 2), -(size.z / 2)), new Vector((size.x / 2), (size.y / 2), (size.z / 2)), new Vector((size.x / 2), -(size.y / 2), (size.z / 2))
                , new Vector(texSX, texSY), new Vector(texEX, texEY), new Vector(texSX, texEY)
            ));

            //right
            tri.Add(new Triangle(new Vector(-(size.x / 2), -(size.y / 2), (size.z / 2)), new Vector(-(size.x / 2), (size.y / 2), (size.z / 2)), new Vector(-(size.x / 2), (size.y / 2), -(size.z / 2))
                , new Vector(texSX, texEY), new Vector(texEX, texEY), new Vector(texEX, texSY)
            ));
            tri.Add(new Triangle(new Vector(-(size.x / 2), -(size.y / 2), (size.z / 2)), new Vector(-(size.x / 2), (size.y / 2), -(size.z / 2)), new Vector(-(size.x / 2), -(size.y / 2), -(size.z / 2))
                , new Vector(texSX, texEY), new Vector(texEX, texSY), new Vector(texSX, texSY)
            ));

            //top
            tri.Add(new Triangle(new Vector(-(size.x / 2), (size.y / 2), -(size.z / 2)), new Vector(-(size.x / 2), (size.y / 2), (size.z / 2)), new Vector((size.x / 2), (size.y / 2), (size.z / 2))
                , new Vector(texSX, texSY), new Vector(texSX, texEY), new Vector(texEX, texEY)
            ));
            tri.Add(new Triangle(new Vector(-(size.x / 2), (size.y / 2), -(size.z / 2)), new Vector((size.x / 2), (size.y / 2), (size.z / 2)), new Vector((size.x / 2), (size.y / 2), -(size.z / 2))
                , new Vector(texSX, texSY), new Vector(texEX, texEY), new Vector(texEX, texSY)
            ));

            //bottom
            tri.Add(new Triangle(new Vector((size.x / 2), -(size.y / 2), (size.z / 2)), new Vector(-(size.x / 2), -(size.y / 2), (size.z / 2)), new Vector(-(size.x / 2), -(size.y / 2), -(size.z / 2))
                , new Vector(texEX, texEY), new Vector(texSX, texEY), new Vector(texSX, texSY)
            ));
            tri.Add(new Triangle(new Vector((size.x / 2), -(size.y / 2), (size.z / 2)), new Vector(-(size.x / 2), -(size.y / 2), -(size.z / 2)), new Vector((size.x / 2), -(size.y / 2), -(size.z / 2))
                , new Vector(texEX, texEY), new Vector(texSX, texSY), new Vector(texEX, texSY)
            ));

            for (int i = 0; i < tri.Count; i++)
            {
                tri[i].points[0] += off;
                tri[i].points[1] += off;
                tri[i].points[2] += off;
            }

            tris.AddRange(tri);

            return this;
        }

        /// <summary>loads an obj file to the mesh</summary>
        /// <param name="path">path to the obj file</param>
        public int makeFromOBJ(string path)
        {
            if (!File.Exists(path))
            {
                return -1;
            }

            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace("  ", " ");
            }

            List<Vector> ver = new List<Vector>();
            List<Triangle> tri = new List<Triangle>();

            foreach (string line in lines)
            {
                if (line.StartsWith("v "))
                {
                    string[] coords = line.Split(' ');
                    Vector v = new Vector();

                    v.x = Convert.ToSingle(coords[1]);
                    v.y = Convert.ToSingle(coords[2]);
                    v.z = Convert.ToSingle(coords[3]);

                    ver.Add(v);
                }

                if (line.StartsWith("f "))
                {
                    string[] v = line.Split(' ');

                    Vector A = ver[Convert.ToInt32(v[1].Split('/')[0]) - 1];
                    Vector B = ver[Convert.ToInt32(v[2].Split('/')[0]) - 1];
                    Vector C = ver[Convert.ToInt32(v[3].Split('/')[0]) - 1];

                    tri.Add(new Triangle(A, B, C));

                }
            }

            tris = tri;

            return tris.Count;
        }
    }
}
