using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
    [SerializeField] 
    bool useDynamicBatching = true;
    
    [SerializeField] 
    bool useGPUInstancing = true;
    
    [SerializeField] 
    bool useSRPBatcher = true;

    [SerializeField] 
    ShadowSettings shadows = default;
    
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(
            useDynamicBatching, 
            useGPUInstancing, 
            useSRPBatcher,
            shadows
        );
    }
}
