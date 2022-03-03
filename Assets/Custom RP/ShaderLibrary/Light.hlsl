#ifndef _CUSTOM_RP_LIGHT_
#define _CUSTOM_RP_LIGHT_

struct Light
{
    float3 color;
    float3 direction;
};

int GetDirectionalLightCount()
{
    return _DirectionalLightCount;    
}

Light GetDirectionalLight(int index)
{
    Light light;
    light.color = _DirectionalLightColors[index].rgb;
    light.direction = _DirectionalLightDirections[index].xyz;
    return light;
}

#endif
