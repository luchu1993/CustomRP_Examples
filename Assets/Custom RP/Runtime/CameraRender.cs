using UnityEngine;
using UnityEngine.Rendering;


public partial class CameraRender 
{
    public void Render(ScriptableRenderContext context, Camera camera, bool useDynamicBatching, bool useGPUInstancing)
    {
        context_ = context;
        camera_ = camera;

        PrepareBuffer();
        PrepareForSceneWindow();

        if (!Culling())
            return;

        Setup();
        lighting_.Setup(context, cullingResults_);
        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
        DrawUnsupporedShaders();
        DrawGizmos();
        Submit();
    }

    bool Culling()
    {
        if (camera_.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            cullingResults_ = context_.Cull(ref p);
            return true;
        }

        return false;
    }

    void Setup()
    {
        context_.SetupCameraProperties(camera_);

        // Clear Flags
        CameraClearFlags flags = camera_.clearFlags;
        buffer_.ClearRenderTarget(
            flags <= CameraClearFlags.Depth,
            flags == CameraClearFlags.Color,
            flags == CameraClearFlags.Color ? camera_.backgroundColor.linear : Color.clear
        );
        
        buffer_.BeginSample(SamplerName);
        ExecuteBuffer();
    }

    void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        SortingSettings sortingSettings = new SortingSettings(camera_);
        sortingSettings.criteria = SortingCriteria.CommonOpaque;
        
        DrawingSettings drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        drawingSettings.enableDynamicBatching = useDynamicBatching;
        drawingSettings.enableInstancing = useGPUInstancing;
        drawingSettings.SetShaderPassName(1, litShaderTagId);
        
        // Draw opaque
        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        context_.DrawRenderers(cullingResults_, ref drawingSettings, ref filteringSettings);

        // Draw skybox
        context_.DrawSkybox(camera_);

        // Draw transparent
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        context_.DrawRenderers(cullingResults_, ref drawingSettings, ref filteringSettings);

    }

    void Submit()
    {
        buffer_.EndSample(SamplerName);
        
        ExecuteBuffer();
        context_.Submit();
    }

    void ExecuteBuffer()
    {
        context_.ExecuteCommandBuffer(buffer_);
        buffer_.Clear();
    }

    #region Editor Interface
    partial void PrepareBuffer();
    partial void PrepareForSceneWindow();
    partial void DrawGizmos();
    partial void DrawUnsupporedShaders();
    #endregion

    ScriptableRenderContext context_;
    Camera camera_;

    const string bufferName_ = "Render Camera";
    CommandBuffer buffer_ = new CommandBuffer() { name = bufferName_ };

    CullingResults cullingResults_;
    private Lighting lighting_ = new Lighting();
    
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
    private static ShaderTagId litShaderTagId = new ShaderTagId("CustomLit");

}
