using UnityEditor;
using UnityEngine;

public class WallMaterialAsset : MonoBehaviour
{
    [MenuItem("Assets/Create/WallMaterial")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<WallMaterial>();
    }
}
