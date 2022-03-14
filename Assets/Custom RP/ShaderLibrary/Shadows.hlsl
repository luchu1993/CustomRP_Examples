#ifndef _CUSTOM_RP_SHADOWS_
#define _CUSTOM_RP_SHADOWS_

#define MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT 4

TEXTURE2D_SHADOW(_DirectionalShadowAtlas);
#define SHADOW_SAMPLER sampler_linear_clamp_compare
SAMPLER_CMP(SHADOW_SAMPLER);

CBUFFER_START(_CustomShadows)
    float4x4 _DirectionalShadowMatrices[MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT];
CBUFFER_END

struct DirectionalShadowData
{
    float strength;
    int tileIndex;
};

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
