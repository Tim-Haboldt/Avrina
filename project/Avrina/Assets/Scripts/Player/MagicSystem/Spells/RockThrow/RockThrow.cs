using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class RockThrow : Spell
{
    /// <summary>
    ///  Will be one single rock of the multiple rock throws
    /// </summary>
    [SerializeField] private Rock singleRock = null;
    /// <summary>
    ///  How far will the spell be spawned from the player
    /// </summary>
    [SerializeField] private float spellOffset = 1f;
    /// <summary>
    ///  How many rocks will be inside the spell
    /// </summary>
    [SerializeField] private int rockAmount = 5;
    /// <summary>
    ///  How far can the rocks fly in a different direction
    /// </summary>
    [SerializeField] private float maxDirectionSpread = 1f;
    /// <summary>
    ///  How far the middle of the spell can the rocks be spawned befor they start flying in the direction
    /// </summary>
    [SerializeField] private float maxPositionSpread = 1f;

    /// <summary>
    ///  Stores the start position of every single rock instance
    /// </summary>
    private List<Vector2> positions;
    /// <summary>
    ///  Stores the direcitons every of the rocks will be heading after being spawned
    /// </summary>
    private List<Vector2> directions;


    /// <summary>
    ///  Sets the start position of the spell
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="castDirection"></param>
    /// <returns></returns>
    protected override Vector2 CalculateStartPosition(Vector2 playerPosition, Vector2 castDirection)
    {
        this.positions = new List<Vector2>();
        this.directions = new List<Vector2>();

        var rockThrowPosition = this.playerPosition + this.castDirection * this.spellOffset;

        for (int i = 0; i < this.rockAmount; i++)
        {
            var direction = Random.insideUnitCircle;
            var distanceToMiddle = Random.value * this.maxPositionSpread;
            this.positions.Add(rockThrowPosition + direction * distanceToMiddle);

            var angle = Vector2.SignedAngle(Vector2.up, castDirection);
            angle -= Random.value * this.maxDirectionSpread - this.maxDirectionSpread * 0.5f;
            this.directions.Add(Quaternion.Euler(0, 0, angle) * Vector2.up);
        }

        return rockThrowPosition;
    }

    /// <summary>
    ///  
    /// </summary>
    protected override void HandleClientStart()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    ///  
    /// </summary>
    protected override void HandleServerStart()
    {
        for (int i = 0; i < this.rockAmount; i++)
        {
            var singleRockInstance = Instantiate(this.singleRock);
            singleRockInstance.playerPosition = this.positions[i];
            singleRockInstance.castDirection = this.directions[i];
            singleRockInstance.caster = this.GetComponent<NetworkIdentity>().netId;
            NetworkServer.Spawn(singleRockInstance.gameObject);
        }

        Destroy(this.gameObject);
    }

    // Spawns multiple rocks at once and controlls them all together through this script
}
