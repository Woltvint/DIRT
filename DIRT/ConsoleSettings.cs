using System;
using DIRT.Types;

namespace DIRT
{
    public static class ConsoleSettings
    {
        private static float width = 230;
        /// <summary>the width of the console window in characters</summary>
        public static float screenWidth
        {
            get
            {
                return width;
            }
            set
            {
                width = MathF.Round(value);
            }
        }

        private static float height = 130;
        /// <summary>the height of the console window in characters</summary>
        public static float screenHeight
        {
            get
            {
                return height;
            }
            set
            {
                height = MathF.Round(value);
            }
        }

        /// <summary>auto find the best screen size</summary>
        public static bool screenAutoSize = false;

        /// <summary>a distance after which triangles stop being rendered</summary>
        public static float renderDistance = 1000;

        /// <summary>the color of the sky (RGB default black)</summary>
        public static Vector backgroundColor = Vector.zero;

        public enum screenModes
        {
            trueColor,
            graySpeed
        }
        /// <summary>mode in which the screen will run</summary>
        public static screenModes screenMode = screenModes.trueColor;

        public static Vector camera = new Vector(0,0,0);
        public static Vector cameraRot = new Vector(0, 0, 0);
    }
}
