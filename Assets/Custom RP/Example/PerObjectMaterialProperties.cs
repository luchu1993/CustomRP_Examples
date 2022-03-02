using System;
using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    public Color baseColor = Color.white;
    [Range(0, 1)]
    public float cutoff = 0.5f;
    
    public void OnValidate()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }
        
        block.SetColor(baseColorId, baseColor);
        block.SetFloat(cutoffId, cutoff);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }

    private static MaterialPropertyBlock block;
    private static int baseColorId = Shader.PropertyToID("_BaseColor");
    private static int cutoffId = Shader.PropertyToID("_Cutoff");

}
