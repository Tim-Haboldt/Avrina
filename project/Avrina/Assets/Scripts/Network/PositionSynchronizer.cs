using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PositionSynchronizer : NetworkBehaviour
{
    /// <summary>
    ///  How often will the values be synced via the network
    /// </summary>
    [Header("Sync Options")]
    [SerializeField] private float networkSyncInterval = 0.1f;
    private float invertedNetworkSyncInterval;
    /// <summary>
    ///  What is the max lerp distance before the player is just teleported. Should only happen if the local game is out of sync
    /// </summary>
    [SerializeField] private float maxPositionDifferenzBeforeTeleport = 10;
    /// <summary>
    ///  When was the last time the position was synced with the network
    /// </summary>
    private float lastSyncTimeStemp = 0;
    /// <summary>
    ///  This will be used to smoothly move the player to the position of the original player.
    ///  Only for the unauthorized instances of the object
    /// </summary>
    private Vector2 lerpDistance;
    /// <summary>
    ///  What was the last position of the player. Used only for the authorized object to store the last position
    /// </summary>
    private Vector2 lastPos;
    /// <summary>
    ///  What is the current position of the player at the time of the sync. Will only the used by the authorized object
    /// </summary>
    private Vector2 currentPos;
    /// <summary>
    ///  What will be the next position. Only for the unauthorized instances of the object
    /// </summary>
    [SyncVar(hook = nameof(HandlePositionWasSynced))]
    private Vector2 nextPos;


    /// <summary>
    ///  Used to calcualte the inverted netowork sync interval. Will grately decrease the calculation efford of the programm in each update loop
    /// </summary>
    public override void OnStartClient()
    {
        this.invertedNetworkSyncInterval = 1 / this.networkSyncInterval;
    }

    /// <summary>
    ///  Will sync the transform values via the internet
    /// </summary>
    private void Update()
    {
        if (!this.hasAuthority)
        {
            return;
        }

        var passedTime = Time.time - this.lastSyncTimeStemp;
        if (passedTime < networkSyncInterval)
        {
            return;
        }

        this.lastSyncTimeStemp = Time.time - (passedTime - networkSyncInterval);

        if (this.nextPos == null)
        {
            this.nextPos = this.transform.position;
        }

        this.lastPos = this.currentPos;
        this.currentPos = this.transform.position;

        this.CmdSendNextPosition(this.currentPos);
    }

    /// <summary>
    ///  Used to smoothly update the position of the player
    /// </summary>
    private void FixedUpdate()
    {
        if (this.hasAuthority || this.lerpDistance == null)
        {
            return;
        }

        var distance = Vector2.Lerp(Vector2.zero, this.lerpDistance, Time.deltaTime * this.invertedNetworkSyncInterval);
        this.transform.position += new Vector3(distance.x, distance.y, 0);
    }

    /// <summary>
    ///  Sets the next position of the player for the server and all clients
    /// </summary>
    /// <param name="nextPos"></param>
    [Command]
    private void CmdSendNextPosition(Vector2 nextPos)
    {
        this.nextPos = nextPos;
    }

    /// <summary>
    ///  Will be called each time the position value is synced by the server
    /// </summary>
    /// <param name="lastPosition"></param>
    /// <param name="nextPosition"></param>
    private void HandlePositionWasSynced(Vector2 lastPosition, Vector2 nextPosition)
    {
        if (this.hasAuthority)
        {
            return;
        }
        
        this.nextPos = nextPosition;
        if (Vector2.Distance(this.transform.position, this.nextPos) >= this.maxPositionDifferenzBeforeTeleport)
        {
            this.transform.position = this.nextPos;
        }

        var position = this.transform.position;
        this.lerpDistance = this.nextPos - new Vector2(position.x, position.y);
    }
}
