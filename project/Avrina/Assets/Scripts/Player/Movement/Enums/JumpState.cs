public enum JumpState
{
    OnGround,       // Player stands on ground and has two jumps left
    InAir,          // Player is in the air and has his second jump left
    WallJumping,    // Player is in the wall jump animation
    WallSliding,    // Player is sliding the wall (slower as free fall)
    Immobile        // Player has used every single jump
}
