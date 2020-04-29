using UnityEditor;

/// <summary>
///  Makes an asset out of GroundMaterial
/// </summary>
public class GroundMaterialAsset
{
    [MenuItem("Assets/Create/GroundMaterial")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<GroundMaterial>();
    }
}
