using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif


public partial class CameraRender
{
#if UNITY_EDITOR
    // Unsupported shaders
    static ShaderTagId[] legacyShaderTagIds =
    {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };

    partial void DrawUnsupporedShaders()
    {
        if (errorMaterial == null)
        {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }

        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera_));
        for (int i = 1; i < legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }

        drawingSettings.overrideMaterial = errorMaterial;

        var filteringSettings = FilteringSettings.defaultValue;
        context_.DrawRenderers(cullingResults_, ref drawingSettings, ref filteringSettings);
    }

    partial void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            context_.DrawGizmos(camera_, GizmoSubset.PreImageEffects);
            context_.DrawGizmos(camera_, GizmoSubset.PostImageEffects);
        }
    }

    partial void PrepareForSceneWindow()
    {
        if (camera_.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera_);
        }
    }

    string SamplerName { get; set; }
    partial void PrepareBuffer()
    {
        buffer_.name = SamplerName = camera_.name;
    }

    static Material errorMaterial;

#else

    const String SamplerName = bufferName_;
#endif

}
