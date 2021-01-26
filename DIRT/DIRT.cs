using System.Collections.Generic;
using System.Threading;
using DIRT.Types;

namespace DIRT
{
    public class DIRT
    {
        public static List<Mesh> Meshes;
        public static List<Vector> Lights;

        private static Thread screenThread;
        private static Thread renderingThread;

        public static void startRenderer()
        {
            Meshes = new List<Mesh>();
            Lights = new List<Vector>();

            screenThread = new Thread(Screen.draw);
            renderingThread = new Thread(Renderer.render);

            screenThread.Start();
            Thread.Sleep(1000);
            renderingThread.Start();

        }
    }
}
