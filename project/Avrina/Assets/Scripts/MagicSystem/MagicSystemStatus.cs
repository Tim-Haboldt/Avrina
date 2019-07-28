using UnityEngine;

[RequireComponent(typeof(MagicSystemEventHandler))]
[RequireComponent(typeof(PlayerStatus))]
public class MagicSystemStatus : MonoBehaviour
{
    // Stores the information which keycode is related to the magic key
    [SerializeField] public KeyCode directionRight;
    [SerializeField] public KeyCode directionLeft;
    [SerializeField] public KeyCode directionUp;
    [SerializeField] public KeyCode directionDown;
    [SerializeField] public KeyCode castSpell;
    // Which was the first element pressed
    // If there was no element pressed before - the state will be None
    [HideInInspector] public Vector2 firstDirection;
    [HideInInspector] public Vector2 secondDirection;
    public bool isFirstDirectionSelected { get; private set; }
    public bool isSecondDirectionSelected { get; private set; }
    public bool isPlayerCastingSpell { get; private set; }
    // Needed for the cast direction
    private PlayerStatus playerStatus;
    private MagicSystemEventHandler handler;

    // Start is called before the first frame update
    void Start()
    {
        this.isFirstDirectionSelected = false;
        this.isSecondDirectionSelected = false;
        this.playerStatus = GetComponent<PlayerStatus>();
        this.handler = GetComponent<MagicSystemEventHandler>();
        this.handler.hideMagicMenu();
    }

    private void Update()
    {
        if (this.isPlayerCastingSpell && !(this.isFirstDirectionSelected && this.isSecondDirectionSelected))
        {
            // Chose elements
            if (Input.GetKeyDown(this.directionRight))
            {
                if (this.isFirstDirectionSelected)
                {
                    this.secondDirection = Vector2.right;
                    this.handler.hideMagicMenu();
                }
                else
                    this.firstDirection = Vector2.right;

                this.isSecondDirectionSelected = this.isFirstDirectionSelected;
                this.isFirstDirectionSelected = true;
            }
            else if (Input.GetKeyDown(this.directionLeft))
            {
                if (this.isFirstDirectionSelected)
                {
                    this.secondDirection = Vector2.left;
                    this.handler.hideMagicMenu();
                }
                else
                    this.firstDirection = Vector2.left;

                this.isSecondDirectionSelected = this.isFirstDirectionSelected;
                this.isFirstDirectionSelected = true;
            }
            else if (Input.GetKeyDown(this.directionUp))
            {
                if (this.isFirstDirectionSelected) { 
                    this.secondDirection = Vector2.up;
                    this.handler.hideMagicMenu();
                }
                else
                this.firstDirection = Vector2.up;

                this.isSecondDirectionSelected = this.isFirstDirectionSelected;
                this.isFirstDirectionSelected = true;
            }
            else if (Input.GetKeyDown(this.directionDown))
            {
                if (this.isFirstDirectionSelected)
                {
                    this.secondDirection = Vector2.down;
                    this.handler.hideMagicMenu();
                }
                else
                    this.firstDirection = Vector2.down;

                this.isSecondDirectionSelected = this.isFirstDirectionSelected;
                this.isFirstDirectionSelected = true;
            }
        }

        if (Input.GetKeyDown(this.castSpell))
        {
            if (!this.isPlayerCastingSpell)
            {
                this.isPlayerCastingSpell = true;
                this.isFirstDirectionSelected = false;
                this.isSecondDirectionSelected = false;

                this.handler.showMagicMenu();
            }
            else 
            {
                if (this.isFirstDirectionSelected && this.isSecondDirectionSelected)
                {
                    var direction = new Vector2(this.playerStatus.movementInputHorizontal, this.playerStatus.movementInputVertical);
                    this.handler.castSpell(this.firstDirection, this.secondDirection, this.transform.position, direction);
                }

                this.handler.hideMagicMenu();
                this.isFirstDirectionSelected = false;
                this.isSecondDirectionSelected = false;
                this.isPlayerCastingSpell = false;
            }
        }
    }
}
