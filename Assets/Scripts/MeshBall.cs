using UnityEngine;
using System.Collections.Generic;


class MeshBall : MonoBehaviour
{
    private static int baseColorId = Shader.PropertyToID("_BaseColor");
    private static int cutoffId = Shader.PropertyToID("_Cutoff");

    void Awake()
    {
        matrices = new Matrix4x4[instanceCount];
        baseColors = new Vector4[instanceCount];
        cutoffs = new float[instanceCount];

        for (int i = 0; i < instanceCount; ++i)
        {
            matrices[i] = Matrix4x4.TRS(
                Random.insideUnitSphere * range, 
                Quaternion.identity, 
                Vector3.one * Random.Range(0.5f, 1.5f));
            baseColors[i] = new Vector4(Random.value, Random.value, Random.value, 1.0f);
            cutoffs[i] = Random.value;
        }
    }

    void Update()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
            block.SetVectorArray(baseColorId, baseColors);
            block.SetFloatArray(cutoffId, cutoffs);
        }
        
        Graphics.DrawMeshInstanced(mesh, 0,material, matrices, instanceCount, block);
    }

    #region Public Fields
    public int instanceCount = 1023;
    public float range = 10.0f;
    public Mesh mesh = default;
    public Material material = default;
    #endregion

    MaterialPropertyBlock block;
    Matrix4x4[] matrices;
    Vector4[] baseColors;
    float[] cutoffs;
}