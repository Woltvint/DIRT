using System;
using DIRT.Types;

namespace DIRT
{
    public static class Settings
    {
        private static float width = 100;
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

        private static float height = 100;
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
        public static float aspectRatio
        {
            get
            {
                return screenWidth / screenHeight;;
            }
        }

        public enum renderModes
        {
            mathWay,
            raycast,
            raycastShadow,
            aiRender
        }

        public static renderModes renderMode = renderModes.raycast;

        public enum frameUpdateModes
        {
            writeAll,
            writeChanges,
            screenBuffer,
            smartWrite
        }

        public static frameUpdateModes frameUpdateMode = frameUpdateModes.writeChanges;

        public enum colorModes
        {
            charPoolBrightness,
            consoleColor,
            virtualTerminalColor
        }

        public static colorModes colorMode = colorModes.virtualTerminalColor;

        public static float zFar = 1000;
        public static float zNear = 0.1f;

        public static float fov = MathF.PI/2;

        public static Vector camera = new Vector(0,0,-1);
        public static Vector cameraRot = new Vector(0, 0, 0);
        public static Vector light = new Vector(1,1,1);

        public static Vector globalRot = new Vector(0, 0, 0, 0);
    }
}
