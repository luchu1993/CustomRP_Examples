using UnityEngine;
using UnityEngine.Rendering;


public class CustomRenderPipeline : RenderPipeline
{
    public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
    {
        useDynamicBathcing_ = useDynamicBatching;
        useGPUInstancing_ = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
    }
    
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        for (int i = 0; i < cameras.Length; ++i)
        {
            render_.Render(context, cameras[i], useDynamicBathcing_, useGPUInstancing_);
        }
    }

    private CameraRender render_ = new CameraRender();
    private bool useDynamicBathcing_;
    private bool useGPUInstancing_;
}
