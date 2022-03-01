#ifndef _CUSTOM_RP_UNLIT_PASS_
#define _CUSTOM_RP_UNLIT_PASS_

#include "../ShaderLibrary/Common.hlsl"

float4 UnlitPassVertex(float3 position : POSITION) : SV_Position
{
    const float3 positionWS = TransformObjectToWorld(position);
    return TransformWorldToHClip(positionWS);
}

float4 UnlitPassFragment() : SV_Target
{
    return _BaseColor;
}

#endif
