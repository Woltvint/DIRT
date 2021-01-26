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

                    v.x = Convert.ToSingle(coords[1]);
                    v.y = Convert.ToSingle(coords[2]);
                    v.z = Convert.ToSingle(coords[3]);
                    /*
                    Random rnd = new Random();

                    float d = (float)rnd.NextDouble()*2f;
                    d -= 1f;
                    d /= 50f;

                    v.x += d;
                    v.y += d;
                    v.z += d;*/

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

        public List<Triangle> getTris()
        {
            object triLock = new object();

            List<Triangle> trisToRet = new List<Triangle>();
            /*
            foreach (Triangle t in tris)
            {
                Triangle tr = new Triangle(Vector.zero,Vector.zero,Vector.zero);

                for (int i = 0; i < 3; i++)
                {
                    tr.points[i] = t.points[i] * Matrix4x4.rotationXMatrix(rotation.x);
                    tr.points[i] = tr.points[i] * Matrix4x4.rotationYMatrix(rotation.y);
                    tr.points[i] = tr.points[i] * Matrix4x4.rotationZMatrix(rotation.z);

                    tr.points[i] += position;
                }

                trisToRet.Add(tr);
            }*/

            Parallel.ForEach(tris, (t) => {
                Triangle tr = new Triangle(Vector.zero, Vector.zero, Vector.zero);

                for (int i = 0; i < 3; i++)
                {
                    tr.points[i] = t.points[i] * Matrix4x4.rotationXMatrix(rotation.x);
                    tr.points[i] = tr.points[i] * Matrix4x4.rotationYMatrix(rotation.y);
                    tr.points[i] = tr.points[i] * Matrix4x4.rotationZMatrix(rotation.z);

                    tr.points[i] += position;
                }

                lock (triLock)
                {
                    trisToRet.Add(tr);
                }
                
            });

            return trisToRet;
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
