#ifndef _CUSTOM_RP_SHADOWS_
#define _CUSTOM_RP_SHADOWS_

#define MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT 4
#define MAX_CASCADE_COUNT 4

TEXTURE2D_SHADOW(_DirectionalShadowAtlas);
#define SHADOW_SAMPLER sampler_linear_clamp_compare
SAMPLER_CMP(SHADOW_SAMPLER);

CBUFFER_START(_CustomShadows)
    int _CascadeCount;
    float4 _CascadeCullingSpheres[MAX_CASCADE_COUNT];
    float4x4 _DirectionalShadowMatrices[MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT * MAX_CASCADE_COUNT];
    float4 _ShadowDistanceFade;
CBUFFER_END

struct ShadowData
{
    int cascadeIndex;
    float strength;
};

struct DirectionalShadowData
{
    float strength;
    int tileIndex;
};


float FadeShadowStrength(float distance, float scale, float fade)
{
    return saturate((1.0f - distance * scale) * fade);
}

ShadowData GetShadowData(Surface surface)
{
    ShadowData data;
    data.strength = FadeShadowStrength(surface.depth, _ShadowDistanceFade.x, _ShadowDistanceFade.y);
    
    int i;
    for (i = 0; i < _CascadeCount; ++i)
    {
        float4 sphere = _CascadeCullingSpheres[i];
        const float distanceSqr = DistanceSquared(surface.position, sphere.xyz);
        if (distanceSqr < sphere.w)
            break;
    }

    if (i == _CascadeCount)
        data.strength = 0.0f;
    
    data.cascadeIndex = i;
    
    return data;
}

float SampleDirectionalShadowAtlas(float3 positionSTS)
{
    return SAMPLE_TEXTURE2D_SHADOW(_DirectionalShadowAtlas, SHADOW_SAMPLER, positionSTS);
}

float GetDirectionalShadowAttenuation(DirectionalShadowData data, Surface surface)
{
    if (data.strength <= 0.0f)
    {
        return 1.0f;
    }
    
    const float3 positionSTS = mul(
        _DirectionalShadowMatrices[data.tileIndex],
        float4(surface.position, 1.0f)
    ).xyz;

    const float shadow = SampleDirectionalShadowAtlas(positionSTS);
    return lerp(1.0f, shadow, data.strength);
}

#endif
