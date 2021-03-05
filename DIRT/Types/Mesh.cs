using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DIRT.Types
{
    public class Mesh
    {
        public Vector position;
        public Vector rotation;
        public List<Triangle> tris;

        public Mesh(Vector pos,Vector rot)
        {
            tris = new List<Triangle>();
            position = pos;
            rotation = rot;
        }

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

        public void makeCubeTextured(float sizeX, float sizeY, float sizeZ,float texSX, float texSY, float texEX, float texEY)
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
        }

        public void makePyramid(float size)
        {
            Vector A = new Vector(-(size / 2), -(size / 2), -(size / 2));
            Vector B = new Vector((size / 2), -(size / 2), -(size / 2));
            Vector C = new Vector(0.5f * (size / 2), -(size / 2), 0.86603f * (size / 2));
            Vector D = new Vector(0.5f * (size / 2), 0.8165f * (size / 2), 0.62201f * (size / 2));

            tris.Add(new Triangle(A, B, C));
            tris.Add(new Triangle(A, D, B));
            tris.Add(new Triangle(A, C, D));
            tris.Add(new Triangle(B, D, C));
        }

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
