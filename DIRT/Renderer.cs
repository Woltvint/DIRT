using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using Cloo;
using DIRT.Types;
using System.Reflection;

namespace DIRT
{
    internal static class Renderer
    {
        public static float[,,] frame = new float[(int)Settings.screenWidth, (int)Settings.screenHeight, 3];
        public static readonly object renderLock = new object();

        public static int fps = 0;

        //gpu props & buffers
        #region gpu stuff
        private static ComputeContext context;

        private static ComputeKernel prepTrisKernel;
        private static ComputeKernel renderKernel;

        private static ComputeEventList eventList;
        private static ComputeCommandQueue commands;

        private static ComputeBuffer<tris> gTris;
        private static ComputeBuffer<int> gOptions;
        private static ComputeBuffer<vec> gCam;
        private static ComputeBuffer<vec> gPos;
        private static ComputeBuffer<vec> gRot;
        private static ComputeBuffer<float> gTexture;

        private static ComputeBuffer<vec> origins;
        private static ComputeBuffer<vec> dirs;
        private static ComputeBuffer<float> outs;
        private static ComputeBuffer<vec> lig;
        #endregion
        #region variables
        public static Bitmap textureMap
        {
            set
            {
                for (int x = 0; x < 1000; x++)
                {
                    for (int y = 0; y < 1000; y++)
                    {
                        texture[(x + (y * 1000)) * 3] = value.GetPixel(x, y).R;
                        texture[((x + (y * 1000)) * 3) + 1] = value.GetPixel(x, y).G;
                        texture[((x + (y * 1000)) * 3) + 2] = value.GetPixel(x, y).B;
                    }
                }
            }
        }

        //raycast
        public static Vector eye = new Vector(0, 0, 0, 0);
        public static Vector dir = new Vector(0, 0, 1, 0);
        private static float[] texture = new float[1000 * 1000 * 3];

        public static float lightX = -10f;
        public static int lastTriCount = -7;
        #endregion

        private static Stopwatch sw;

        public static void render()
        {
            context = new ComputeContext(ComputeDeviceTypes.All, new ComputeContextPropertyList(ComputePlatform.Platforms[0]), null, IntPtr.Zero);

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DIRT.kernel.c";

            string ker = "";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                ker = reader.ReadToEnd();
            }

            var program = new ComputeProgram(context, ker);
            program.Build(null, null, null, IntPtr.Zero);

            prepTrisKernel = program.CreateKernel("prepTris");
            renderKernel = program.CreateKernel("ray");

            eventList = new ComputeEventList();
            commands = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            sw = new Stopwatch();

            while (true)
            {

                lock (renderLock)
                {
                    eye = Settings.camera;

                    if (Screen.frameReady)
                    {
                        continue;
                    }
                    else
                    {
                        frame = new float[(int)Settings.screenWidth, (int)Settings.screenHeight,3];
                    }

                    sw.Restart();

                    switch (Settings.renderMode)
                    {
                        case Settings.renderModes.mathWay:
                            break;
                        case Settings.renderModes.raycast:
                            raycast();
                            break;
                        case Settings.renderModes.raycastShadow:
                            break;
                    }

                    sw.Stop();

                    if (sw.ElapsedMilliseconds > 0)
                    {
                        fps = (int)(1000 / sw.ElapsedMilliseconds);
                    }

                    lock (Screen.nextFrameLock)
                    {

                        Screen.nextFrame = frame;
                        Screen.frameReady = true;
                    }

                    if (gTris != null)
                    {
                        lastTriCount = (int)gTris.Count;
                    }
                }

                foreach (ComputeEvent e in eventList)
                {
                    e.Dispose();
                }
                eventList.Clear();
            }
        }

        private static bool prepTris()
        {
            int width = frame.GetLength(0);
            int height = frame.GetLength(1);

            List<tris> tris = new List<tris>();
            List<vec> poss = new List<vec>();
            List<vec> rots = new List<vec>();


            int[] options = new int[4];

            vec[] cam = new vec[3];

            cam[0] = eye.toVec();
            Vector look = new Vector(0, 0, 1, 0);
            Vector up = new Vector(0, 1, 0, 0);
            look *= Matrix4x4.rotationXMatrix(Settings.cameraRot.x);
            look *= Matrix4x4.rotationYMatrix(Settings.cameraRot.y);
            cam[1] = look.toVec();
            cam[2] = up.toVec();

            foreach (Mesh m in ConsoleRenderer.Meshes)
            {
                foreach (Triangle t in m.tris)
                {
                    tris.Add(t.toTris());

                    poss.Add(m.position.toVec());
                    rots.Add(m.rotation.toVec());
                }
            }

            options[0] = tris.Count;
            options[1] = width;
            options[2] = height;

            if (tris.Count <= 0)
            {
                return false;
            }

            if (lastTriCount != tris.Count)
            {
                gTris = new ComputeBuffer<tris>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, tris.ToArray());
                gOptions = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, options);
                gCam = new ComputeBuffer<vec>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, cam);
                gPos = new ComputeBuffer<vec>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, poss.ToArray());
                gRot = new ComputeBuffer<vec>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, rots.ToArray());
                gTexture = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, texture);
            }
            else
            {
                commands.WriteToBuffer(tris.ToArray(), gTris, false, eventList);
                commands.WriteToBuffer(options, gOptions, false, eventList);
                commands.WriteToBuffer(cam, gCam, false, eventList);
                commands.WriteToBuffer(rots.ToArray(), gRot, false, eventList);
                commands.WriteToBuffer(poss.ToArray(), gPos, false, eventList);
                commands.WriteToBuffer(texture, gTexture, false, eventList);
            }
                
            

            prepTrisKernel.SetMemoryArgument(0, gTris);
            prepTrisKernel.SetMemoryArgument(1, gOptions);
            prepTrisKernel.SetMemoryArgument(2, gRot);
            prepTrisKernel.SetMemoryArgument(3, gPos);
            prepTrisKernel.SetMemoryArgument(4, gCam);
            
            commands.Execute(prepTrisKernel, null, new long[] { tris.Count }, null, eventList);


            return true;
        }

        private static void raycast()
        {

            if (!prepTris())
            {
                return;
            }


            int width = frame.GetLength(0);
            int height = frame.GetLength(1);

            vec[] os = new vec[width * height];
            vec[] ds = new vec[width * height+1];

            ds[0] = Settings.cameraRot.toVec();

            vec eyeVec = eye.toVec();

            float f = 0.0155f / 1.75f;

            
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector pos = dir + new Vector((x - (width / 2)) * f, (y - (height / 2)) * f);

                    //pos *= rotMat;

                    os[x + (y * width)] = eyeVec;
                    ds[x + (y * width) + 1] = pos.toVec();
                }
            }
            
            List<vec> lights = new List<vec>();
            
            if (gTris == null || gTris.Count == 0)
            {
                return;
            }

            foreach (Vector l in ConsoleRenderer.Lights)
            {
                lights.Add(l.toVec());
            }


            lightX += 0.01f;

            float[] ous = new float[width * height*3];


            

            if (lastTriCount != gTris.Count)
            {
                origins = new ComputeBuffer<vec>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, os);
                dirs = new ComputeBuffer<vec>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, ds);

                outs = new ComputeBuffer<float>(context, ComputeMemoryFlags.WriteOnly , ous.Length);

                lig = new ComputeBuffer<vec>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, lights.ToArray());
            
            }
            else
            {
                commands.WriteToBuffer(os, origins, false, eventList);
                commands.WriteToBuffer(ds, dirs, false, eventList);

                commands.WriteToBuffer(lights.ToArray(), lig, false, eventList);
            }
            

            renderKernel.SetMemoryArgument(0, gTris);
            renderKernel.SetMemoryArgument(1, lig);

            renderKernel.SetMemoryArgument(2, gOptions);
            renderKernel.SetMemoryArgument(3, origins);
            renderKernel.SetMemoryArgument(4, dirs);

            renderKernel.SetMemoryArgument(5, gTexture);

            renderKernel.SetMemoryArgument(6, outs);

            commands.Execute(renderKernel, null, new long[] { width * height }, null, eventList);

            commands.ReadFromBuffer(outs, ref ous, true, eventList);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    frame[x, y, 0] = ous[(x + (y * width)) * 3];
                    frame[x, y, 1] = ous[((x + (y * width)) * 3) + 1];
                    frame[x, y, 2] = ous[((x + (y * width)) * 3) + 2];
                }
            }
            //eventList.Clear();


        }

        
    }
}
