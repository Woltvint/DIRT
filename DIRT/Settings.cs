using System;

namespace DIRT
{
    public static class Settings
    {
        public static double width = 9;
        public static double height = 10;
        public static double aspectRatio
        {
            get
            {
                return width / height;;
            }
        }


        public static double zFar = 1000;
        public static double zNear = 0.1;

        public static double fov = Math.PI/2;

        public static double scale = /*17.9*/1;

        public static Vector camera = new Vector(0,0,-1);
        public static Vector light = new Vector(-1,-1,-0.5);

        public static Vector globalRot = new Vector(0, 0, 0, 0);
    }
}
