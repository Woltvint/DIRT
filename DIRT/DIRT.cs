using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DIRT
{
    public class DIRT
    {
        public static List<Mesh> Meshes;

        public static readonly object renderLock = new object();

        private static int targetFps = 60;

        private static Thread screenThread;
        private static Thread renderingThread;



        public static void startRenderer()
        {
            Meshes = new List<Mesh>();

            screenThread = new Thread(Screen.draw);
            renderingThread = new Thread(render);

            screenThread.Start();
            Thread.Sleep(10);
            renderingThread.Start();
        }


        private static void render()
        {
            Stopwatch sw = new Stopwatch();
            while (true)
            {
                if (Screen.frameReady)
                {
                    continue;
                }

                sw.Reset();
                sw.Start();
                
                lock (renderLock)
                {/*
                    Parallel.ForEach(Meshes, (m) =>
                    {
                        m.renderMesh();
                    });*/
                    Screen.raycast();
                }

                

                sw.Stop();

                if (sw.ElapsedMilliseconds > 0)
                {
                    if (sw.ElapsedMilliseconds < 1000 / targetFps)
                    {
                        Thread.Sleep(Math.Abs(1000 / targetFps - (int)sw.ElapsedMilliseconds));
                    }
                    Screen.fps = 1000 / (int)sw.ElapsedMilliseconds;
                }

                
                Screen.frameReady = true;
            }
        }
    }
}
