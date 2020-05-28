//UNITY_SHADER_NO_UPGRADE
#ifndef WAVES_INCLUDE
#define WAVES_INCLUDE

void TriangleWave_float(float In, float Period, float Offset, out float Out) {
    In = (In + Offset) * Period * 2 + 1;
    Out = abs((In % 2) - 1);
}

void RepetitionWidth_float(float2 Border, float2 MiddleTileCount, out float2 Out) {
    float2 m = 1 - 2 * Border;
    float2 d0 = m * MiddleTileCount;

    Out = d0;
}

// void RewaveSegment_float(float In, float Border, float Period, out float Out) {
//     float Ceiling = 1 - Border * 2;
//     float tmp = 
// }

#endif // WAVES_INCLUDE

