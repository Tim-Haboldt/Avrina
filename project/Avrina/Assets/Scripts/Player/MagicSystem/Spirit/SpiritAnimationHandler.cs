using UnityEngine;
using Mirror;

[RequireComponent(typeof(SpriteRenderer))]
public class SpiritAnimationHandler : NetworkBehaviour
{
    [Header("Sprites")]
    /// <summary>
    ///  Sprite of the default none element selected state
    /// </summary>
    [SerializeField] private Sprite noElementSprite;
    /// <summary>
    ///  Sprite of the fire state
    /// </summary>
    [SerializeField] private Sprite fireSprite;
    /// <summary>
    ///  Sprite of the water state
    /// </summary>
    [SerializeField] private Sprite waterSprite;
    /// <summary>
    ///  Sprite of the air state
    /// </summary>
    [SerializeField] private Sprite airSprite;
    /// <summary>
    ///  Sprite of the earth state
    /// </summary>
    [SerializeField] private Sprite earthSprite;

    /// <summary>
    ///  Used to update the sprite of the spirit corresponding to its state
    /// </summary>
    private SpriteRenderer spriteRenderer;


    /// <summary>
    ///  Get the sprite renderer
    /// </summary>
    void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    ///  Updates the appearance of the spirit
    /// </summary>
    public void UpdateSpiritAppearance(SpiritState spiritState)
    {
        switch (spiritState)
        {
            case SpiritState.None:
                this.spriteRenderer.sprite = this.noElementSprite;
                break;
            case SpiritState.Air:
                this.spriteRenderer.sprite = this.airSprite;
                break;
            case SpiritState.Fire:
                this.spriteRenderer.sprite = this.fireSprite;
                break;
            case SpiritState.Water:
                this.spriteRenderer.sprite = this.waterSprite;
                break;
            case SpiritState.Earth:
                this.spriteRenderer.sprite = this.earthSprite;
                break;
        }
    }
}
