using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cloo;
using DIRT.Types;

namespace DIRT
{
    public static class Renderer
    {
        public static float[,] frame = new float[(int)Settings.screenWidth, (int)Settings.screenHeight];
        public static readonly object renderLock = new object();


        //gpu props & buffers
        #region gpu stuff
        private static ComputeContext context;
        private static ComputeKernel kernel;
        private static ComputeKernel kernelLight;
        private static ComputeEventList eventList;
        private static ComputeCommandQueue commands;
        private static ComputeBuffer<tris> tri;
        private static ComputeBuffer<int> tric;
        private static ComputeBuffer<vec> origins;
        private static ComputeBuffer<vec> dirs;
        private static ComputeBuffer<float> outs;
        private static ComputeBuffer<vec> lig;
        #endregion
        #region renderModes variables
        //raycast
        public static Vector eye = new Vector(0, 0, 0, 0);
        public static Vector dir = new Vector(0, 0, 1, 0);

        public static float lightX = -10f;
        public static int lastTriCount = -1;
        #endregion

        public static void render()
        {
            context = new ComputeContext(ComputeDeviceTypes.All, new ComputeContextPropertyList(ComputePlatform.Platforms[0]), null, IntPtr.Zero);

            string ker = File.ReadAllText("kernel.c");
            var program = new ComputeProgram(context, ker);
            program.Build(null, null, null, IntPtr.Zero);

            kernel = program.CreateKernel("ray");
            kernelLight = program.CreateKernel("rayLight");

            eventList = new ComputeEventList();
            commands = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            while (true)
            {
                frame = new float[(int)Settings.screenWidth, (int)Settings.screenHeight];

                lock (renderLock)
                {
                    switch (Settings.renderMode)
                    {
                        case Settings.renderModes.mathWay:
                            break;
                        case Settings.renderModes.raycast:
                            raycast();
                            break;
                        case Settings.renderModes.raycastShadow:
                            break;
                        case Settings.renderModes.aiRender:
                            break;
                    }

                    lock (Screen.nextFrameLock)
                    {
                        Screen.nextFrame = frame;
                        Screen.frameReady = true;
                    }
                }
                
            }
        }

        private static void raycast()
        {
            int width = frame.GetLength(0);
            int height = frame.GetLength(1);

            vec[] os = new vec[width * height];
            vec[] ds = new vec[width * height];

            vec eyeVec = eye.toVec();

            float f = 0.01f;

            Vector rot = new Vector(dir);

            
            Parallel.For(0, height, (y) => {
                Parallel.For(0, width, (x) => {
                    Vector pos = dir + new Vector((x - (width / 2)) * f, (y - (height / 2)) * f);
                    /*pos *= Matrix4x4.rotationXMatrix(Settings.cameraRot.x);
                    pos *= Matrix4x4.rotationYMatrix(Settings.cameraRot.y);
                    pos *= Matrix4x4.rotationZMatrix(Settings.cameraRot.z);*/
                    //pos += eye;

                    os[x + (y * width)] = eyeVec;
                    ds[x + (y * width)] = pos.toVec();
                });
            });



            List<tris> tris = new List<tris>();
            List<vec> lights = new List<vec>();

            foreach (Mesh m in DIRT.Meshes)
            {
                List<Triangle> triangles = m.getTris();
                foreach (Triangle t in triangles)
                {
                    tris.Add(t.toTris());
                }
            }

            if (tris == null || tris.Count == 0)
            {
                return;
            }

            foreach (Vector l in DIRT.Lights)
            {
                lights.Add(l.toVec());
            }


            lightX += 0.01f;

            int[] tsc = new int[2];

            tsc[0] = tris.Count;
            tsc[1] = lights.Count;

            float[] ous = new float[width * height];


            //sw.Restart();

           /* if (lastTriCount != tris.Count)
            {*/
                tri = new ComputeBuffer<tris>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, tris.ToArray());

                tric = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, tsc);
                origins = new ComputeBuffer<vec>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, os);
                dirs = new ComputeBuffer<vec>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, ds);

                outs = new ComputeBuffer<float>(context, ComputeMemoryFlags.WriteOnly, ous.Length);

                lig = new ComputeBuffer<vec>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, lights.ToArray());

                if (tris.Count > 0)
                    lastTriCount = tris.Count;
            /*}
            else
            {
                commands.WriteToBuffer(tris.ToArray(), tri, false, eventList);

                commands.WriteToBuffer(tsc, tric, false, eventList);
                commands.WriteToBuffer(os, origins, false, eventList);
                commands.WriteToBuffer(ds, dirs, false, eventList);

                //commands.WriteToBuffer(ous.Length, outs, true, eventList);

                commands.WriteToBuffer(lights.ToArray(), lig, false, eventList);
            }*/

            

            /*
            kernel.SetMemoryArgument(0, tri);
            kernelLight.SetMemoryArgument(1, lig);

            kernel.SetMemoryArgument(1, tric);
            kernel.SetMemoryArgument(2, origins);
            kernel.SetMemoryArgument(3, dirs);

            kernel.SetMemoryArgument(4, outs);*/

            kernel.SetMemoryArgument(0, tri);
            kernel.SetMemoryArgument(1, lig);

            kernel.SetMemoryArgument(2, tric);
            kernel.SetMemoryArgument(3, origins);
            kernel.SetMemoryArgument(4, dirs);

            kernel.SetMemoryArgument(5, outs);

            commands.Execute(kernel, null, new long[] { width * height }, null, eventList);

            /*
            kernelLight.SetMemoryArgument(0, tri);
            kernelLight.SetMemoryArgument(1, lig);

            kernelLight.SetMemoryArgument(2, tric);
            kernelLight.SetMemoryArgument(3, origins);
            kernelLight.SetMemoryArgument(4, dirs);

            kernelLight.SetMemoryArgument(5, outs);

            commands.Execute(kernelLight, null, new long[] { width * height }, null, eventList);*/


            commands.ReadFromBuffer(outs, ref ous, true, eventList);

            //sw.Stop();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //drawPoint(new Vector(x - (width / 2), y - (height / 2)), ous[x + (y * width)]);
                    frame[x, y] = ous[x + (y * width)];
                }
            }

            eventList.Clear();
            /*
            tri.Dispose();
            lig.Dispose();

            tric.Dispose();
            origins.Dispose();
            dirs.Dispose();
            outs.Dispose();*/
        }

        
    }
}
