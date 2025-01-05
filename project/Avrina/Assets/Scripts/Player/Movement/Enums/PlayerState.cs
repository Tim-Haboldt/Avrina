[System.Serializable]
public enum PlayerState
{
    Disabled,       // Inputs have no effect (If the Player is in the Menu etc.)
    Uncontrolled,   // Player cannot move at all until the status is over
    OnGround,       // Player stands on ground and has two jumps left
    InAir,          // Player is in the air and has his second jump left
    Jumping,        // Player jumps from the ground
    AirJumping,     // Player jumps in the air
    WallJumping,    // Player is in the wall jump animation
    WallSliding,    // Player is sliding the wall (slower as free fall)
    Immobile,       // Player has used every single jump and is in the air
    Frozen,         // Player cannot move at all until the frozen state is over
}
