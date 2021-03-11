using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using DIRT.Types;

namespace DIRT
{
    public static class ConsoleRenderer
    {
        private static List<Mesh> meshes;
        public static List<Mesh> Meshes
        {
            get
            {
                return meshes;
            }
            set
            {
                lock (Renderer.renderLock)
                {
                    meshes = value;
                }
                
            }
        }

        private static List<Vector> lights;
        public static List<Vector> Lights
        {
            get
            {
                return lights;
            }
            set
            {
                lock (Renderer.renderLock)
                {
                    lights = value;
                }

            }
        }

        private static Thread screenThread;
        private static Thread renderingThread;

        [DllImport("user32.dll")]
        static extern int ShowCursor(bool bShow);

        public static void startRenderer()
        {
            meshes = new List<Mesh>();
            lights = new List<Vector>();

            screenThread = new Thread(Screen.draw);
            renderingThread = new Thread(Renderer.render);

            screenThread.Start();
            renderingThread.Start();

            ShowCursor(false);

        }

        public static void stopRenderer()
        {
            screenThread.Abort();
            renderingThread.Abort();
        }

        public static object renderLock
        {
            get
            {
                return Renderer.renderLock;
            }
        }

        public static Bitmap textureMap
        {
            set
            {
                lock (Renderer.renderLock)
                {
                    Renderer.textureMap = value;
                }
            }
        }

        public static bool mouseLeft
        {
            get
            {
                return Screen.Engine.GetMouseLeft();
            }
        }

        public static bool keyDown(ConsoleKey key)
        {
            return Screen.Engine.GetKeyDown(key);
        }

        public static bool keyPressed(ConsoleKey key)
        {
            return Screen.Engine.GetKey(key);
        }

    }
}
