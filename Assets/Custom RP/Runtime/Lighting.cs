using UnityEngine;
using Unity.Collections;
using UnityEngine.Rendering;

public class Lighting
{
    public void Setup(ScriptableRenderContext context, CullingResults cullingResults)
    {
        cullingResults_ = cullingResults;
        buffer_.BeginSample(bufferName);
        SetupLights();
        buffer_.EndSample(bufferName);
        context.ExecuteCommandBuffer(buffer_);
        buffer_.Clear();
    }

    void SetupLights()
    {
        NativeArray<VisibleLight> visibleLights = cullingResults_.visibleLights;

        int dirLightCount = 0;
        for (int i = 0; i < visibleLights.Length; ++i)
        {
            VisibleLight visibleLight = visibleLights[i];
            if (visibleLight.lightType == LightType.Directional)
            {
                SetupDirectionalLight(dirLightCount++, ref visibleLight);
            
                if (dirLightCount >= maxDirLightCount)
                    break;
            }
        }
        
        buffer_.SetGlobalInt(dirLightCountId, visibleLights.Length);
        buffer_.SetGlobalVectorArray(dirLightColorsId, dirLightColors);
        buffer_.SetGlobalVectorArray(dirLightDirectionsId, dirLightDirections);
    }

    void SetupDirectionalLight(int index, ref VisibleLight visibleLight)
    {
        dirLightColors[index] = visibleLight.finalColor;
        dirLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
    }
    
    const string bufferName = "Lighting";

    private CommandBuffer buffer_ = new CommandBuffer()
    {
        name = bufferName
    };

    CullingResults cullingResults_;
    
    const int maxDirLightCount = 4;
    static int dirLightCountId = Shader.PropertyToID("_DirectionalLightCount");
    static int dirLightColorsId = Shader.PropertyToID("_DirectionalLightColors");
    static int dirLightDirectionsId = Shader.PropertyToID("_DirectionalLightDirections");

    static Vector4[] dirLightColors = new Vector4[maxDirLightCount];
    static Vector4[] dirLightDirections = new Vector4[maxDirLightCount];
}