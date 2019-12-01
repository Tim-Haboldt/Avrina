[System.Serializable]
public enum PlayerState
{
    Unchanged,      // Will be returned if the state did not change during an update
    Disabled,       // Inputs have no effect (If the Player is in the Menu etc.)
    Uncontrolled,   // Player is not on Ground and only special inputs have an effect
    OnGround,       // Player stands on ground and has two jumps left
    InAir,          // Player is in the air and has his second jump left
    Jumping,        // Player jumps from the ground
    AirJumping,     // Player jumps in the air
    WallJumping,    // Player is in the wall jump animation
    WallSliding,    // Player is sliding the wall (slower as free fall)
    Immobile        // Player has used every single jump and is in the air
}
