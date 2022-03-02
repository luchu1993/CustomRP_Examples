using UnityEngine;
using System.Collections.Generic;


class MeshBall : MonoBehaviour
{
    private static int baseColorId = Shader.PropertyToID("_BaseColor");

    void Awake()
    {
        matrices = new Matrix4x4[instanceCount];
        baseColors = new Vector4[instanceCount];

        for (int i = 0; i < instanceCount; ++i)
        {
            matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 10.0f, Quaternion.identity, Vector3.one);
            baseColors[i] = new Vector4(Random.value, Random.value, Random.value, 1.0f);
        }
    }

    void Update()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
            block.SetVectorArray(baseColorId, baseColors);
        }
        
        Graphics.DrawMeshInstanced(mesh, 0,material, matrices, instanceCount, block);
    }

    #region Public Fields
    public int instanceCount = 1023;
    public Mesh mesh = default;
    public Material material = default;
    #endregion

    MaterialPropertyBlock block;
    Matrix4x4[] matrices;
    Vector4[] baseColors;
}