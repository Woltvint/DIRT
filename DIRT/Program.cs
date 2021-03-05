using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using DIRT.Types;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

namespace DIRT
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point pos);

        [DllImport("user32.dll")]
        public static extern long GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        

        static void Main(string[] args)
        {

            ConsoleRenderer.startRenderer();
            /*
            Mesh m = new Mesh(new Vector(0,0.5f,2,0), new Vector(0,0,MathF.PI,0));
            m.makeFromOBJ("./skull.obj");
            //m.rotation.z = MathF.PI/90;
            m.rotation.y = MathF.PI;

            DIRT.Meshes.Add(m);*/

            //Mesh ground = new Mesh(new Vector(0, 0,100, 0), new Vector(0, 0, 0, 0));
            Mesh ground = new Mesh(new Vector(0, 0, 7, 0), new Vector(0, 0, MathF.PI, 0));
            //ground.makeFromOBJ("Skull.obj");
            //ground.makeFromOBJ("teapot.obj");
            //ground.makeFromOBJ("sphere.obj");
            //ground.makeFromOBJ("destroyer.obj");
            //ground.makeCube(3, 3, 3);
            //ground.makeCubeTextured(4, 4, 4, 48.1f, 16.1f, 111.9f, 79.9f);
            ground.makeCubeTextured(4, 4, 4, 16.1f, 0.1f, 31.9f, 15.9f);

            //ground.makeCubeTextured(4, 4, 4, 0.1f, 80.1f, 127.9f, 207.9f);
            /*Triangle t = new Triangle(new Vector(0, 0, 0), new Vector(4, 4, 0), new Vector(4, 0, 0), new Vector(0, 0), new Vector(16, 16), new Vector(16, 0));

            

            ground.tris.Add(t);*/


            ConsoleRenderer.Meshes.Add(ground);

            Vector light = new Vector(-1f, -1, -1f);
            //Vector light = new Vector(-1f, 0, 0);
            ConsoleRenderer.Lights.Add(light);

            float rot = -MathF.PI;

            Rectangle sc = new Rectangle();
            GetWindowRect(GetConsoleWindow(), ref sc);
            SetCursorPos((sc.Width / 2) + sc.Left, (sc.Height / 2) + sc.Top);
            Point mp = new Point();
            GetCursorPos(out mp);

            ConsoleRenderer.textureMap = (Bitmap)Image.FromFile("textureMap.png");

            bool capture = false;
            //ground.rotation.z = 10;
            while (true)
            {
                Thread.Sleep(16);
                rot += 0.01f;

                lock (ConsoleRenderer.renderLock)
                {
                    ground.rotation.x += 0.005f;
                    ground.rotation.y += 0.002f;
                    ground.rotation.z += 0.007f;
                    //DIRT.Lights[0] = new Vector(MathF.Cos(rot), -0.25f, MathF.Sin(rot));

                    if (capture)
                    {
                        GetCursorPos(out mp);

                        int x = (sc.Width / 2) + sc.Left - mp.X;
                        int y = (sc.Height / 2) + sc.Top - mp.Y;
                        Settings.cameraRot.y += (float)x / 1500f;
                        Settings.cameraRot.x += (float)y / 1500f;

                        SetCursorPos((sc.Width / 2) + sc.Left, (sc.Height / 2) + sc.Top);
                    }
                    
                    if (ConsoleRenderer.keyDown(ConsoleKey.Escape))
                    {
                        capture = false;
                    }


                    if (ConsoleRenderer.keyPressed(ConsoleKey.W))
                    {
                        Settings.camera += new Vector(MathF.Sin(-Settings.cameraRot.y), MathF.Sin(-Settings.cameraRot.x), MathF.Cos(-Settings.cameraRot.y)) / 10f;
                    }
                    if (ConsoleRenderer.keyPressed(ConsoleKey.S))
                    {
                        Settings.camera -= new Vector(MathF.Sin(-Settings.cameraRot.y), MathF.Sin(-Settings.cameraRot.x), MathF.Cos(-Settings.cameraRot.y)) / 10f;
                    }

                    if (ConsoleRenderer.keyPressed(ConsoleKey.E))
                    {
                        Settings.camera -= new Vector(0, 0.1f, 0);
                    }
                    if (ConsoleRenderer.keyPressed(ConsoleKey.Q))
                    {
                        Settings.camera += new Vector(0, 0.1f, 0);
                    }

                }

                
            

                

                
            }

            

        }
    }

}
