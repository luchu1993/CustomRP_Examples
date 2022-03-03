#ifndef _CUSTOM_RP_LIGHTING_
#define _CUSTOM_RP_LIGHTING_

float3 GetLighting(Surface surface, Light light)
{
    return saturate(dot(surface.normal, light.direction)) * light.color;
}


float3 GetLighting(Surface surface)
{
    float3 color = 0.0f;
    for (int i = 0; i < GetDirectionalLightCount(); ++i)
    {
        color += GetLighting(surface, GetDirectionalLight(i));
    }
  
    return color * surface.color;
}

#endif
