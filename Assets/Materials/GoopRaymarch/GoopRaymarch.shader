Shader "Raymarching/GoopRaymarch"
{

Properties
{
    [Header(Base)]
    [MainColor] _BaseColor("Color", Color) = (0.5, 0.5, 0.5, 1)
    [HideInInspector][MainTexture] _BaseMap("Albedo", 2D) = "white" {}
    [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.5
    _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5

    [Header(Pass)]
    [Enum(UnityEngine.Rendering.CullMode)] _Cull("Culling", Int) = 2
    [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc("Blend Src", Float) = 5 
    [Enum(UnityEngine.Rendering.BlendMode)] _BlendDst("Blend Dst", Float) = 10
    [Toggle][KeyEnum(Off, On)] _ZWrite("ZWrite", Float) = 1

    [Header(Raymarching)]
    _Loop("Loop", Range(1, 100)) = 30
    _MinDistance("Minimum Distance", Range(0.001, 0.1)) = 0.01
    _DistanceMultiplier("Distance Multiplier", Range(0.001, 2.0)) = 1.0

// @block Properties
// 
_Smooth("Goopiness", Range (0.5, 4.0)) = 2.0
_Radius("Radius", Float) = 0.5

// Metaballs
// _Ball0("Ball 0", Vector) = (0,0,0,0)
// _Ball1("Ball 1", Vector) = (0,0,0,0)
// _Ball2("Ball 2", Vector) = (0,0,0,0)
// _Ball3("Ball 3", Vector) = (0,0,0,0)
// _Ball4("Ball 4", Vector) = (0,0,0,0)
// _Ball5("Ball 5", Vector) = (0,0,0,0)
// _Ball6("Ball 6", Vector) = (0,0,0,0)
// _Ball7("Ball 7", Vector) = (0,0,0,0)

// _Ball8("Ball 8", Vector) = (0,0,0,0)
// _Ball9("Ball 9", Vector) = (0,0,0,0)
// _BallA("Ball 10", Vector) = (0,0,0,0)
// _BallB("Ball 11", Vector) = (0,0,0,0)
// _BallC("Ball 12", Vector) = (0,0,0,0)
// _BallD("Ball 13", Vector) = (0,0,0,0)
// _BallE("Ball 14", Vector) = (0,0,0,0)
// _BallF("Ball 15", Vector) = (0,0,0,0)
// @endblock
}

SubShader
{

Tags 
{ 
    "RenderType" = "Opaque"
    "Queue" = "Geometry"
    "IgnoreProjector" = "True" 
    "RenderPipeline" = "UniversalPipeline" 
    "DisableBatching" = "True"
}

LOD 300

HLSLINCLUDE

#define OBJECT_SHAPE_CUBE

#define CHECK_IF_INSIDE_OBJECT

#define DISTANCE_FUNCTION DistanceFunction
#define POST_EFFECT PostEffect

#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
#include "Assets/uRaymarching/Shaders/Include/UniversalRP/Primitives.hlsl"
#include "Assets/uRaymarching/Shaders/Include/UniversalRP/Math.hlsl"
#include "Assets/uRaymarching/Shaders/Include/UniversalRP/Structs.hlsl"
#include "Assets/uRaymarching/Shaders/Include/UniversalRP/Utils.hlsl"

// @block DistanceFunction
half _Smooth;
half _Radius;

half4 _Balls[16];
int _BallsCount;

inline float DistanceFunction(float3 pos)
{
    float r = Sphere(pos + _Balls[0].xyz, _Balls[0].w);

    for(uint i = 1; i < _BallsCount; i++) {
        r = SmoothMin(r, Sphere(pos + _Balls[i].xyz, _Balls[i].w), _Smooth);
    }

    return r;
}
// @endblock

#define PostEffectOutput SurfaceData

// @block PostEffect
inline void PostEffect(RaymarchInfo ray, inout PostEffectOutput o)
{
    float ao = 1.0 - pow(1.0 * ray.loop / ray.maxLoop, 2);
    o.occlusion = ao;
}
// @endblock

ENDHLSL

Pass
{
    Name "ForwardLit"
    Tags { "LightMode" = "UniversalForward" }

    Blend [_BlendSrc] [_BlendDst]
    ZWrite [_ZWrite]
    Cull [_Cull]

    HLSLPROGRAM

    #pragma shader_feature _NORMALMAP
    #pragma shader_feature _ALPHATEST_ON
    #pragma shader_feature _ALPHAPREMULTIPLY_ON
    #pragma shader_feature _EMISSION
    #pragma shader_feature _METALLICSPECGLOSSMAP
    #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
    #pragma shader_feature _OCCLUSIONMAP
    #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
    #pragma shader_feature _ENVIRONMENTREFLECTIONS_OFF
    #pragma shader_feature _SPECULAR_SETUP
    #pragma shader_feature _RECEIVE_SHADOWS_OFF

    #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
    #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
    #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
    #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
    #pragma multi_compile _ _SHADOWS_SOFT
    #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

    #pragma multi_compile _ DIRLIGHTMAP_COMBINED
    #pragma multi_compile _ LIGHTMAP_ON
    #pragma multi_compile_fog
    #pragma multi_compile_instancing

    #pragma prefer_hlslcc gles
    #pragma exclude_renderers d3d11_9x
    #pragma target 2.0

    #pragma vertex Vert
    #pragma fragment Frag
    #include "Assets/uRaymarching/Shaders/Include/UniversalRP/ForwardLit.hlsl"

    ENDHLSL
}

Pass
{
    Name "DepthOnly"
    Tags { "LightMode" = "DepthOnly" }

    ZWrite On
    ColorMask 0
    Cull [_Cull]

    HLSLPROGRAM

    #pragma shader_feature _ALPHATEST_ON
    #pragma multi_compile_instancing

    #pragma prefer_hlslcc gles
    #pragma exclude_renderers d3d11_9x
    #pragma target 2.0

    #pragma vertex Vert
    #pragma fragment Frag
    #include "Assets/uRaymarching/Shaders/Include/UniversalRP/DepthOnly.hlsl"

    ENDHLSL
}

}

FallBack "Hidden/Universal Render Pipeline/FallbackError"
CustomEditor "uShaderTemplate.MaterialEditor"

}