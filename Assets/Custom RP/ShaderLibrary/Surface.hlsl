#ifndef _CUSTOM_RP_SURFACE_
#define _CUSTOM_RP_SURFACE_

struct Surface
{
    float3 position;
    float3 normal;
    float3 viewDirection;
    float depth;
    float3 color;
    float alpha;
    float metallic;
    float smoothness;
    float dither;
};

#endif
