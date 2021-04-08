using System;
using DIRT.Types;

namespace DIRT
{
    public static class Settings
    {
        private static float width = 230;
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

        public static bool screenAutoSize = false;

        public enum screenModes
        {
            trueColor,
            graySpeed
        }

        public static screenModes screenMode = screenModes.trueColor;

        public static Vector camera = new Vector(0,0,-1);
        public static Vector cameraRot = new Vector(0, 0, 0);
        public static Vector light = new Vector(1,1,1);
    }
}
