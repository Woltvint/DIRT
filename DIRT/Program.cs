using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using DIRT.Types;


namespace DIRT
{
    class Program
    {
        static void Main(string[] args)
        {
            DIRT.startRenderer();
            /*
            Mesh m = new Mesh(new Vector(0,0.5f,2,0), new Vector(0,0,MathF.PI,0));
            m.makeFromOBJ("./skull.obj");
            //m.rotation.z = MathF.PI/90;
            m.rotation.y = MathF.PI;

            DIRT.Meshes.Add(m);*/

            Mesh ground = new Mesh(new Vector(0, 0,2.5f, 0), new Vector(0, 0, MathF.PI, 0));
            //Mesh ground = new Mesh(new Vector(0, 0, 10, 0), new Vector(0, 0, MathF.PI, 0));
            //ground.makeFromOBJ("Skull.obj");
            //ground.makeFromOBJ("teapot.obj");
            //ground.makeFromOBJ("sphere.obj");
            ground.makeCube(1, 1, 1);

            DIRT.Meshes.Add(ground);

            Vector light = new Vector(-0.5f, -1, -0.25f);

            DIRT.Lights.Add(light);

            float rot = -MathF.PI;

            while (true)
            {
                Thread.Sleep(33);
                rot += 0.01f;

                lock (Renderer.renderLock)
                {
                    ground.rotation.x += -0.005f;
                    ground.rotation.y += 0.006f;
                    ground.rotation.z += 0.007f;
                    //DIRT.Lights[0] = new Vector(MathF.Cos(rot),-0.25f, MathF.Sin(rot));
                }

                char input = Screen.getInput();

                
                switch (input)
                {
                    case 'a':
                        lock (Renderer.renderLock)
                        {
                            Settings.cameraRot.y += 0.02f;
                        }
                        break;
                    case 'd':
                        lock (Renderer.renderLock)
                        {
                            Settings.cameraRot.y -= 0.02f;
                        }
                        break;

                    case 'w':
                        lock (Renderer.renderLock)
                        {
                            Renderer.eye +=  new Vector(0, 0, 0.1f) * Matrix4x4.rotationYMatrix(Settings.cameraRot.y);
                        }
                        break;
                    case 's':
                        lock (Renderer.renderLock)
                        {
                            Renderer.eye += new Vector(0, 0, -0.1f) * Matrix4x4.rotationYMatrix(Settings.cameraRot.y);
                        }
                        break;


                    case '1':
                        Settings.colorMode = Settings.colorModes.charPoolBrightness;
                        break;
                    case '2':
                        Settings.colorMode = Settings.colorModes.virtualTerminalColor;
                        break;
                    case '3':
                        Settings.frameUpdateMode = Settings.frameUpdateModes.writeAll;
                        break;
                    case '4':
                        Settings.frameUpdateMode = Settings.frameUpdateModes.writeChanges;
                        break;
                }

                

                
            }

            

        }
    }

}
