using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCollider : MonoBehaviour
{
    [SerializeField] public bool isTriggered;

    public LayerMask mask { private get; set; }

    private BoxCollider2D boxCollider;
    private int amountOfColliders;

    private void Start()
    {
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.boxCollider.isTrigger = true;
        this.isTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & this.mask) != 0)
        {
            this.isTriggered = true;
            this.amountOfColliders++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & this.mask) != 0)
        {
            this.amountOfColliders--;

            if (this.amountOfColliders == 0)
                this.isTriggered = false;
        }
    }
}
