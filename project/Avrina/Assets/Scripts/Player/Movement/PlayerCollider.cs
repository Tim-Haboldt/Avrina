using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCollider : MonoBehaviour
{
    /// <summary>
    ///  Is the collider currently triggered
    /// </summary>
    [SerializeField] public bool isTriggered;
    /// <summary>
    ///  What is the mask the other gameobject needs to have
    /// </summary>
    [SerializeField] public LayerMask mask { private get; set; }
    /// <summary>
    ///  Stores all current colliders inside this dictionary
    /// </summary>
    public Dictionary<ushort, Collider2D> colliders { get; private set; }
    /// <summary>
    ///  What is the collider used as trigger
    /// </summary>
    private BoxCollider2D boxCollider;
    /// <summary>
    ///  Defines the next collider id
    /// </summary>
    private ushort currentId;

   
    /**
     * <summary>
     *  Sets up most of the default values.
     *  Will be called at the start of the game
     * </summary>
     */ 
    private void Start()
    {
        this.colliders = new Dictionary<ushort, Collider2D>();
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.boxCollider.isTrigger = true;
        this.isTriggered = false;
        this.currentId = 0;
    }

    /**
     * <summary>
     *  Called each time a trigger/collider enters the triggers range.
     *  Adds the collider to the triggers list.
     * </summary>
     */
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (((1 << otherCollider.gameObject.layer) & this.mask) != 0)
        {
            this.isTriggered = true;
            otherCollider.name = this.currentId.ToString();
            this.colliders.Add(this.currentId, otherCollider);
            this.currentId++;
        }
    }

    /**
     * <summary>
     *  Called each time a trigger/collider leaves the triggers range
     *  Removes the collider from the triggers list.
     * </summary>
     */
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (((1 << otherCollider.gameObject.layer) & this.mask) != 0)
        {
            var id = Convert.ToUInt16(otherCollider.name);
            this.colliders.Remove(id);

            if (this.colliders.Count == 0)
                this.isTriggered = false;
        }
    }
}
