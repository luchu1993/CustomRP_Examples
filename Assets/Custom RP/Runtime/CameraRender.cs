using UnityEngine;
using UnityEngine.Rendering;


public partial class CameraRender 
{
    public void Render(ScriptableRenderContext context, Camera camera, 
        bool useDynamicBatching, bool useGPUInstancing, ShadowSettings shadowSettings)
    {
        this.context = context;
        this.camera = camera;

        PrepareBuffer();
        PrepareForSceneWindow();

        if (!Culling(shadowSettings.maxDistance))
            return;

        buffer_.BeginSample(SamplerName);
        ExecuteBuffer();
        lighting.Setup(context, cullingResults, shadowSettings);
        buffer_.EndSample(SamplerName);
        
        Setup();
        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
        DrawUnsupporedShaders();
        DrawGizmos();
        
        lighting.Cleanup();
        Submit();
    }

    bool Culling(float maxShadowDistance)
    {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            p.shadowDistance = Mathf.Min(maxShadowDistance, camera.farClipPlane);
            cullingResults = context.Cull(ref p);
            return true;
        }

        return false;
    }

    void Setup()
    {
        context.SetupCameraProperties(camera);

        // Clear Flags
        CameraClearFlags flags = camera.clearFlags;
        buffer_.ClearRenderTarget(
            flags <= CameraClearFlags.Depth,
            flags == CameraClearFlags.Color,
            flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear
        );
        
        buffer_.BeginSample(SamplerName);
        ExecuteBuffer();
    }

    void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        SortingSettings sortingSettings = new SortingSettings(camera);
        sortingSettings.criteria = SortingCriteria.CommonOpaque;
        
        DrawingSettings drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        drawingSettings.enableDynamicBatching = useDynamicBatching;
        drawingSettings.enableInstancing = useGPUInstancing;
        drawingSettings.SetShaderPassName(1, litShaderTagId);
        
        // Draw opaque
        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        // Draw skybox
        context.DrawSkybox(camera);

        // Draw transparent
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

    }

    void Submit()
    {
        buffer_.EndSample(SamplerName);
        
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer_);
        buffer_.Clear();
    }

    #region Editor Interface
    partial void PrepareBuffer();
    partial void PrepareForSceneWindow();
    partial void DrawGizmos();
    partial void DrawUnsupporedShaders();
    #endregion

    ScriptableRenderContext context;
    Camera camera;

    const string bufferName_ = "Render Camera";
    CommandBuffer buffer_ = new CommandBuffer() { name = bufferName_ };

    CullingResults cullingResults;
    private Lighting lighting = new Lighting();
    
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
    private static ShaderTagId litShaderTagId = new ShaderTagId("CustomLit");

}
