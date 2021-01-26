float3 normal(float3 p1, float3 p2, float3 p3)
{
    return normalize(cross(p2 - p1, p3 - p1));
}

float angleDist(float3 v1, float3 v2)
{
    return dot(normalize(v1), normalize(v2));
}

bool intersects(float3 A, float3 B, float3 C, float3 origin, float3 dir)
{
    float3 N = normal(A, B, C);
    
    if (angleDist(N, dir) > 0)
    {
        return false;
    }

    float NdotRayDir = dot(N, dir);

    if (fabs(NdotRayDir) < 0.000000001f)
    {
        return false;
    }

    float D = dot(N, A);

    float t = (dot(N, origin) - D) / -NdotRayDir;

    if (t <= 0)
    {
        return false;
    }

    float3 P = origin + (dir * t);

    float3 Edge1 = B - A;
    float3 AP = P - A;

    if (dot(N, cross(Edge1, AP)) < 0)
    {
        return false;
    }


    float3 Edge2 = C - B;
    float3 BP = P - B;

    if (dot(N, cross(Edge2, BP)) < 0)
    {
        return false;
    }


    float3 Edge3 = A - C;
    float3 CP = P - C;

    if (dot(N, cross(Edge3, CP)) < 0)
    {
        return false;
    }

    return true;
}

bool intersectsShadow(float3 A, float3 B, float3 C, float3 origin, float3 dir)
{
    float3 N = normal(A, B, C);

    float NdotRayDir = dot(N, dir);

    if (fabs(NdotRayDir) < 0.000000001f)
    {
        return false;
    }

    float D = dot(N, A);

    float t = (dot(N, origin) - D) / -NdotRayDir;

    if (t <= 0)
    {
        return false;
    }

    float3 P = origin + (dir * t);

    float3 Edge1 = B - A;
    float3 AP = P - A;

    if (dot(N, cross(Edge1, AP)) < 0)
    {
        return false;
    }


    float3 Edge2 = C - B;
    float3 BP = P - B;

    if (dot(N, cross(Edge2, BP)) < 0)
    {
        return false;
    }


    float3 Edge3 = A - C;
    float3 CP = P - C;

    if (dot(N, cross(Edge3, CP)) < 0)
    {
        return false;
    }

    return true;
}

bool intersectsShadowPoint(float3 A, float3 B, float3 C, float3 origin, float3 dir,float3 point)
{
    float3 N = normal(A, B, C);

    float NdotRayDir = dot(N, dir);

    if (fabs(NdotRayDir) < 0.000000001f)
    {
        return false;
    }

    float D = dot(N, A);

    float t = (dot(N, origin) - D) / -NdotRayDir;

    if (t <= 0)
    {
        return false;
    }
    
    if (t >= length((origin - point)) / length(dir))
    {
        return false;
    }

    float3 P = origin + (dir * t);

    float3 Edge1 = B - A;
    float3 AP = P - A;

    if (dot(N, cross(Edge1, AP)) < 0)
    {
        return false;
    }


    float3 Edge2 = C - B;
    float3 BP = P - B;

    if (dot(N, cross(Edge2, BP)) < 0)
    {
        return false;
    }


    float3 Edge3 = A - C;
    float3 CP = P - C;

    if (dot(N, cross(Edge3, CP)) < 0)
    {
        return false;
    }

    return true;
}

float3 intersectPoint(float3 A, float3 B, float3 C, float3 origin, float3 dir)
{
    float3 N = normal(A, B, C);

    float NdotRayDir = dot(N, dir);

    float D = dot(N, A);

    float t = (dot(N, origin) - D) / -NdotRayDir;

    float3 P = origin + (dir * t);

    return P;
}

struct vec
{
    float x;
    float y;
    float z;
};

struct tri
{
    struct vec p1;
    struct vec p2;
    struct vec p3;
};




__kernel void ray(global struct tri* tris, global struct vec* lights, global int* _trisCount, global struct vec* _origin, global struct vec* _dir, global float* _output) {

    int index = get_global_id(0);

    float3 origin = (float3)(_origin[index].x, _origin[index].y, _origin[index].z);

    float3 dir = (float3)(_dir[index].x, _dir[index].y, _dir[index].z);

    int trisCount = _trisCount[0];

    float3 light = (float3)(lights[0].x, lights[0].y, lights[0].z);
    //float3 light = (float3)(1, 1, 1);

    float dist = 30;
    int distIndex = -1;
    bool cast = false;

    for (int i = 0; i < trisCount; i++)
    {
        float3 A = (float3)(tris[i].p1.x, tris[i].p1.y, tris[i].p1.z);
        float3 B = (float3)(tris[i].p2.x, tris[i].p2.y, tris[i].p2.z);
        float3 C = (float3)(tris[i].p3.x, tris[i].p3.y, tris[i].p3.z);
        
        if (intersects(A, B, C, origin, dir))
        {
            float3 P = intersectPoint(A, B, C, origin, dir);

            if (fast_distance(origin, P) < dist)
            {
                dist = fast_distance(origin, P);
                distIndex = i;
                _output[index] = ((angleDist(light, normal(A, B, C))+1)*128);
                cast = true;
            }
        }
    }


    
    if (cast)
    {
        float3 A = (float3)(tris[distIndex].p1.x, tris[distIndex].p1.y, tris[distIndex].p1.z);
        float3 B = (float3)(tris[distIndex].p2.x, tris[distIndex].p2.y, tris[distIndex].p2.z);
        float3 C = (float3)(tris[distIndex].p3.x, tris[distIndex].p3.y, tris[distIndex].p3.z);
        
        float3 P = intersectPoint(A, B, C, origin, dir);

        float change = 0;
        
        for (int a = 0; a < trisCount; a++)
        {
            if (distIndex == a)
            {
                continue;
            }

            float3 tA = (float3)(tris[a].p1.x, tris[a].p1.y, tris[a].p1.z);
            float3 tB = (float3)(tris[a].p2.x, tris[a].p2.y, tris[a].p2.z);
            float3 tC = (float3)(tris[a].p3.x, tris[a].p3.y, tris[a].p3.z);

            if (intersectsShadow(tA, tB, tC, P, light))
            {
                change = 0;
                break;
            }

        }

        _output[index] += change;
    }
    else
    {
        _output[index] = 0;
    }
   
    
}


__kernel void rayLight(global struct tri* tris, global struct vec* lights, global int* _trisCount, global struct vec* _origin, global struct vec* _dir, global float* _output) {

    int index = get_global_id(0);

    float3 origin = (float3)(_origin[index].x, _origin[index].y, _origin[index].z);

    float3 dir = (float3)(_dir[index].x, _dir[index].y, _dir[index].z);

    int trisCount = _trisCount[0];
    int lightCount = _trisCount[1];

    float dist = 1000;
    float lightDist = 1000;
    int distIndex = -1;

    bool cast = false;

    float3 distP = (0, 0, 0);
    
    for (int i = 0; i < trisCount; i++)
    {
        float3 A = (float3)(tris[i].p1.x, tris[i].p1.y, tris[i].p1.z);
        float3 B = (float3)(tris[i].p2.x, tris[i].p2.y, tris[i].p2.z);
        float3 C = (float3)(tris[i].p3.x, tris[i].p3.y, tris[i].p3.z);

        if (intersects(A, B, C, origin, dir))
        {
            float3 P = intersectPoint(A, B, C, origin, dir);

            if (fast_distance(origin, P) < dist)
            {
                dist = fast_distance(origin, P);
                distIndex = i;
                distP = P;
                cast = true;
                _output[index] = 0;
            }
        }
    }

    if (cast)
    {
        float3 A = (float3)(tris[distIndex].p1.x, tris[distIndex].p1.y, tris[distIndex].p1.z);
        float3 B = (float3)(tris[distIndex].p2.x, tris[distIndex].p2.y, tris[distIndex].p2.z);
        float3 C = (float3)(tris[distIndex].p3.x, tris[distIndex].p3.y, tris[distIndex].p3.z);

        float3 P = intersectPoint(A, B, C, origin, dir);

        float change = 0;

        for (int l = 0; l < lightCount; l++)
        {
            float3 light = (float3)(lights[l].x, lights[l].y, lights[l].z);
            float shadow = 0;

            for (int a = 0; a < trisCount; a++)
            {
                if (distIndex == a)
                {
                    continue;
                }

                float3 tA = (float3)(tris[a].p1.x, tris[a].p1.y, tris[a].p1.z);
                float3 tB = (float3)(tris[a].p2.x, tris[a].p2.y, tris[a].p2.z);
                float3 tC = (float3)(tris[a].p3.x, tris[a].p3.y, tris[a].p3.z);

                if (intersectsShadowPoint(tA, tB, tC, P, light - P,light))
                {
                    shadow = -2;
                    break;
                }
            }

            float thisLight = pow(0.95f, (distance(light, P) * 2)) * 128 + shadow;

            change += thisLight;
        }
        _output[index] = change;
    }
    else
    {
        _output[index] = 0;
    }

    
}
