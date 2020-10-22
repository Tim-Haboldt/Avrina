using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
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

    [Header("Lighting")]
    /// <summary>
    ///  Used to update the light color of the light
    /// </summary>
    [SerializeField] private Light2D light2D = null;
    /// <summary>
    ///  Light color of the sprite if it represents no specific element
    /// </summary>
    [SerializeField] private Color noElement;
    /// <summary>
    ///  Light color of the fire element
    /// </summary>
    [SerializeField] private Color fire;
    /// <summary>
    ///  Light color of the water element
    /// </summary>
    [SerializeField] private Color water;
    /// <summary>
    ///  Light color of the air element
    /// </summary>
    [SerializeField] private Color air;
    /// <summary>
    ///  Light color of the earth element
    /// </summary>
    [SerializeField] private Color earth;

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
                this.light2D.color = this.noElement;
                break;
            case SpiritState.Air:
                this.spriteRenderer.sprite = this.airSprite;
                this.light2D.color = this.air;
                break;
            case SpiritState.Fire:
                this.spriteRenderer.sprite = this.fireSprite;
                this.light2D.color = this.fire;
                break;
            case SpiritState.Water:
                this.spriteRenderer.sprite = this.waterSprite;
                this.light2D.color = this.water;
                break;
            case SpiritState.Earth:
                this.spriteRenderer.sprite = this.earthSprite;
                this.light2D.color = this.earth;
                break;
        }
    }
}
