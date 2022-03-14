using UnityEngine;
using System.Collections.Generic;


class MeshBall : MonoBehaviour
{
    private static int baseColorId = Shader.PropertyToID("_BaseColor");
    private static int cutoffId = Shader.PropertyToID("_Cutoff");
    private static int metallicId = Shader.PropertyToID("_Metallic");
    private static int smoothnessId = Shader.PropertyToID("_Smoothness");

    void Awake()
    {
        matrices = new Matrix4x4[instanceCount];
        baseColors = new Vector4[instanceCount];
        cutoffs = new float[instanceCount];
        metallic = new float[instanceCount];
        smoothness = new float[instanceCount];

        for (int i = 0; i < instanceCount; ++i)
        {
            matrices[i] = Matrix4x4.TRS(
                Random.insideUnitSphere * range, 
                Quaternion.identity, 
                Vector3.one * Random.Range(0.5f, 1.5f));
            baseColors[i] = new Vector4(Random.value, Random.value, Random.value, 1.0f);
            cutoffs[i] = Random.value;
            
            metallic[i] = Random.value < 0.25f ? 1f : 0f;
            smoothness[i] = Random.Range(0.05f, 0.95f);
        }
    }

    void Update()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
            block.SetVectorArray(baseColorId, baseColors);
            block.SetFloatArray(cutoffId, cutoffs);
            block.SetFloatArray(metallicId, metallic);
            block.SetFloatArray(smoothnessId, smoothness);
        }
        
        Graphics.DrawMeshInstanced(mesh, 0,material, matrices, instanceCount, block);
    }

    #region Public Fields
    public int instanceCount = 1023;
    public float range = 10.0f;
    public Mesh mesh = default;
    public Material material = default;
    #endregion

    #region Private Fields
    private MaterialPropertyBlock block;
    private Matrix4x4[] matrices;
    private Vector4[] baseColors;
    private float[] cutoffs;
    private float[] metallic;
    private float[] smoothness;
    #endregion

}