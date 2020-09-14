using System;
using System.Collections.Generic;

namespace DIRT
{
    class Mesh
    {
        public Vector position;
        public Vector rotation;
        List<Triangle> tris = new List<Triangle>();

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
            List<Triangle> renderedTris = new List<Triangle>();

            foreach (Triangle t in tris)
            {
                renderedTris.Add(t.renderTriangle(position, rotation));
            }

            foreach (Triangle r in renderedTris)
            {
                if (Vector.angleDist(Settings.camera, r.nomal) < 0)
                {
                    continue;
                }

                double b = (Vector.angleDist(Settings.light, r.nomal) + 1) * 2;
                
                /*
                Screen.drawLine(r.points[0], r.points[1], 6);
                Screen.drawLine(r.points[1], r.points[2], 6);
                Screen.drawLine(r.points[0], r.points[2], 6);*/

                Screen.drawTriangle(r,b);

            }

        }
    }
}
