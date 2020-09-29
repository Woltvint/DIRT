
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DIRT
{
    public struct Ray
    {
        public Vector origin;
        public Vector direction;

        public int result;

        public Ray(Vector _origin, Vector _dir)
        {
            origin = _origin;
            direction = _dir;
            result = 0;
        }

        public void cast(List<Triangle> tri)
        {
            for (int i = 0; i < tri.Count; i++)
            {
                if(tri[i].intersects(origin,direction))
                {
                    result = (int)Math.Round(((Vector.angleDist(Settings.light, tri[i].normal) + 1) / 2) * 8);
                    return;
                }
            }

            result = 0;
        }

    }
}
