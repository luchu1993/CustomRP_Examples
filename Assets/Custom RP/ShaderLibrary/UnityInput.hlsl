#ifndef _CUSTOM_RP_UNITY_INPUT_
#define _CUSTOM_RP_UNITY_INPUT_

CBUFFER_START(UnityPerMaterial)
    float4 _BaseColor;
CBUFFER_END


CBUFFER_START(UnityPerDraw)
    float4x4 unity_ObjectToWorld;
    float4x4 unity_WorldToObject;
    float4 unity_LODFade;
    float4x4 glstate_matrix_projection;
CBUFFER_END

float4x4 unity_MatrixV;
float4x4 unity_MatrixVP;

real4 unity_WorldTransformParams;

#endif
