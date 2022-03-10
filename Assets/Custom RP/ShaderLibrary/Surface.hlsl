#ifndef _CUSTOM_RP_SURFACE_
#define _CUSTOM_RP_SURFACE_

struct Surface
{
    float3 normal;
    float3 viewDirection;
    float3 color;
    float alpha;
    float metallic;
    float smoothness;
};

#endif
