using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace DIRT
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

        public void makeCube(double size)
        {
            //front
            tris.Add(new Triangle(new Vector(-(size/2), -(size/2), -(size/2)), new Vector(-(size/2), (size/2), -(size/2)), new Vector((size/2), (size/2), -(size/2))));
            tris.Add(new Triangle(new Vector(-(size/2), -(size/2), -(size/2)), new Vector((size/2), (size/2), -(size/2)), new Vector((size/2), -(size/2), -(size/2))));

            //back
            tris.Add(new Triangle(new Vector((size/2), -(size/2), (size/2)), new Vector((size/2), (size/2), (size/2)), new Vector(-(size/2), (size/2), (size/2))));
            tris.Add(new Triangle(new Vector((size/2), -(size/2), (size/2)), new Vector(-(size/2), (size/2), (size/2)), new Vector(-(size/2), -(size/2), (size/2))));

            //left
            tris.Add(new Triangle(new Vector((size/2), -(size/2), -(size/2)), new Vector((size/2), (size/2), -(size/2)), new Vector((size/2), (size/2), (size/2))));
            tris.Add(new Triangle(new Vector((size/2), -(size/2), -(size/2)), new Vector((size/2), (size/2), (size/2)), new Vector((size/2), -(size/2), (size/2))));

            //right
            tris.Add(new Triangle(new Vector(-(size/2), -(size/2), (size/2)), new Vector(-(size/2), (size/2), (size/2)), new Vector(-(size/2), (size/2), -(size/2))));
            tris.Add(new Triangle(new Vector(-(size/2), -(size/2), (size/2)), new Vector(-(size/2), (size/2), -(size/2)), new Vector(-(size/2), -(size/2), -(size/2))));

            //top
            tris.Add(new Triangle(new Vector(-(size/2), (size/2), -(size/2)), new Vector(-(size/2), (size/2), (size/2)), new Vector((size/2), (size/2), (size/2))));
            tris.Add(new Triangle(new Vector(-(size/2), (size/2), -(size/2)), new Vector((size/2), (size/2), (size/2)), new Vector((size/2), (size/2), -(size/2))));

            //bottom
            tris.Add(new Triangle(new Vector((size/2), -(size/2), (size/2)), new Vector(-(size/2), -(size/2), (size/2)), new Vector(-(size/2), -(size/2), -(size/2))));
            tris.Add(new Triangle(new Vector((size/2), -(size/2), (size/2)), new Vector(-(size/2), -(size/2), -(size/2)), new Vector((size/2), -(size/2), -(size/2))));
        }

        public void makePyramid(double size)
        {
            Vector A = new Vector(-(size / 2), -(size / 2), -(size / 2));
            Vector B = new Vector((size / 2), -(size / 2), -(size / 2));
            Vector C = new Vector(0.5 * (size / 2), -(size / 2), 0.86603 * (size / 2));
            Vector D = new Vector(0.5 * (size / 2), 0.8165 * (size / 2), 0.62201 * (size / 2));

            tris.Add(new Triangle(A, B, C));
            tris.Add(new Triangle(A, D, B));
            tris.Add(new Triangle(A, C, D));
            tris.Add(new Triangle(B, D, C));
        }

        public void makeFromOBJ(string path)
        {
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

                    v.x = Convert.ToDouble(coords[1]);
                    v.y = Convert.ToDouble(coords[2]);
                    v.z = Convert.ToDouble(coords[3]);

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

            
        }

        public void renderMesh()
        {
            Parallel.ForEach(tris, (t) =>
            {
                t.renderTriangle(position, rotation);
            }); 
        }
    }
}
