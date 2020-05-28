//UNITY_SHADER_NO_UPGRADE
#ifndef LINSTEP_INCLUDE
#define LINSTEP_INCLUDE

void linstep_float(float2 In, float2 Edge1, float2 Edge2, out float2 Out) {
    Out = (In - Edge1) / (Edge2 - Edge1); 
}

#endif // LINSTEP_INCLUDE