using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private PlayerInput inputs;
    private Animator animator;

    // 0 => up
    // 1 => right
    // 2 => down
    // 3 => left
    private float lastDirection;

    void Start()
    {
        this.inputs = GetComponent<PlayerInput>();
        this.animator = GetComponent<Animator>();

        this.lastDirection = 2;
    }

    private void Update()
    {
        float movX = this.inputs.movementInput;
        float movY = 0;

        this.animator.SetFloat("MovementX", movX);
        this.animator.SetFloat("MovementY", movY);

        float speedX = Mathf.Abs(movX);
        float speedY = Mathf.Abs(movY);

        this.animator.SetFloat("TotalSpeed", speedX + speedY);

        // calculate last direction
        if (speedX > 0 || speedY > 0)
        {
            this.setDirection((speedX > speedY) ? (movX, 1) : (movY, 0));
        }

        this.animator.SetFloat("LastDirection", this.lastDirection);
    }

    private void setDirection((float speed, int number) direction)
    {
        if (direction.speed > 0)
        {
            this.lastDirection = direction.number;
        }
        else
        {
            this.lastDirection = direction.number + 2;
        }
    }
}
