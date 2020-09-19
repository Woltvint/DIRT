using System;

namespace DIRT
{
    public static class Settings
    {
        public static double width = 1000;
        public static double height = 900;
        public static double aspectRatio
        {
            get
            {
                return height / width;;
            }
        }


        public static double zFar = 1000;
        public static double zNear = 0.1;

        public static double fov = Math.PI/2;

        public static double scale = 1;

        public static Vector camera = new Vector(0,0,-1);
        public static Vector light = new Vector(-1,-1,-1);

        public static Vector globalRot = new Vector(Math.PI / 3, 0,0);
    }
}
