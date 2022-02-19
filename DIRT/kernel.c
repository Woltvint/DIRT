float3 normal(float3 p1, float3 p2, float3 p3)
{
    return normalize(cross(p2 - p1, p3 - p1));
}

float angleDist(float3 v1, float3 v2)
{
    return dot(normalize(v1), normalize(v2));
}

bool intersects(float3 A, float3 B, float3 C, float3 origin, float3 dir, float3* oP)
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

    *oP = P;

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

    struct vec t1;
    struct vec t2;
    struct vec t3;

    struct vec dist;

    float visible;
};

struct mat4x4
{
    float m[4][4];
};

float triangleArea(float3 A, float3 B, float3 C)
{
    float a = length(B - C);
    float b = length(A - C);
    float c = length(A - B);

   float s = (a + b + c) / 2;

    return sqrt(s * (s - a) * (s - b) * (s - c));
}

float3 vecToFloat3(struct vec v)
{
    return (float3)(v.x, v.y, v.z);
}

float3 triangleCenter(struct tri t)
{
    float3 A = vecToFloat3(t.p1);
    float3 B = vecToFloat3(t.p2);
    float3 C = vecToFloat3(t.p3);

    return ((A + B + C) / 3);
}


float4 vec4MatMult(float4 i, struct mat4x4 m)
{
    float4 o;

    o.x = i.x * m.m[0][0] + i.y * m.m[1][0] + i.z * m.m[2][0] + i.w * m.m[3][0];
    o.y = i.x * m.m[0][1] + i.y * m.m[1][1] + i.z * m.m[2][1] + i.w * m.m[3][1];
    o.z = i.x * m.m[0][2] + i.y * m.m[1][2] + i.z * m.m[2][2] + i.w * m.m[3][2];
    o.w = i.x * m.m[0][3] + i.y * m.m[1][3] + i.z * m.m[2][3] + i.w * m.m[3][3];

    return o;
}
float3 vec3MatMult(float3 i, struct mat4x4 m)
{
    float3 o;

    o.x = i.x * m.m[0][0] + i.y * m.m[1][0] + i.z * m.m[2][0];
    o.y = i.x * m.m[0][1] + i.y * m.m[1][1] + i.z * m.m[2][1];
    o.z = i.x * m.m[0][2] + i.y * m.m[1][2] + i.z * m.m[2][2];

    return o;
}

struct mat4x4 rotMatX(float rot)
{
    struct mat4x4 mat;

    mat.m[0][0] = 1;
    mat.m[1][1] = cos(rot);
    mat.m[1][2] = sin(rot);
    mat.m[2][1] = -sin(rot);
    mat.m[2][2] = cos(rot);
    mat.m[3][3] = 1;

    return mat;
}

struct mat4x4 rotMatY(float rot)
{
    struct mat4x4 mat;

    mat.m[0][0] = cos(rot);
    mat.m[0][2] = sin(rot);
    mat.m[2][0] = -sin(rot);
    mat.m[1][1] = 1;
    mat.m[2][2] = cos(rot);
    mat.m[3][3] = 1;

    return mat;
}

struct mat4x4 rotMatZ(float rot)
{
    struct mat4x4 mat;

    mat.m[0][0] = cos(rot);
    mat.m[0][1] = sin(rot);
    mat.m[1][0] = -sin(rot);
    mat.m[1][1] = cos(rot);
    mat.m[2][2] = 1;
    mat.m[3][3] = 1;

    return mat;
}

struct mat4x4 projMat()
{
    float far = 1000;
    float near = 0.1f;
    float a = 1.3f/2.3f;
    float f = 1 / tan((1.570796f/3));
    float q = far / (far - near);

    struct mat4x4 m;


    m.m[0][0] = a * f;
    m.m[1][1] = f;
    m.m[2][2] = far / (far - near);
    m.m[3][2] = (-far * near) / (far - near);
    m.m[2][3] = 1.0f;
    m.m[3][3] = 0.0f;

    return m;
}

struct mat4x4 pointAt(float3 pos, float3 target, float3 up)
{
    // Calculate new forward direction
    float3 newForward = target;
    newForward = normalize(newForward);

    // Calculate new Up direction
    float3 a = newForward * dot(up, newForward);
    float3 newUp = up - a;
    newUp = normalize(newUp);

    // New Right direction is easy, its just cross product
    float3 newRight = cross(newUp, newForward);

    // Construct Dimensioning and Translation Matrix	
    struct mat4x4 matrix;
    matrix.m[0][0] = newRight.x;	matrix.m[0][1] = newRight.y;	matrix.m[0][2] = newRight.z;	matrix.m[0][3] = 0.0f;
    matrix.m[1][0] = newUp.x;		matrix.m[1][1] = newUp.y;		matrix.m[1][2] = newUp.z;		matrix.m[1][3] = 0.0f;
    matrix.m[2][0] = newForward.x;	matrix.m[2][1] = newForward.y;	matrix.m[2][2] = newForward.z;	matrix.m[2][3] = 0.0f;
    matrix.m[3][0] = pos.x;			matrix.m[3][1] = pos.y;			matrix.m[3][2] = pos.z;			matrix.m[3][3] = 1.0f;
    return matrix;
}

struct mat4x4 matInvert(struct mat4x4 m)
{
    struct mat4x4 matrix;
    matrix.m[0][0] = m.m[0][0]; matrix.m[0][1] = m.m[1][0]; matrix.m[0][2] = m.m[2][0]; matrix.m[0][3] = 0.0f;
    matrix.m[1][0] = m.m[0][1]; matrix.m[1][1] = m.m[1][1]; matrix.m[1][2] = m.m[2][1]; matrix.m[1][3] = 0.0f;
    matrix.m[2][0] = m.m[0][2]; matrix.m[2][1] = m.m[1][2]; matrix.m[2][2] = m.m[2][2]; matrix.m[2][3] = 0.0f;
    matrix.m[3][0] = -(m.m[3][0] * matrix.m[0][0] + m.m[3][1] * matrix.m[1][0] + m.m[3][2] * matrix.m[2][0]);
    matrix.m[3][1] = -(m.m[3][0] * matrix.m[0][1] + m.m[3][1] * matrix.m[1][1] + m.m[3][2] * matrix.m[2][1]);
    matrix.m[3][2] = -(m.m[3][0] * matrix.m[0][2] + m.m[3][1] * matrix.m[1][2] + m.m[3][2] * matrix.m[2][2]);
    matrix.m[3][3] = 1.0f;
    return matrix;
}


__kernel void prepTris(global struct tri* tris, global float* trisCount, global struct vec* rots, global struct vec* poss, global struct vec* camera)
{
        int i = get_global_id(0);

        tris[i].visible = 0;

        float3 points[3];

        points[0] = (float3)(tris[i].p1.x, tris[i].p1.y, tris[i].p1.z);
        points[1] = (float3)(tris[i].p2.x, tris[i].p2.y, tris[i].p2.z);
        points[2] = (float3)(tris[i].p3.x, tris[i].p3.y, tris[i].p3.z);

        float3 rot = (float3)(rots[i].x, rots[i].y, rots[i].z);
        float3 pos = (float3)(poss[i].x, poss[i].y, poss[i].z);
        

        for (int p = 0; p < 3; p++)
        {
            points[p] = vec3MatMult(points[p], rotMatX(rot.x));
            points[p] = vec3MatMult(points[p], rotMatY(rot.y));
            points[p] = vec3MatMult(points[p], rotMatZ(rot.z));

            points[p] = points[p] + pos;
        }

        float3 n = normal(points[0], points[1], points[2]);

        float3 camPos = (float3)(camera[0].x, camera[0].y, camera[0].z);
        float3 camLook = (float3)(camera[1].x, camera[1].y, camera[1].z);

        if (angleDist(camLook, n) > 0.05)
            return;

        float Dist1 = fast_distance(points[0], camPos);
        float Dist2 = fast_distance(points[1], camPos);
        float Dist3 = fast_distance(points[2], camPos);
        float dist = trisCount[3];

        if (Dist1 > dist && Dist2 > dist && Dist3 > dist)
            return;

        tris[i].dist.x = Dist1;
        tris[i].dist.y = Dist2;
        tris[i].dist.z = Dist3;

        /*
        if (angleDist(points[0] - camPos, camLook) < 0)
            tris[i].visible = 0;
        else
            tris[i].visible = 1;

        if (angleDist(points[1] - camPos, camLook) < 0)
            tris[i].visible = 0;
        else
            tris[i].visible = 1;

        if (angleDist(points[2] - camPos, camLook) < 0)
            tris[i].visible = 0;
        else
            tris[i].visible = 1;*/

        tris[i].p1.x = points[0].x;
        tris[i].p1.y = points[0].y;
        tris[i].p1.z = points[0].z;

        tris[i].p2.x = points[1].x;
        tris[i].p2.y = points[1].y;
        tris[i].p2.z = points[1].z;

        tris[i].p3.x = points[2].x;
        tris[i].p3.y = points[2].y;
        tris[i].p3.z = points[2].z;


        tris[i].visible = 1; 
} 

__kernel void ray(global struct tri* tris, global struct vec* lights, global float* _trisCount, global struct vec* _origin, global struct vec* _dir, global float* texture, global float* _output) {

    int index = get_global_id(0);

    float3 origin = (float3)(_origin[index].x, _origin[index].y, _origin[index].z);

    float3 dir = (float3)(_dir[index+1].x, _dir[index+1].y, _dir[index+1].z);

    float3 rot = (float3)(_dir[0].x, _dir[0].y, _dir[0].z);
    
    dir = vec3MatMult(dir, rotMatX(rot.x));
    dir = vec3MatMult(dir, rotMatY(rot.y));
    dir = vec3MatMult(dir, rotMatZ(rot.z));


    int trisCount = _trisCount[0];

    float3 light = (float3)(lights[0].x, lights[0].y, lights[0].z);
    

    float dist = _trisCount[3];
    int distIndex = -1;
    bool cast = false;

    float3 fP;

    for (int i = 0; i < trisCount; i++)
    {
        if (tris[i].visible == 0)
        {
            continue;
        }
        
        if (tris[i].dist.x > dist && tris[i].dist.y > dist && tris[i].dist.z > dist)
            continue;

        float3 A = (float3)(tris[i].p1.x, tris[i].p1.y, tris[i].p1.z);
        float3 B = (float3)(tris[i].p2.x, tris[i].p2.y, tris[i].p2.z);
        float3 C = (float3)(tris[i].p3.x, tris[i].p3.y, tris[i].p3.z);

        float3 P;

        if (intersects(A, B, C, origin, dir, &P))
        {

            float nDist = fast_distance(origin, P);

            if (nDist < dist)
            {
                dist = nDist;
                distIndex = i;
                
                _output[(index * 3)] = 255;
                _output[(index * 3) + 1] = 255;
                _output[(index * 3) + 2] = 255;

                cast = true;
                fP = P;
            }
        }
    }

    
    if (cast)
    {
        float3 A = (float3)(tris[distIndex].p1.x, tris[distIndex].p1.y, tris[distIndex].p1.z);
        float3 B = (float3)(tris[distIndex].p2.x, tris[distIndex].p2.y, tris[distIndex].p2.z);
        float3 C = (float3)(tris[distIndex].p3.x, tris[distIndex].p3.y, tris[distIndex].p3.z);
        
        float2 tA = (float2)(tris[distIndex].t1.x, tris[distIndex].t1.y);
        float2 tB = (float2)(tris[distIndex].t2.x, tris[distIndex].t2.y);
        float2 tC = (float2)(tris[distIndex].t3.x, tris[distIndex].t3.y);

        float3 P = fP;

        

        float br = ((angleDist(light, normal(A, B, C))));

        if (tA.x < 0)
        {
            _output[(index * 3)] = ((br+1)/2) * 255;
            _output[(index * 3) + 1] = ((br + 1) / 2) * 255;
            _output[(index * 3) + 2] = ((br + 1) / 2) * 255;
        }
        else
        {
            float ABC = triangleArea(A, B, C);

            float PBC = triangleArea(P, B, C);
            float PAC = triangleArea(P, A, C);
            float PAB = triangleArea(P, A, B);

            float u = PBC / ABC;
            float v = PAC / ABC;
            float w = PAB / ABC;

            float2 tP = ((tA * u) + (tB * v) + (tC * w));

            float R = 255;
            float G = 255;
            float B = 255;

            if (tP.x >= 0 && tP.x < 1000 && tP.y >= 0 && tP.y < 1000)
            {
                R = texture[((((int)tP.x + (1000 * (int)tP.y)) * 3))];
                G = texture[((((int)tP.x + (1000 * (int)tP.y)) * 3) + 1)];
                B = texture[((((int)tP.x + (1000 * (int)tP.y)) * 3) + 2)];
            }

            float colorBr = (br + 1.0f) / 2.0f;

            _output[(index * 3)] = R * colorBr;
            _output[(index * 3) + 1] = G * colorBr;
            _output[(index * 3) + 2] = B * colorBr;
        }

        float change = 0;
        
        for (int a = 0; a < trisCount; a++)
        {
            if (distIndex == a)
            {
                continue;
            }

            float3 sA = (float3)(tris[a].p1.x, tris[a].p1.y, tris[a].p1.z);
            float3 sB = (float3)(tris[a].p2.x, tris[a].p2.y, tris[a].p2.z);
            float3 sC = (float3)(tris[a].p3.x, tris[a].p3.y, tris[a].p3.z);

            if (intersectsShadow(sA, sB, sC, P, light))
            {
                _output[index * 3] *= 0.9f;
                _output[(index * 3) + 1] *= 0.9f;
                _output[(index * 3) + 2] *= 0.9f;
                break;
            }

        }

        _output[index * 3] += change;
        _output[(index * 3) + 1] += change;
        _output[(index * 3) + 2] += change;
    }
    else
    {
        _output[index * 3] = _trisCount[4];
        _output[(index * 3)+1] = _trisCount[5];
        _output[(index * 3)+2] = _trisCount[6];
    }
   
    
}

