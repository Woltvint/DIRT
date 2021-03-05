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

        private static readonly object inputLock = new object();
        private static ConsoleKey input;

        private static Stopwatch sw = new Stopwatch();

        public static List<Triangle> tris = new List<Triangle>();

        //public static string[] charPool = new string[] {"  ", "░ ", "{}", "≡≡", "∑∑",  "‡‡", "@@", "░░", "88", "¶¶",  "ææ", "░▒", "◄►", "▒▒" , "■■", "▄▀",  "▓▒", "▓▓", "▓█", "██" };

        //public static string[] charPool = new string[] { "  ", "░ ", "░}", "{}", "{≡", "≡≡", "≡∑", "∑∑", "∑‡", "‡‡", "‡@", "@@", "@░", "░░", "░8", "88", "8¶", "¶¶", "¶æ", "ææ", "æ░", "░▒", "▒►", "◄►", "◄▒", "▒▒", "▒■", "■■", "■▀", "▄▀", "▀▓", "▓▒", "▓▓", "▓█", "██" };

        //public static string[] charPool = new string[] { "  ", "= ", "} ", " {", "≡ ", "] ", " ‡", "[ ", " ∑", " $", "æ ", " #", "}=", "{=", "◄ ", " ►", " 8", "≡=", "=]", "¶ ", " @", "‡=", "}}", "{}", "≡}", "}]", "{≡", "[=", "≡≡", "]{", "]≡", "‡}", "░ ", "{‡", "≡‡", "]‡", "∑=", "$=", "‡‡", "[}", "=æ", "=#", "[{", "[≡", "][", "=◄", "=►", "=8", "¶=", "[‡", "=@", "∑}", "$}", "∑{", "{$", "∑≡", "$≡", "∑]", "$]", "æ}", "}#", "æ{", "{#", "æ≡", "‡∑", "≡#", "‡$", "◄}", "]æ", "►}", "#]", "}8", "{◄", "}¶", "{►", "{8", "@}", "≡◄", "≡►", "≡8", "{¶", "æ‡", "]◄", "#‡", "]►", "@{", "]8", "≡¶", "@≡", "■ ", "]¶", "░=", "@]", "◄‡", "►‡", "[∑", "8‡", "[$", "‡¶", "▒ ", " ▒", "@‡", "[æ", "#[", "◄[", "∑∑", "[►", "∑$", "[8", "▄ ", "░}", "[¶", "@[", "░{", "░≡", "æ∑", "æ$", "#∑", "#$", "░]", "◄∑", "◄$", "►∑", "►$", "ææ", "░‡", "‡░", "$8", "#æ", "▀ ", "∑¶", "$¶", "@∑", "@$", "◄æ", "æ►", "◄#", "æ8", "#►", "8#", "æ¶", "#¶", "æ@", "#@", "◄◄", "►◄", "8◄", "►8", "░[", "88", "¶◄", "¶►", "¶8", "◄@", "►@", "8@", "=■", "@¶", "@@", "=▒", "▒=", "∑░", "$░", "æ░", "░#", "▄=", "}■", "{■", "◄░", "≡■", "►░", "8░", "]■", "}▒", "▒}", "░¶", "░@", "{▒", "▒{", "≡▒", "▒≡", "‡■", "]▒", "▒]", "=▀", "▄}", "‡▒", "‡▒", "{▄", "≡▄", "▄]", "[■", "‡▄", "[▒", "▒[", "▀}", "{▀", "▀≡", "■∑", "]▀", "■$", "[▄", "‡▀", "▒∑", "∑▒", "$▒", "▒$", "▒$", "#■", "■◄", "æ▒", "æ▒", "►■", "■8", "#▒", "▒#", "■¶", "@■", "▄∑", "▄$", "◄▒", "▀[", "▒◄", "►▒", "▒►", "8▒", "▒8", "¶▒", "▒¶", "@▒", "▒@", "æ▄", "#▄", "◄▄", "►▄", "8▄", "▀∑", "$▀", "¶▄", "@▄", "▀æ", "#▀", "■░", "◄▀", "►▀", "▀8", "▒░", "░▒", "¶▀", "▀@", "▄░", " ▓", "▀░", "■■", "■▒", "▒■", "▒▒", "▒▒", "▄■", "▄▒", "▒▄", "▓=", "▀■", "▀▒", "▒▀", "}▓", "{▓", "≡▓", "▄▀", "]▓", "█ ", "‡▓", "▀▀", "▓[", "∑▓", "$▓", "▓æ", "▓#", "▓◄", "▓►", "▓8", "▓¶", "@▓", "█=", "}█", "█{", "█≡", "▓░", "]█", "‡█", "[█", "█∑", "$█", "█æ", "█#", "◄█", "█►", "█8", "■▓", "█¶", "@█", "▒▓", "▓▒", "▄▓", "█░", "▓▀", "█■", "▒█", "█▒", "▄█", "█▀", "▓█", "██" };

        //public static string[] charPool = new string[] { "  ", "░ ", "░░", "░▒", "▒▒", "▓▒", "▓▓", "▓█", "██" };

        public static string[] charPool = new string[] { " ", "░", "▒", "▓", "█" };

        public static string[] colorsText = new string[256];

        public static ConsoleGameEngine.ConsoleEngine Engine;

        [DllImport("user32.dll")]
        static extern int ShowCursor(bool bShow);

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



            ShowCursor(false);



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
                                if ((nextFrame[x, y,0]) <= 0)
                                    frame[x, y,0] = 0;
                                else if ((nextFrame[x, y,0]) >= 255)
                                    frame[x, y,0] = 255;
                                else
                                    frame[x, y,0] = (int)(nextFrame[x, y,0]);

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


                
                switch (Settings.frameUpdateMode)
                {
                    case Settings.frameUpdateModes.writeAll:
                        screenWriteAll();
                        break;
                    case Settings.frameUpdateModes.writeChanges:
                        screenWriteChanges();
                        break;
                    case Settings.frameUpdateModes.screenBuffer:
                        //todo screenbufffunc
                        break;
                }

                /*
                
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        ConsoleGameEngine.ConsoleCharacter cc = ConsoleGameEngine.ConsoleCharacter.Full;
                        
                        if (frame[x, y,0]/4 >= 0 && frame[x, y,0]/4 < 16)
                        {
                            cc = ConsoleGameEngine.ConsoleCharacter.Light;
                        }

                        if (frame[x, y,0]/4 >= 16 && frame[x, y,0]/4 < 32)
                        {
                            cc = ConsoleGameEngine.ConsoleCharacter.Medium;
                        }

                        if (frame[x, y,0]/4 >= 32 && frame[x, y,0]/4 < 48)
                        {
                            cc = ConsoleGameEngine.ConsoleCharacter.Dark;
                        }

                        if (frame[x, y,0]/4 >= 48 && frame[x, y,0]/4 < 64)
                        {
                            cc = ConsoleGameEngine.ConsoleCharacter.Full;
                        }


                        Engine.SetPixel(new ConsoleGameEngine.Point(x, y),frame[x,y,0]/16, cc);

                        

                    }
                }*/

                

                //Engine.DisplayBuffer();
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

                    Console.Title = "screen: " + fps + " fps | render: " + Renderer.fps + "fps | triangles: " + Renderer.lastTriCount;
                    

                    fpsCounter.Insert(0, (int)(1000 / sw.ElapsedMilliseconds));
                    fpsCounter.RemoveAt(fpsCounter.Count-1);
                }
                
                //lastColor = 0;

                
                /*

                lock (inputLock)
                {
                    if (Console.KeyAvailable)
                    {
                        input = Console.ReadKey().Key;
                    }
                    else
                    {
                        input = ConsoleKey.NoName;
                    }
                }*/
            }

        }



        private static void screenWriteAll()
        {
            List<char> scChars = new List<char>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    scChars.AddRange(getColor(frame[x, y,0], frame[x, y, 1],frame[x, y, 2]));
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
                        
                        scChars.AddRange(getColor(frame[x, y,0], frame[x, y, 1], frame[x, y, 2]));
                    }
                }
            }

            

            //Console.Write(scChars.ToArray());

            Console.Out.WriteAsync(scChars.ToArray());
        }

        private static bool bufferInicDone = false;
        private static void moveScreenBuffer()
        {
            if (!bufferInicDone)
            {

            }
        }


        private static char[] getColor(int R, int G, int B)
        {
            switch (Settings.colorMode)
            {
                case Settings.colorModes.charPoolBrightness:
                    //return getCharPoolBrightness(pixel);
                case Settings.colorModes.consoleColor:
                    break;
                case Settings.colorModes.virtualTerminalColor:
                    return getVirtualTerminalColor(R,G,B);
            }

            return " ".ToCharArray();
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

        

        private static char[] getCharPoolBrightness(int pixel)
        {
            return charPool[(int)Remap(pixel, 0, 255, 0, charPool.Length-1)].ToCharArray();
        }



        private static int lastR = 0;
        private static int lastG = 0;
        private static int lastB = 0;

        private static char[] getVirtualTerminalColor(int R,int G,int B)
        {
            //return $"\u001b[38;2;{pixel};{pixel};{pixel}m█".ToCharArray();
            if (lastR == R && lastG == G && lastB == B)
            {
                return "█".ToCharArray();
            }
            else
            {
                lastR = R;
                lastG = G;
                lastB = B;
                //return colorsText[pixel].ToCharArray();
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















        public static void drawPoint(Vector p, float brightness)
        {
            int px = (int)MathF.Round(p.x);
            int py = (int)MathF.Round(p.y);

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
            /*
            if (nextFrame[px + (width / 2), py + (height / 2), 1] > p.z)
            {
                nextFrame[px + (width / 2), py + (height / 2), 0] = brightness;
                nextFrame[px + (width / 2), py + (height / 2), 1] = p.z;
            }*/
            
        }

        public static void drawLine(Vector p1, Vector p2, float brightness)
        {
            Vector dir = p2 - p1;
            float l = MathF.Round(Vector.distance(p1, p2));
            dir = dir / l;

            for (int i = 0; i < l; i++)
            {
                drawPoint(p1 + (dir * i), brightness);
            }

        }
        
        public static void drawTriangle(Triangle t, float brightness)
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
                float l = (MathF.Round(Vector.distance(t.points[i], t.points[(i+1)%3])));
                dir = dir / l;

                for (int j = 0; j < l; j++)
                {
                    drawLine(t.points[i] + (dir * j), t.points[(i+2)%3], brightness);
                }
            }
        }

        public static bool testPoint(Vector point)
        {
            int px = (int)MathF.Round(point.x);
            int py = (int)MathF.Round(point.y);

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


        public static ConsoleKey getInput()
        {
            lock (inputLock)
            {
                return input;
            }
        }


        /*static int CompareTriangleDistance(Triangle t1, Triangle t2)
        {
            float dist1 = Vector.distance(t1.middle, eye);
            float dist2 = Vector.distance(t2.middle, eye);

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
        }*/
    }
}