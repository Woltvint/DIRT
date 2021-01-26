using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DIRT.Types;

namespace DIRT
{
    static class Screen
    {
        private static int[,] frame;
        private static int[,] lastFrame;

        public static readonly object nextFrameLock = new object();
        public static float[,] nextFrame;
        public static bool frameReady = false;

        public static int width = 100;
        public static int height = 100;

        private static readonly object inputLock = new object();
        private static char input = ' ';

        private static Stopwatch sw = new Stopwatch();

        public static List<Triangle> tris = new List<Triangle>();

        //public static string[] charPool = new string[] {"  ", "░ ", "{}", "≡≡", "∑∑",  "‡‡", "@@", "░░", "88", "¶¶",  "ææ", "░▒", "◄►", "▒▒" , "■■", "▄▀",  "▓▒", "▓▓", "▓█", "██" };

        //public static string[] charPool = new string[] { "  ", "░ ", "░}", "{}", "{≡", "≡≡", "≡∑", "∑∑", "∑‡", "‡‡", "‡@", "@@", "@░", "░░", "░8", "88", "8¶", "¶¶", "¶æ", "ææ", "æ░", "░▒", "▒►", "◄►", "◄▒", "▒▒", "▒■", "■■", "■▀", "▄▀", "▀▓", "▓▒", "▓▓", "▓█", "██" };

        //public static string[] charPool = new string[] { "  ", "= ", "} ", " {", "≡ ", "] ", " ‡", "[ ", " ∑", " $", "æ ", " #", "}=", "{=", "◄ ", " ►", " 8", "≡=", "=]", "¶ ", " @", "‡=", "}}", "{}", "≡}", "}]", "{≡", "[=", "≡≡", "]{", "]≡", "‡}", "░ ", "{‡", "≡‡", "]‡", "∑=", "$=", "‡‡", "[}", "=æ", "=#", "[{", "[≡", "][", "=◄", "=►", "=8", "¶=", "[‡", "=@", "∑}", "$}", "∑{", "{$", "∑≡", "$≡", "∑]", "$]", "æ}", "}#", "æ{", "{#", "æ≡", "‡∑", "≡#", "‡$", "◄}", "]æ", "►}", "#]", "}8", "{◄", "}¶", "{►", "{8", "@}", "≡◄", "≡►", "≡8", "{¶", "æ‡", "]◄", "#‡", "]►", "@{", "]8", "≡¶", "@≡", "■ ", "]¶", "░=", "@]", "◄‡", "►‡", "[∑", "8‡", "[$", "‡¶", "▒ ", " ▒", "@‡", "[æ", "#[", "◄[", "∑∑", "[►", "∑$", "[8", "▄ ", "░}", "[¶", "@[", "░{", "░≡", "æ∑", "æ$", "#∑", "#$", "░]", "◄∑", "◄$", "►∑", "►$", "ææ", "░‡", "‡░", "$8", "#æ", "▀ ", "∑¶", "$¶", "@∑", "@$", "◄æ", "æ►", "◄#", "æ8", "#►", "8#", "æ¶", "#¶", "æ@", "#@", "◄◄", "►◄", "8◄", "►8", "░[", "88", "¶◄", "¶►", "¶8", "◄@", "►@", "8@", "=■", "@¶", "@@", "=▒", "▒=", "∑░", "$░", "æ░", "░#", "▄=", "}■", "{■", "◄░", "≡■", "►░", "8░", "]■", "}▒", "▒}", "░¶", "░@", "{▒", "▒{", "≡▒", "▒≡", "‡■", "]▒", "▒]", "=▀", "▄}", "‡▒", "‡▒", "{▄", "≡▄", "▄]", "[■", "‡▄", "[▒", "▒[", "▀}", "{▀", "▀≡", "■∑", "]▀", "■$", "[▄", "‡▀", "▒∑", "∑▒", "$▒", "▒$", "▒$", "#■", "■◄", "æ▒", "æ▒", "►■", "■8", "#▒", "▒#", "■¶", "@■", "▄∑", "▄$", "◄▒", "▀[", "▒◄", "►▒", "▒►", "8▒", "▒8", "¶▒", "▒¶", "@▒", "▒@", "æ▄", "#▄", "◄▄", "►▄", "8▄", "▀∑", "$▀", "¶▄", "@▄", "▀æ", "#▀", "■░", "◄▀", "►▀", "▀8", "▒░", "░▒", "¶▀", "▀@", "▄░", " ▓", "▀░", "■■", "■▒", "▒■", "▒▒", "▒▒", "▄■", "▄▒", "▒▄", "▓=", "▀■", "▀▒", "▒▀", "}▓", "{▓", "≡▓", "▄▀", "]▓", "█ ", "‡▓", "▀▀", "▓[", "∑▓", "$▓", "▓æ", "▓#", "▓◄", "▓►", "▓8", "▓¶", "@▓", "█=", "}█", "█{", "█≡", "▓░", "]█", "‡█", "[█", "█∑", "$█", "█æ", "█#", "◄█", "█►", "█8", "■▓", "█¶", "@█", "▒▓", "▓▒", "▄▓", "█░", "▓▀", "█■", "▒█", "█▒", "▄█", "█▀", "▓█", "██" };

        //public static string[] charPool = new string[] { "  ", "░ ", "░░", "░▒", "▒▒", "▓▒", "▓▓", "▓█", "██" };

        public static string[] charPool = new string[] { " ", "░", "▒", "▓", "█" };

        private static string[] colors = new string[256];

        public static void draw()
        {
            int saveNum = 0;

            List<int> fpsCounter = new List<int>();

            for (int i = 0; i < 1; i++)
            {
                fpsCounter.Add(30);
            }


            frame = new int[width, height];

            lock (nextFrameLock)
            {
                nextFrame = new float[width, height];
                lastFrame = frame;
                /*
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        nextFrame[x, y, 1] = 1000;
                    }
                }*/
            }

            

            

            

            for (int i = 0; i < 256; i++)
            {
                colors[i] = $"\u001b[38;2;{i};{i};{i}m█";

                //colors[i] = charPool[(int)MathF.Floor(i / ((int)MathF.Ceiling(256/charPool.Length)+1))];


                //colors[i] = $"\u001b[38;5;{i}██";

                //Console.Write($"\x1b]4;{i};rgb:{i}/{i}/{i}\x1b");
            }



            while (true)
            {
                Settings.screenWidth = (Console.WindowWidth);
                Settings.screenHeight = Console.WindowHeight-2;
                Console.CursorVisible = false;
                //Console.ForegroundColor = ConsoleColor.White;

                
                lock (nextFrameLock)
                {
                    if (frameReady)
                    {
                        lastFrame = frame;
                        frame = new int[nextFrame.GetLength(0), nextFrame.GetLength(1)];
                        width = nextFrame.GetLength(0);
                        height = nextFrame.GetLength(1);

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                if ((nextFrame[x, y]) >= 0)
                                    frame[x, y] = (int)(nextFrame[x, y]);
                                else
                                    frame[x, y] = 0;

                            }
                        }
                        frameReady = false;
                    }
                }

                sw.Restart();
                
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

                sw.Stop();

                
                /*
                if (saveNum < 1256)
                {
                    DirectoryInfo di;
                    di = new DirectoryInfo("./save");
                    if (!di.Exists) { di.Create(); }

                    PrintScreen ps = new PrintScreen();
                    ps.CaptureScreenToFile(di + "\\" + saveNum.ToString("D5") + ".png");

                    saveNum++;
                }
                else
                {
                    break;
                }*/


                
                if (sw.ElapsedMilliseconds > 0)
                {
                    Console.SetCursorPosition(0, height);

                    int fps = 0;

                    for (int i = 0; i < fpsCounter.Count; i++)
                    {
                        fps += fpsCounter[i];
                    }

                    fps /= fpsCounter.Count;

                    Console.WriteLine("\u001b[38;2;255;255;255m" + fps + "        ");

                    fpsCounter.Insert(0, (int)(1000 / sw.ElapsedMilliseconds));
                    fpsCounter.RemoveAt(fpsCounter.Count-1);

                    if (sw.ElapsedMilliseconds < 16)
                    {
                        Thread.Sleep((int)(16 - sw.ElapsedMilliseconds));
                    }
                }

                lastColor = 255;

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

        private static void screenWriteAll()
        {
            List<char> scChars = new List<char>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    scChars.AddRange(getColor(frame[x, y]));
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
            
            for (int y = 0; y < height; y++)
            {
                int lastX = 0;
                for (int x = 0; x < width; x++)
                {
                    if (colorChanged(frame[x,y],lastFrame[x,y]))
                    {
                        if (lastX != x-1)
                        {
                            scChars.AddRange($"\u001b[{y};{x}H");
                            lastX = x;
                        }
                        
                        scChars.AddRange(getColor(frame[x, y]));
                    }
                }
            }

            Console.Write(scChars.ToArray());
        }

        private static bool bufferInicDone = false;
        private static void moveScreenBuffer()
        {
            if (!bufferInicDone)
            {

            }
        }


        private static char[] getColor(int pixel)
        {
            switch (Settings.colorMode)
            {
                case Settings.colorModes.charPoolBrightness:
                    return getCharPoolBrightness(pixel);
                case Settings.colorModes.consoleColor:
                    break;
                case Settings.colorModes.virtualTerminalColor:
                    return getVirtualTerminalColor(pixel);
            }

            return " ".ToCharArray();
        }

        private static bool colorChanged(float first, float second)
        {
            switch (Settings.colorMode)
            {
                case Settings.colorModes.charPoolBrightness:
                    return (int)Remap(first, 0, 255, 0, charPool.Length - 1) != (int)Remap(second, 0, 255, 0, charPool.Length - 1);
                case Settings.colorModes.consoleColor:
                    break;
                case Settings.colorModes.virtualTerminalColor:
                    return first != second;
            }

            return true;
        }

        

        private static char[] getCharPoolBrightness(int pixel)
        {
            return charPool[(int)Remap(pixel, 0, 255, 0, charPool.Length-1)].ToCharArray();
        }



        private static int lastColor = 0;
        private static char[] getVirtualTerminalColor(int pixel)
        {
            //return $"\u001b[38;2;{pixel};{pixel};{pixel}m██";
            if (lastColor == pixel)
            {
                return "█".ToCharArray();
            }
            else
            {
                lastColor = pixel;
                return colors[pixel].ToCharArray();
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


        public static char getInput()
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