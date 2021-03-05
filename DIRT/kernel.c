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

    struct vec t1;
    struct vec t2;
    struct vec t3;

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

void swap(float3* xp, float3* yp)
{
    float3 temp = *xp;
    *xp = *yp;
    *yp = temp;
}

// Function to perform Selection Sort 
void selectionSort(struct tri arr[], int n, float3 cam)
{
    int i, j, min_idx;

    // One by one move boundary of unsorted subarray 
    for (i = 0; i < n - 1; i++) {

        // Find the minimum element in unsorted array 
        min_idx = i;
        for (j = i + 1; j < n; j++)
            if (fast_distance(triangleCenter(arr[j]), cam) < fast_distance(triangleCenter(arr[min_idx]), cam))
                min_idx = j;

        // Swap the found minimum element 
        // with the first element 
        swap(&arr[min_idx], &arr[i]);
    }
}



__kernel void prepTris(global struct tri* tris, global int* trisCount, global struct vec* rots, global struct vec* poss, global struct vec* camera)
{
    int i = get_global_id(0);

    //float3 globRot = (float3)(globalRot[0].x, globalRot[0].y, globalRot[0].z);

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

            /*points[p] = vec3MatMult(points[p], rotMatX(globRot.x));
            points[p] = vec3MatMult(points[p], rotMatY(globRot.y));
            points[p] = vec3MatMult(points[p], rotMatZ(globRot.z));*/
        }

        float3 n = normal(points[0], points[1], points[2]);

        float3 camPos = (float3)(camera[0].x, camera[0].y, camera[0].z);
        float3 camLook = (float3)(camera[1].x, camera[1].y, camera[1].z);

        if (angleDist(camLook, n) > 0)
            tris[i].visible = 0;
        else
            tris[i].visible = 1;

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
            tris[i].visible = 1;
        

        tris[i].p1.x = points[0].x;
        tris[i].p1.y = points[0].y;
        tris[i].p1.z = points[0].z;

        tris[i].p2.x = points[1].x;
        tris[i].p2.y = points[1].y;
        tris[i].p2.z = points[1].z;

        tris[i].p3.x = points[2].x;
        tris[i].p3.y = points[2].y;
        tris[i].p3.z = points[2].z;
}



__kernel void ray(global struct tri* tris, global struct vec* lights, global int* _trisCount, global struct vec* _origin, global struct vec* _dir, global float* texture, global float* _output) {

    int index = get_global_id(0);

    float3 origin = (float3)(_origin[index].x, _origin[index].y, _origin[index].z);

    float3 dir = (float3)(_dir[index+1].x, _dir[index+1].y, _dir[index+1].z);

    float3 rot = (float3)(_dir[0].x, _dir[0].y, _dir[0].z);
    /*
    dir = vec3MatMult(dir, rotMatX(rot.x));
    dir = vec3MatMult(dir, rotMatY(rot.y));
    dir = vec3MatMult(dir, rotMatZ(rot.z));*/


    int trisCount = _trisCount[0];

    float3 light = (float3)(lights[0].x, lights[0].y, lights[0].z);
    //float3 light = (float3)(1, 1, 1);

    float dist = 100;
    int distIndex = -1;
    bool cast = false;
    


    for (int i = 0; i < trisCount; i++)
    {
        if (tris[i].visible == 0)
        {
            continue;
        }

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
                
                _output[(index * 3)] = 255;
                _output[(index * 3) + 1] = 255;
                _output[(index * 3) + 2] = 255;

                cast = true;
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

        float3 P = intersectPoint(A, B, C, origin, dir);

        float br = ((angleDist(light, normal(A, B, C))));

        if (tA.x < 0)
        {
            _output[(index * 3)] = br * 255;
            _output[(index * 3) + 1] = br * 255;
            _output[(index * 3) + 2] = br * 255;
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
                //change = -64;
                break;
            }

        }

        _output[index * 3] += change;
        _output[(index * 3) + 1] += change;
        _output[(index * 3) + 2] += change;
    }
    else
    {
        _output[index * 3] = 0;
        _output[(index * 3)+1] = 0;
        _output[(index * 3)+2] = 0;
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


















float4 projectPoint(float4 p,int w,int h)
{
    float4 output = (float4)(p.x,p.y,p.z,1);

    output = vec4MatMult(output, projMat());
    

    output /= (output.w);
    


    output.x *= 0.5f * w;
    output.y *= 0.5f * h;

    return (float4)(output.x, output.y, output.z,output.w);
}

/*struct tri projectTris(struct tri triangle,float3 rot, float3 pos, float3 globalRot,int w,int h)
{
    struct tri t;
    
    float3 A = (float3)(triangle.p1.x, triangle.p1.y, triangle.p1.z);
    float3 B = (float3)(triangle.p2.x, triangle.p2.y, triangle.p2.z);
    float3 C = (float3)(triangle.p3.x, triangle.p3.y, triangle.p3.z);

    A = projectPoint(A, rot, pos, globalRot, w, h);
    B = projectPoint(B, rot, pos, globalRot, w, h);
    C = projectPoint(C, rot, pos, globalRot, w, h);

    t.p1.x = A.x;
    t.p1.y = A.y;
    t.p1.z = A.z;

    t.p2.x = B.x;
    t.p2.y = B.y;
    t.p2.z = B.z;

    t.p3.x = C.x;
    t.p3.y = C.y;
    t.p3.z = C.z;

    return t;
}*/



__kernel void mathway(global struct tri* tris, global int* trisCount, global struct vec* camera, global float* _output)
{
    int count = trisCount[0];
    int width = trisCount[1];
    int height = trisCount[2];
    
    int i = get_global_id(0);

    if (tris[i].visible == 0)
    {
        return;
    }

    float3 oA = (float3)(tris[i].p1.x, tris[i].p1.y, tris[i].p1.z);
    float3 oB = (float3)(tris[i].p2.x, tris[i].p2.y, tris[i].p2.z);
    float3 oC = (float3)(tris[i].p3.x, tris[i].p3.y, tris[i].p3.z);

    float3 camPos = (float3)(camera[0].x, camera[0].y, camera[0].z);
    float3 camLook = (float3)(camera[1].x, camera[1].y, camera[1].z);
    float3 camUp = (float3)(camera[2].x, camera[2].y, camera[2].z);

    float3 n = normal(oA, oB, oC);
    
    if (angleDist(n, oA - camPos) > 0)
    {
        return;
    }

    struct mat4x4 view = matInvert(pointAt(camPos, camLook, camUp));

    float4 A = (float4)(tris[i].p1.x, tris[i].p1.y, tris[i].p1.z, 1);
    float4 B = (float4)(tris[i].p2.x, tris[i].p2.y, tris[i].p2.z, 1);
    float4 C = (float4)(tris[i].p3.x, tris[i].p3.y, tris[i].p3.z, 1);

    A = vec4MatMult(A, view);
    B = vec4MatMult(B, view);
    C = vec4MatMult(C, view);

    A = projectPoint(A, width, height);
    B = projectPoint(B, width, height);
    C = projectPoint(C, width, height);
    
    
    if (C.y > B.y)
        swap(&C, &B);
    if (C.y > A.y)
        swap(&C, &A);
    if (B.y > A.y)
        swap(&B, &A);

    int brightness = ((angleDist((float3)(1,-1,-1), n)) * 256);


    int hw = (width / 2);
    int hh = (height / 2);
    /*
    int y1 = (int)C.y;
    int y2 = (int)B.y;
    int y3 = (int)A.y;

    int x1 = (int)C.x;
    int x2 = (int)B.x;
    int x3 = (int)A.x;*/

    
    int x1 = (int)A.x;
    int y1 = (int)A.y;
    float u1 = 0;
    float v1 = 0;
    float w1 = A.w;

    int x2 = (int)B.x;
    int y2 = (int)B.y;
    float u2 = 0;
    float v2 = 0;
    float w2 = B.w;

    int x3 = (int)C.x;
    int y3 = (int)C.y;
    float u3 = 0;
    float v3 = 0;
    float w3 = C.w;

    if (y2 < y1)
    {
        swap(&y1, &y2);
        swap(&x1, &x2);
        swap(&u1, &u2);
        swap(&v1, &v2);
        swap(&w1, &w2);
    }

    if (y3 < y1)
    {
        swap(&y1, &y3);
        swap(&x1, &x3);
        swap(&u1, &u3);
        swap(&v1, &v3);
        swap(&w1, &w3);
    }

    if (y3 < y2)
    {
        swap(&y2, &y3);
        swap(&x2, &x3);
        swap(&u2, &u3);
        swap(&v2, &v3);
        swap(&w2, &w3);
    }
    
    int dy1 = y2 - y1;
    int dx1 = x2 - x1;
    float dv1 = v2 - v1;
    float du1 = u2 - u1;
    float dw1 = w2 - w1;

    int dy2 = y3 - y1;
    int dx2 = x3 - x1;
    float dv2 = v3 - v1;
    float du2 = u3 - u1;
    float dw2 = w3 - w1;

    float tex_u, tex_v, tex_w;

    float dax_step = 0, dbx_step = 0,
        du1_step = 0, dv1_step = 0,
        du2_step = 0, dv2_step = 0,
        dw1_step = 0, dw2_step = 0;

    if (dy1) dax_step = dx1 / (float)abs(dy1);
    if (dy2) dbx_step = dx2 / (float)abs(dy2);

    if (dy1) du1_step = du1 / (float)abs(dy1);
    if (dy1) dv1_step = dv1 / (float)abs(dy1);
    if (dy1) dw1_step = dw1 / (float)abs(dy1);

    if (dy2) du2_step = du2 / (float)abs(dy2);
    if (dy2) dv2_step = dv2 / (float)abs(dy2);
    if (dy2) dw2_step = dw2 / (float)abs(dy2);

    if (dy1)
    {
        for (int i = y1; i <= y2; i++)
        {
            int ax = x1 + (float)(i - y1) * dax_step;
            int bx = x1 + (float)(i - y1) * dbx_step;

            float tex_su = u1 + (float)(i - y1) * du1_step;
            float tex_sv = v1 + (float)(i - y1) * dv1_step;
            float tex_sw = w1 + (float)(i - y1) * dw1_step;

            float tex_eu = u1 + (float)(i - y1) * du2_step;
            float tex_ev = v1 + (float)(i - y1) * dv2_step;
            float tex_ew = w1 + (float)(i - y1) * dw2_step;

            if (ax > bx)
            {
                swap(&ax, &bx);
                swap(&tex_su, &tex_eu);
                swap(&tex_sv, &tex_ev);
                swap(&tex_sw, &tex_ew);
            }

            tex_u = tex_su;
            tex_v = tex_sv;
            tex_w = tex_sw;

            float tstep = 1.0f / ((float)(bx - ax));
            float t = 0.0f;

            for (int j = ax; j < bx; j++)
            {
                tex_u = (1.0f - t) * tex_su + t * tex_eu;
                tex_v = (1.0f - t) * tex_sv + t * tex_ev;
                tex_w = (1.0f - t) * tex_sw + t * tex_ew;
                /*if (tex_w > pDepthBuffer[i * ScreenWidth() + j])
                {
                    Draw(j, i, tex->SampleGlyph(tex_u / tex_w, tex_v / tex_w), tex->SampleColour(tex_u / tex_w, tex_v / tex_w));
                    pDepthBuffer[i * ScreenWidth() + j] = tex_w;
                }*/
                if (j + hw >= 0 && j + hw < width && i + hh >= 0 && i + hh < height && tex_w < 2)
                    _output[(j+hw) + ((i+hh) * width)] = brightness;
                t += tstep;
            }

        }
    }

    dy1 = y3 - y2;
    dx1 = x3 - x2;
    dv1 = v3 - v2;
    du1 = u3 - u2;
    dw1 = w3 - w2;

    if (dy1) dax_step = dx1 / (float)abs(dy1);
    if (dy2) dbx_step = dx2 / (float)abs(dy2);

    du1_step = 0, dv1_step = 0;
    if (dy1) du1_step = du1 / (float)abs(dy1);
    if (dy1) dv1_step = dv1 / (float)abs(dy1);
    if (dy1) dw1_step = dw1 / (float)abs(dy1);

    if (dy1)
    {
        for (int i = y2; i <= y3; i++)
        {
            int ax = x2 + (float)(i - y2) * dax_step;
            int bx = x1 + (float)(i - y1) * dbx_step;

            float tex_su = u2 + (float)(i - y2) * du1_step;
            float tex_sv = v2 + (float)(i - y2) * dv1_step;
            float tex_sw = w2 + (float)(i - y2) * dw1_step;

            float tex_eu = u1 + (float)(i - y1) * du2_step;
            float tex_ev = v1 + (float)(i - y1) * dv2_step;
            float tex_ew = w1 + (float)(i - y1) * dw2_step;

            if (ax > bx)
            {
                swap(&ax, &bx);
                swap(&tex_su, &tex_eu);
                swap(&tex_sv, &tex_ev);
                swap(&tex_sw, &tex_ew);
            }

            tex_u = tex_su;
            tex_v = tex_sv;
            tex_w = tex_sw;

            float tstep = 1.0f / ((float)(bx - ax));
            float t = 0.0f;

            for (int j = ax; j < bx; j++)
            {
                tex_u = (1.0f - t) * tex_su + t * tex_eu;
                tex_v = (1.0f - t) * tex_sv + t * tex_ev;
                tex_w = (1.0f - t) * tex_sw + t * tex_ew;

                /*if (tex_w > pDepthBuffer[i * ScreenWidth() + j])
                {
                    Draw(j, i, tex->SampleGlyph(tex_u / tex_w, tex_v / tex_w), tex->SampleColour(tex_u / tex_w, tex_v / tex_w));
                    pDepthBuffer[i * ScreenWidth() + j] = tex_w;
                }*/
                if (j+hw >= 0 && j+hw < width && i+hh >= 0 && i+hh < height && tex_w < 2)
                    _output[(j + hw) + ((i + hh) * width)] = brightness;
                t += tstep;
            }
        }
    }


    /*
    float mid = 1/(y3+y1);

    int y2y1 = y2 - y1;

    for (int i = 0; i <= y2y1; i++)
    {
        float xStep1 = x2 - x1;
        xStep1 /= y2y1;

        float xStep2 = x3 - x1;
        xStep2 /= (y3 - y1);
        
        if (xStep1 > xStep2)
        {
            swap(&xStep1, &xStep2);
        }

        for (int u = (int)(xStep1*i); u <= (int)(xStep2 * i); u++)
        {
            int X = x1 + hw + u;
            int Y = y1 + i + hh;

            _output[X + (Y * width)] = brightness;
        }

    }

    int y3y2 = y3 - y2;

    for (int i = 0; i <= y3y2; i++)
    {
        float xStep1 = x3 - x2;
        xStep1 /= y3y2;

        float xStep2 = x3 - x1;
        xStep2 /= (y3 - y1);

        if (xStep1 > xStep2)
        {
            swap(&xStep1, &xStep2);
        }

        for (int u = (int)(xStep1 * i); u <= (int)(xStep2 * i); u++)
        {
            int X = x3 + hw - u;
            int Y = y3 - i + hh;

            _output[X + (Y * width)] = brightness;
        }
    }



    _output[(x1 + hw) + ((y1 + hh) * width)] = 128;
    _output[(x2 + hw) + ((y2 + hh) * width)] = 128;
    _output[(x3 + hw) + ((y3 + hh) * width)] = 128;
    */
    /*if (y2 != y3)
    {

    }*/

    /*
    A.z = oA.z;
    B.z = oB.z;
    C.z = oC.z;*/
    /*
    int res = 200;

    float4 AB = (B - A)/res;
    float4 AC = (C - A)/res;
    float4 BC = (C - B)/res;



    for (int i = 0; i < res; i++)
    {
        float4 AP = A + (AB * i);
        float4 BP = A + (AC * i);
        float4 CP = B + (BC * i);

        int APX = hw + (int)AP.x;
        int APY = hh + (int)AP.y;
        int APZ = (int)AP.z;

        int BPX = hw + (int)BP.x;
        int BPY = hh + (int)BP.y;
        int BPZ = (int)BP.z;

        int CPX = hw + (int)CP.x;
        int CPY = hh + (int)CP.y;
        int CPZ = (int)CP.z;

        if (APX >= 0 && APX < width && APY >= 0 && APY < height)
        {
            if (APZ < 1)
            _output[APX + (APY * width)] = 255;
        }

        if (BPX >= 0 && BPX < width && BPY >= 0 && BPY < height)
        {
            if (APZ < 1)
            _output[BPX + (BPY * width)] = 255;
        }

        if (CPX >= 0 && CPX < width && CPY >= 0 && CPY < height)
        {
            if (APZ < 1)
            _output[CPX + (CPY * width)] = 255;
        }


    }*/

    

    
    /*for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            if (x == y)
            _output[x + (y * width)] = 255;

            
        }
    }*/
}