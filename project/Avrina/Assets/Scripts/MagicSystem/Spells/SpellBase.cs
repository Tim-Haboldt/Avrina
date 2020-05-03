using UnityEngine;

[System.Serializable]
public abstract class SpellBase : MonoBehaviour
{
    public abstract void CastSpell(Vector2 playerPosition, Vector2 castDirection);
}
