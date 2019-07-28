using UnityEngine;

public class ElementBase : MonoBehaviour
{
    // Stores all four Spells and their direction
    [SerializeField] public SpellBase spellLeft;
    [SerializeField] public SpellBase spellRight;
    [SerializeField] public SpellBase spellUp;
    [SerializeField] public SpellBase spellDown;
    // Every Element needs to know what their name is
    [SerializeField] public Element elementType;

    // This function will be called if a spell was casted
    public void CastSpell(Vector2 secondDir, Vector2 playerPosition, Vector2 castDirection)
    {
        if (secondDir == Vector2.up)
            this.spellUp.CastSpell(playerPosition, castDirection);
        else if (secondDir == Vector2.down)
            this.spellDown.CastSpell(playerPosition, castDirection);
        else if (secondDir == Vector2.left)
            this.spellLeft.CastSpell(playerPosition, castDirection);
        else if (secondDir == Vector2.right)
            this.spellRight.CastSpell(playerPosition, castDirection);
    }
}
