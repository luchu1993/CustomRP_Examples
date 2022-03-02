using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [MenuItem("Custom RP/Example/GenerateObjects")]
    private static void GenerateObjects()
    {
        var objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        objectManager.Generate();
    }
    
    [MenuItem("Custom RP/Example/ClearObjects")]
    private static void ClearObjects()
    {
        var objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        objectManager.Clear();
    }
    
    private void Generate()
    {
        for (int i = 0; i < InstanceCount; ++i)
        {
            float positionX = UnityEngine.Random.Range(-AreaSize, AreaSize);
            float positionY = UnityEngine.Random.Range(-AreaSize, AreaSize);
            
            Transform instance = GameObject.Instantiate(ObjectPrefab, 
                new Vector3(positionX, positionY, 0.0f), Quaternion.identity);
            PerObjectMaterialProperties materialProperties = instance.gameObject.AddComponent<PerObjectMaterialProperties>();

            materialProperties.baseColor = new Color(
                UnityEngine.Random.Range(0.0f, 1.0f),
                UnityEngine.Random.Range(0.0f, 1.0f),
                UnityEngine.Random.Range(0.0f, 1.0f)
            );
            materialProperties.OnValidate();
            
            instance.SetParent(transform);
        }
    }

    private void Clear()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    
    #region Public Fields
    public Transform ObjectPrefab;
    public int InstanceCount;
    public float AreaSize;
    #endregion
}