using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DIRT
{
    static class Screen
    {
        private static int[,] frame;

        private static readonly object nextFrameLock = new object();
        private static double[,,] nextFrame;

        public static bool frameReady = false;

        public static int width = 300;
        public static int height = 200;

        public static void draw()
        {
            frame = new int[width, height];
            
            lock (nextFrameLock)
            {
                nextFrame = new double[width, height, 2];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        nextFrame[x, y, 1] = 1000;
                    }
                }
            }

            

            Stopwatch sw = new Stopwatch();

            while (true)
            {
                sw.Reset();
                sw.Start();



                if (frameReady)
                {
                    lock (nextFrameLock)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                frame[x, y] = (int)Math.Round(nextFrame[x, y, 0]);
                            }
                        }

                        nextFrame = new double[width, height, 2];

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                nextFrame[x, y, 1] = 1000;
                            }
                        }
                    }

                    frameReady = false;
                }

                char[] sc = new char[(width * 2 * height) + height];

                int g = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        char c = ' ';
                        switch (frame[x, y])
                        {
                            case 0:
                                c = ' ';
                                break;
                            case 1:
                                c = '░';
                                break;
                            case 2:
                                c = '▒';
                                break;
                            case 3:
                                c = '▓';
                                break;
                            case 4:
                                c = '█';
                                break;
                        }

                        sc[g] = c;
                        g++;
                        sc[g] = c;
                        g++;

                    }

                    sc[g] = '\n';
                    g++;
                }

                Console.SetCursorPosition(0, 0);
                Console.Write(sc);

                sw.Stop();

                if (sw.ElapsedMilliseconds > 0)
                {
                    Console.SetCursorPosition(0, height);
                    Console.WriteLine("               ");
                    Console.SetCursorPosition(0, height);
                    Console.WriteLine(1000 / sw.ElapsedMilliseconds);
                }
            }

        }

        public static void drawPoint(Vector p,double brightness)
        {
            if ((int)Math.Round(p.x) < -width/2 || (int)Math.Round(p.x) > width/2 - 1)
            {
                return;
            }

            if ((int)Math.Round(p.y) < -height/2 || (int)Math.Round(p.y) > height/2 - 1)
            {
                return;
            }

            lock (nextFrameLock)
            {
                if (nextFrame[(int)Math.Round(p.x) + (width / 2), (int)Math.Round(p.y) + (height / 2), 1] > p.z)
                {
                    nextFrame[(int)Math.Round(p.x) + (width / 2), (int)Math.Round(p.y) + (height / 2), 0] = brightness;
                    nextFrame[(int)Math.Round(p.x) + (width / 2), (int)Math.Round(p.y) + (height / 2), 1] = p.z;
                }
            }
        }

        public static void drawLine(Vector p1, Vector p2, double brightness)
        {
            Vector dir = p2 - p1;
            int l = (int)Math.Round(Vector.distance(p1, p2));
            dir = dir / l;

            for (int i = 0; i < l; i++)
            {
                drawPoint(p1 + (dir * i),brightness);
            }

        }

        public static void drawTriangle(Triangle t, double brightness)
        {
            Vector dir = t.points[1] - t.points[0];
            int l = (int)Math.Round(Vector.distance(t.points[0], t.points[1])) * 2;
            dir = dir / l;

            for (int i = 0; i < l; i++)
            {
                drawLine(t.points[0] + (dir * i),t.points[2], brightness);
            }
        }
    }
}