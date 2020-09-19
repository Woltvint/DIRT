using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace DIRT
{
    class Program
    {

        static Thread screenThread;
        static void Main(string[] args)
        {
            Console.WriteLine("path: ");
            string path = Console.ReadLine();

            Mesh m = loadFromOBJ(path);
            
            screenThread = new Thread(Screen.draw);
            screenThread.Start();

            Thread.Sleep(10);

            Stopwatch sw = new Stopwatch();

            while (true)
            {
                if (Screen.frameReady)
                {
                    continue;
                }

                sw.Reset();
                sw.Start();

                //m.rotation.x += 0.005;
                m.rotation.y += 0.02;
                //m.rotation.z += 0.01;

                m.renderMesh();

                sw.Stop();

                Screen.fps = (int)sw.ElapsedMilliseconds;

                if (sw.ElapsedMilliseconds < 33)
                {
                    Thread.Sleep(Math.Abs(33-(int)sw.ElapsedMilliseconds));
                }

                Screen.frameReady = true;

                
                switch(Screen.getInput())
                {
                    case 'w':
                        Settings.scale += 0.1;
                        break;
                    case 's':
                        Settings.scale -= 0.1;
                        break;

                    case 'a':
                        m.position.x += 1;
                        break;
                    case 'd':
                        m.position.x -= 1;
                        break;
                    case 'q':
                        m.position.y += 1;
                        break;
                    case 'e':
                        m.position.y -= 1;
                        break;
                }

                
            }
        }

        static Mesh loadFromOBJ(string path)
        {
            Mesh m = new Mesh(new Vector(0,0,0),new Vector(0,0,Math.PI));

            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace("  ", " ");
            }
            
            List<Vector> verts = new List<Vector>();
            List<Triangle> tris = new List<Triangle>();

            foreach (string line in lines)
            {
                if (line.StartsWith("v "))
                {
                    string[] coords = line.Split(' ');
                    Vector v = new Vector();

                    v.x = Convert.ToDouble(coords[1])*10;
                    v.y = Convert.ToDouble(coords[2])*10;
                    v.z = Convert.ToDouble(coords[3])*10;

                    verts.Add(v);
                }

                if (line.StartsWith("f "))
                {
                    string[] ver = line.Split(' ');

                    Triangle t = new Triangle();

                    t.points[0] = verts[Convert.ToInt32( ver[1].Split('/')[0])-1];
                    t.points[1] = verts[Convert.ToInt32(ver[2].Split('/')[0])-1];
                    t.points[2] = verts[Convert.ToInt32(ver[3].Split('/')[0])-1];

                    tris.Add(t);
                }
            }

            m.tris = tris;

            return m;

        }
    }
}
