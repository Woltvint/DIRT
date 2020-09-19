using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIRT
{
    class Mesh
    {
        public Vector position;
        public Vector rotation;
        public List<Triangle> tris = new List<Triangle>();

        public Mesh(Vector pos,Vector rot)
        {
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

        public void renderMesh()
        {
            Triangle[] renderedTris = new Triangle[tris.Count];

            
            Parallel.ForEach(tris, (t) =>
            {
                Triangle r = t.renderTriangle(position, rotation);
                Screen.drawTriangle(r, (Vector.angleDist(Settings.light, r.nomal) + 1) * 2);
            });



            //Screen.drawTriangle(t, (Vector.angleDist(Settings.light, t.nomal) + 1) * 2);

            
        }
    }
}
