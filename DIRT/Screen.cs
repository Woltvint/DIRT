using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using DIRT.Types;

namespace DIRT
{
    internal static class Screen
    {
        private static int[,,] frame;
        private static int[,,] lastFrame;

        public static readonly object nextFrameLock = new object();
        public static float[,,] nextFrame;
        public static bool frameReady = false;

        public static int width = 99;
        public static int height = 99;

        public static int lastScreenMode = -1;

        private static Stopwatch sw = new Stopwatch();

        public static List<Triangle> tris = new List<Triangle>();

        public static string[] colorsText = new string[256];

        public static ConsoleGameEngine.ConsoleEngine Engine;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        public static void draw()
        {
            List<int> fpsCounter = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                fpsCounter.Add(0);
            }


            frame = new int[width, height,3];

            lock (nextFrameLock)
            {
                nextFrame = new float[width, height,3];
                lastFrame = frame;
            }


            var handle = GetStdHandle(-11);
            uint mode;
            GetConsoleMode(handle, out mode);
            mode |= 4;
            SetConsoleMode(handle, mode);

            Engine = new ConsoleGameEngine.ConsoleEngine((int)Settings.screenWidth+1, (int)Settings.screenHeight+1,8, 8);
            //Engine.Borderless();
            

            ConsoleGameEngine.Color[] colors = new ConsoleGameEngine.Color[16];

            for (int i = 0; i < 16; i++)
            {
                colors[i] = new ConsoleGameEngine.Color(i*16, i*16, i*16);
            }
            for (int i = 0; i < 256; i++)
            {
                colorsText[i] = $"\u001b[38;2;{i};{i};{i}m█";
            }

            Engine.SetPalette(colors);

            Stopwatch frameClock = new Stopwatch();

            while (true)
            {

                sw.Restart();
                //frameClock.Restart();

                lock (nextFrameLock)
                {
                    if (frameReady)
                    {
                        lastFrame = frame;
                        frame = new int[nextFrame.GetLength(0), nextFrame.GetLength(1),3];
                        width = nextFrame.GetLength(0);
                        height = nextFrame.GetLength(1);

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                if ((nextFrame[x, y, 0]) <= 0)
                                    frame[x, y, 0] = 0;
                                else if ((nextFrame[x, y, 0]) >= 255)
                                    frame[x, y, 0] = 255;
                                else
                                    frame[x, y, 0] = (int)(nextFrame[x, y, 0]);

                                if ((nextFrame[x, y, 1]) <= 0)
                                    frame[x, y, 1] = 0;
                                else if ((nextFrame[x, y, 1]) >= 255)
                                    frame[x, y, 1] = 255;
                                else
                                    frame[x, y, 1] = (int)(nextFrame[x, y, 1]);

                                if ((nextFrame[x, y, 2]) <= 0)
                                    frame[x, y, 2] = 0;
                                else if ((nextFrame[x, y, 2]) >= 255)
                                    frame[x, y, 2] = 255;
                                else
                                    frame[x, y, 2] = (int)(nextFrame[x, y, 2]);


                            }
                        }
                        frameReady = false;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (lastScreenMode != (int)Settings.screenMode)
                {
                    screenWriteAll();
                    lastScreenMode = (int)Settings.screenMode;
                    lastFrame = new int[width, height, 3];

                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            lastFrame[x, y, 0] = -1;
                            lastFrame[x, y, 1] = -1;
                            lastFrame[x, y, 2] = -1;
                        }
                    }

                    lastR = -1;
                    lastG = -1;
                    lastB = -1;
                }

                switch (Settings.screenMode)
                {
                    case Settings.screenModes.trueColor:
                        screenWriteChanges();
                        break;
                    case Settings.screenModes.graySpeed:

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                float br = (frame[x, y, 0] + frame[x, y, 1] + frame[x, y, 2]) / 3;

                                ConsoleGameEngine.ConsoleCharacter cc = ConsoleGameEngine.ConsoleCharacter.Full;

                                if (br < 64)
                                {
                                    cc = ConsoleGameEngine.ConsoleCharacter.Light;
                                }

                                if (br >= 64 && br < 128)
                                {
                                    cc = ConsoleGameEngine.ConsoleCharacter.Medium;
                                }

                                if (br >= 128 && br < 192)
                                {
                                    cc = ConsoleGameEngine.ConsoleCharacter.Dark;
                                }

                                if (br >= 192)
                                {
                                    cc = ConsoleGameEngine.ConsoleCharacter.Full;
                                }


                                Engine.SetPixel(new ConsoleGameEngine.Point(x-1, y-1), frame[x, y, 0] / 16, cc);

                            }
                        }

                        Engine.DisplayBuffer();
                        break;
                }
                
                /*
                frameClock.Stop();

                if (frameClock.ElapsedMilliseconds < 14)
                {
                    Thread.Sleep((int)(15 - frameClock.ElapsedMilliseconds));
                }*/

                sw.Stop();
                
                if (sw.ElapsedMilliseconds > 0)
                {
                    int fps = 0;

                    for (int i = 0; i < fpsCounter.Count; i++)
                    {
                        fps += fpsCounter[i];
                    }

                    fps /= fpsCounter.Count;

                    Console.Title = "screen: " + fps + " fps | render: " + Renderer.fps + " fps | triangles: " + Renderer.lastTriCount;
                    

                    fpsCounter.Insert(0, (int)(1000 / sw.ElapsedMilliseconds));
                    fpsCounter.RemoveAt(fpsCounter.Count-1);
                }

            }

        }

        private static void screenWriteAll()
        {
            List<char> scChars = new List<char>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    scChars.AddRange(getVirtualTerminalColor(frame[x, y,0], frame[x, y, 1],frame[x, y, 2]));
                }

                scChars.Add('\n');
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(scChars.ToArray());

        }

        private static void screenWriteChanges()
        {
            if (frame.GetLength(0) != lastFrame.GetLength(0) || frame.GetLength(1) != lastFrame.GetLength(1))
            {
                screenWriteAll();
                return;
            }

            List<char> scChars = new List<char>();

            scChars.AddRange($"\u001b[0;0H");

            int lastY = 0;

            for (int y = 0; y < height; y++)
            {
                int lastX = 0;
                for (int x = 0; x < width; x++)
                {
                    if (colorChanged(x,y))
                    {
                        if (lastX != x-1 || lastY != y)
                        {
                            scChars.AddRange($"\u001b[{y};{x}H");
                            lastX = x;
                            lastY = y;
                        }
                        
                        scChars.AddRange(getVirtualTerminalColor(frame[x, y,0], frame[x, y, 1], frame[x, y, 2]));
                    }
                }
            }

            

            //Console.Write(scChars.ToArray());

            Console.Out.WriteAsync(scChars.ToArray());
        }

        private static bool colorChanged(int x,int y)
        {
            if (frame[x, y, 0] != lastFrame[x, y, 0])
            {
                return true;
            }
            if (frame[x, y, 1] != lastFrame[x, y, 1])
            {
                return true;
            }
            if (frame[x, y, 2] != lastFrame[x, y, 2])
            {
                return true;
            }

            return false;
        }

        private static int lastR = 0;
        private static int lastG = 0;
        private static int lastB = 0;

        private static char[] getVirtualTerminalColor(int R,int G,int B)
        {
            if (lastR == R && lastG == G && lastB == B)
            {
                return "█".ToCharArray();
            }
            else
            {
                lastR = R;
                lastG = G;
                lastB = B;
                return $"\u001b[38;2;{R};{G};{B}m█".ToCharArray();
            }

        }

        private static float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }
    }
}