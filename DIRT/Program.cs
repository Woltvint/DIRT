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
            ground.makeFromOBJ("destroyer.obj");
            //ground.makeCube(3, 3, 3);
            //ground.makeCubeTextured(4, 4, 4, 48.1f, 16.1f, 111.9f, 79.9f);
            //ground.makeCubeTextured(4, 4, 4, 64.1f, 0.1f, 79.9f, 15.9f);

            //ground.makeCubeTextured(4, 4, 4, 0.1f, 80.1f, 127.9f, 207.9f);
            //ground.makeCubeTextured(4, 4, 4, 128.1f, 80.1f, 255.9f, 207.9f);
            /*Triangle t = new Triangle(new Vector(0, 0, 0), new Vector(4, 4, 0), new Vector(4, 0, 0), new Vector(0, 0), new Vector(16, 16), new Vector(16, 0));
            
            ground.tris.Add(t);*/
            ConsoleRenderer.Meshes.Add(ground);
            

            Vector light = new Vector(-1f, -1, -1f);
            //Vector light = new Vector(-1f, 0, 0);
            ConsoleRenderer.Lights.Add(light);
            float rot = -MathF.PI;
            /*
            List<Mesh> meshes = new List<Mesh>();

            Random rnd = new Random();

            for (int i = 0; i < 40; i++)
            {
                for (int u = 0; u < 20; u++)
                {
                    Mesh block = new Mesh(new Vector((i * 4)-(40*2), (u*4)-(20*2), 60), Vector.zero);

                    switch (rnd.Next(0, 3))
                    {
                        case 0:
                            block.makeCubeTextured(4, 4, 4, 16.1f, 0.1f, 31.9f, 15.9f);
                            break;
                        case 1:
                            block.makeCubeTextured(4, 4, 4, 48.1f, 0.1f, 63.9f, 15.9f);
                            break;
                        case 2:
                            block.makeCubeTextured(4, 4, 4, 63.1f, 0.1f, 79.9f, 15.9f);
                            break;
                    }

                    meshes.Add(block);
                }
            }
            ConsoleRenderer.Meshes.AddRange(meshes);*/
            /*
            List<Mesh> logo = new List<Mesh>();

            //D
            logo.Add(new Mesh(new Vector(0, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(0, 1, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(0, 2, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(0, 3, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(1, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(1, 3, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(2, 1, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(2, 2, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            //I
            logo.Add(new Mesh(new Vector(4, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(4, 3, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(5, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(5, 1, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(5, 2, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(5, 3, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(6, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(6, 3, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            //R
            logo.Add(new Mesh(new Vector(8, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(8, 1, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(8, 2, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(8, 3, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(9, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(9, 2, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(10, 1, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(10, 3, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            //T
            logo.Add(new Mesh(new Vector(12, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(13, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(13, 1, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(13, 2, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(13, 3, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));
            logo.Add(new Mesh(new Vector(14, 0, 10), Vector.zero).makeCubeTextured(1, 1, 1, 16.1f, 0.1f, 31.9f, 15.9f));

            Mesh rysboi = new Mesh(new Vector(0, -4, 10), Vector.zero);
            rysboi.makeCubeTextured(4, 4, 4, 0.1f, 80.1f, 127.9f, 207.9f);

            Mesh veda = new Mesh(new Vector(14, -4, 10), Vector.zero);
            veda.makeCubeTextured(4, 4, 4, 128.1f, 80.1f, 255.9f, 207.9f);

            ConsoleRenderer.Meshes.Add(rysboi);
            ConsoleRenderer.Meshes.Add(veda);
            ConsoleRenderer.Meshes.AddRange(logo);

            */



            Rectangle sc = new Rectangle();
            GetWindowRect(GetConsoleWindow(), ref sc);
            SetCursorPos((sc.Width / 2) + sc.Left, (sc.Height / 2) + sc.Top);
            Point mp = new Point();
            GetCursorPos(out mp);

            ConsoleRenderer.textureMap = (Bitmap)Image.FromFile("textureMap.png");

            bool capture = true;
            //ground.rotation.z = 10;
            while (true)
            {
                Thread.Sleep(16);
                rot += 0.01f;

                lock (ConsoleRenderer.renderLock)
                {
                    /*veda.rotation.x += 0.005f;
                    veda.rotation.y += 0.002f;
                    veda.rotation.z += 0.007f;

                    rysboi.rotation.x += 0.005f;
                    rysboi.rotation.y += 0.002f;
                    rysboi.rotation.z += 0.007f;*/

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


                    if (ConsoleRenderer.keyPressed(ConsoleKey.NumPad1))
                    {
                        Settings.screenMode = Settings.screenModes.trueColor;
                    }
                    if (ConsoleRenderer.keyPressed(ConsoleKey.NumPad2))
                    {
                        Settings.screenMode = Settings.screenModes.graySpeed;
                    }

                }

                
            

                

                
            }

            

        }
    }

}
