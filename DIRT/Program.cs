using System;
using System.Threading;

namespace DIRT
{
    class Program
    {

        static Thread screenThread;
        static void Main(string[] args)
        {
            screenThread = new Thread(Screen.draw);
            screenThread.Start();

            Thread.Sleep(100);

            Mesh m1 = new Mesh(new Vector(-75, 0, 200), new Vector(0,0,0));
            Mesh m2 = new Mesh(new Vector(75, 0, 200), new Vector(0, 0, 0));
            Mesh m3 = new Mesh(new Vector(0, 0, 100), new Vector(0, 0, 0));

            m1.makeCube(100);
            m2.makePyramid(100);
            m3.makeCube(25);

            bool side = false;

            while (true)
            {
                m1.rotation.x += 0.001;
                m1.rotation.y += 0.0005;
                m1.rotation.z += 0.002;

                m1.renderMesh();

                m2.rotation.x += 0.0005;
                m2.rotation.y += 0.0025;
                m2.rotation.z += 0.002;

                m2.renderMesh();

                if (side)
                {
                    m3.position.x += 0.5;
                }
                else
                {
                    m3.position.x -= 0.5;
                }

                if (m3.position.x > 100)
                {
                    side = false;
                }

                if (m3.position.x < -100)
                {
                    side = true;
                }

                m3.rotation.x += 0.006;
                m3.rotation.y += 0.004;
                m3.rotation.z += 0.002;

                m3.renderMesh();


                Screen.frameReady = true;
            }

            

        }
    }
}
