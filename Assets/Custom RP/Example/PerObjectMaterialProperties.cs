using System;
using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    private static int baseColorId = Shader.PropertyToID("_BaseColor");
    
    public Color baseColor = Color.white;

    static MaterialPropertyBlock block;

    public void OnValidate()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }
        
        block.SetColor(baseColorId, baseColor);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
}
