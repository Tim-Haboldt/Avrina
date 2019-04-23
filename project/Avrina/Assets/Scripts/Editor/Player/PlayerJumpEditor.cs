using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerJump))]
public class PlayerJumpEditor : Editor
{
    /**
     * Draws the line which defines when the player is on ground
     */
    private void OnSceneGUI()
    {
        var playerJump = (PlayerJump)target;

        Vector2 playerPos = playerJump.transform.position;
        Vector2 relativeGroundPos = new Vector2(playerPos.x + playerJump.groundPosition.x, playerPos.y + playerJump.groundPosition.y);

        Vector2 leftPoint = new Vector2(relativeGroundPos.x - playerJump.groundWidth / 2, relativeGroundPos.y);
        Vector2 rightPoint = new Vector2(relativeGroundPos.x + playerJump.groundWidth / 2, relativeGroundPos.y);

        Handles.color = Color.red;
        Handles.DrawLine(leftPoint, rightPoint);
    }
}
