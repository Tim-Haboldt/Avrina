using UnityEngine;

public class MagicSystemEventHandler : MonoBehaviour
{
    // Stores all four Elements and their direction
    [SerializeField] public ElementBase elementLeft;
    [SerializeField] public ElementBase elementRight;
    [SerializeField] public ElementBase elementUp;
    [SerializeField] public ElementBase elementDown;

    public void castSpell(Vector2 firstDir, Vector2 secondDir, Vector2 playerPosition, Vector2 castDirection)
    {
        if (firstDir == Vector2.up)
            this.elementUp.CastSpell(secondDir, playerPosition, castDirection);
        else if (firstDir == Vector2.down)
            this.elementDown.CastSpell(secondDir, playerPosition, castDirection);
        else if (firstDir == Vector2.left)
            this.elementLeft.CastSpell(secondDir, playerPosition, castDirection);
        else if (firstDir == Vector2.right)
            this.elementRight.CastSpell(secondDir, playerPosition, castDirection);
    }

    public void hideMagicMenu()
    {
        this.elementLeft.gameObject.SetActive(false);
        this.elementRight.gameObject.SetActive(false);
        this.elementUp.gameObject.SetActive(false);
        this.elementDown.gameObject.SetActive(false);
    }

    public void showMagicMenu()
    {
        this.elementLeft.gameObject.SetActive(true);
        this.elementRight.gameObject.SetActive(true);
        this.elementUp.gameObject.SetActive(true);
        this.elementDown.gameObject.SetActive(true);
    }
}
