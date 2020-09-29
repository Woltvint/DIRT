﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DIRT
{
    static class Screen
    {
        private static int[,] frame;

        private static readonly object nextFrameLock = new object();
        private static double[,,] nextFrame;

        public static bool frameReady = false;

        public static int width = 100;
        public static int height = 100;

        private static readonly object inputLock = new object();
        private static char input = ' ';

        public static int fps = 1;

        public static List<Triangle> tris = new List<Triangle>();

        public static string[] charPool = new string[] { "  ", "░ ", "░░", "░▒", "▒▒", "▓▒", "▓▓", "▓█", "██" };
        //public static string[] charPool = new string[] { "  ", "░░",  "▒▒",  "▓▓",  "██" };

        private static bool isRunningBlink = false;

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

            Console.CursorVisible = false;

            Stopwatch sw = new Stopwatch();

            while (true)
            {/*
                sw.Reset();
                sw.Start();*/



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
                    /*
                    isRunningBlink = !isRunningBlink;

                    Console.SetCursorPosition(0, height + 3);

                    if (isRunningBlink)
                    {

                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                    */
                    frameReady = false;
                }

                char[] sc = new char[(width * 2 * height) + height];

                int g = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {/*
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
                            default:
                                c = ' ';
                                break;
                        }*/
                        
                        sc[g] = charPool[frame[x, y]][y % 2];
                        g++;
                        sc[g] = charPool[frame[x, y]][(y + 1) % 2];
                        g++;
                        /*
                        sc[g] = frame[x, y].ToString()[0];
                        g++;
                        sc[g] = frame[x, y].ToString()[0];
                        g++;*/

                    }

                    sc[g] = '\n';
                    g++;
                }

                Console.SetCursorPosition(0, 0);
                Console.Write(sc);
                Console.CursorVisible = false;

                

                /*
                sw.Stop();
                
                if (sw.ElapsedMilliseconds > 0)
                {
                    Console.SetCursorPosition(0, height);
                    Console.WriteLine("               ");
                    Console.SetCursorPosition(0, height);
                    Console.WriteLine(1000 / sw.ElapsedMilliseconds);
                }*/

                if (fps > 0)
                {
                    Console.SetCursorPosition(0, height);
                    Console.WriteLine("               ");
                    Console.SetCursorPosition(0, height);
                    Console.WriteLine(fps);
                }

                lock (inputLock)
                {
                    if (Console.KeyAvailable)
                    {
                        input = Console.ReadKey().KeyChar;
                    }
                    else
                    {
                        input = '|';
                    }
                }
            }

        }


        public static void drawPoint(Vector p, double brightness)
        {
            int px = (int)Math.Round(p.x);
            int py = (int)Math.Round(p.y);

            if (px < -width / 2 || px > width / 2 - 1)
            {
                return;
            }

            if (py < -height / 2 || py > height / 2 - 1)
            {
                return;
            }

            if (brightness > charPool.Length - 1)
            {
                brightness = charPool.Length - 1;
            }
            if (brightness < 0)
            {
                brightness = 0;
            }

            if (nextFrame[px + (width / 2), py + (height / 2), 1] > p.z)
            {
                nextFrame[px + (width / 2), py + (height / 2), 0] = brightness;
                nextFrame[px + (width / 2), py + (height / 2), 1] = p.z;
            }
            /*else if (nextFrame[px + (width / 2), py + (height / 2), 1] == p.z)
            {
                if (nextFrame[px + (width / 2), py + (height / 2), 1] > brightness)
                {
                    nextFrame[px + (width / 2), py + (height / 2), 0] = brightness;
                    nextFrame[px + (width / 2), py + (height / 2), 1] = p.z;
                }
            }*/
        }


    

        public static void drawLine(Vector p1, Vector p2, double brightness)
        {
            Vector dir = p2 - p1;
            double l = Math.Round(Vector.distance(p1, p2));
            dir = dir / l;

            for (int i = 0; i < l; i++)
            {
                drawPoint(p1 + (dir * i), brightness);
            }

        }
        
        public static void drawTriangle(Triangle t, double brightness)
        {
            if ((!testPoint(t.points[0])) && (!testPoint(t.points[1])) && (!testPoint(t.points[2])))
            {
                return;
            }

            if (Vector.angleDist(Settings.camera, t.normal) < 0)
            {
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                Vector dir = t.points[(i+1)%3] - t.points[i];
                double l = (Math.Round(Vector.distance(t.points[i], t.points[(i+1)%3])));
                dir = dir / l;

                for (int j = 0; j < l; j++)
                {
                    drawLine(t.points[i] + (dir * j), t.points[(i+2)%3], brightness);
                }
            }
        }

        public static bool testPoint(Vector point)
        {
            int px = (int)Math.Round(point.x);
            int py = (int)Math.Round(point.y);

            if (px < -width / 2 || px > width / 2 - 1)
            {
                return false;
            }

            if (py < -height / 2 || py > height / 2 - 1)
            {
                return false ;
            }
            
            if (point.z > 0)
            {
                return false;
            }

            return true;
        }


        public static char getInput()
        {
            lock (inputLock)
            {
                return input;
            }
        }


        public static Vector eye = new Vector(0, 0, 0, 0);
        public static Vector dir = new Vector(0,0,1,0);

        public static void raycast()
        {
            Vector direction = dir * 1;

            Ray[] rays = new Ray[width * height];
            
            double f = 0.01;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector pos = direction + new Vector((x-(width/2)) * f, (y - (height / 2)) * f, 0,0);
                    pos *= Matrix4x4.rotationYMatrix(Settings.cameraRot.y);
                    pos += eye;

                    rays[x + (y * width)] = new Ray(eye, pos);

                }
            }

            List<Triangle> prepTris = new List<Triangle>();
            List<Triangle> tris = new List<Triangle>();

            foreach (Mesh m in DIRT.Meshes)
            {
                prepTris.AddRange(m.getTris());
            }

            foreach (Triangle t in prepTris)
            {
                if (Vector.angleDist(direction * Matrix4x4.rotationYMatrix(Settings.cameraRot.y), t.middle) > 0.3)
                {
                    tris.Add(t);
                }
            }



            tris.Sort(CompareTriangleDistance);
            
            Parallel.For(0, rays.Length, (i) =>
            {
                 rays[i].cast(tris);
            });
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    drawPoint(new Vector(x - (width / 2), y - (height / 2), 0,0), rays[x + (y * width)].result);
                }
            }
        }

        static int CompareTriangleDistance(Triangle t1, Triangle t2)
        {
            double dist1 = Vector.distance(t1.middle, eye);
            double dist2 = Vector.distance(t2.middle, eye);

            if (dist1 == dist2)
            {
                return 0;
            }
            else
            {
                if (dist1 < dist2)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}