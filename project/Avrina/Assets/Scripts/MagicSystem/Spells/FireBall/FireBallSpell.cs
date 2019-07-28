using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(FireBall))]
[RequireComponent(typeof(BoxCollider2D))]
public class FireBallSpell : SpellBase
{
    public override void CastSpell(Vector2 playerPosition, Vector2 castDirection)
    {
        // Create the fireball gameobject
        var fireBall = new GameObject();

        // Add the spriteRenderer and the animation
        var renderer = fireBall.AddComponent<SpriteRenderer>() as SpriteRenderer;
        var animator = fireBall.AddComponent<SpriteAnimator>() as SpriteAnimator;
        // Update some basic values
        renderer.sortingLayerName = this.spellLayer.name;
        renderer.sortingLayerID = this.spellLayer.id;
        animator.animations = new SpriteAnimation[1];
        animator.animations[0] = this.spellAnimation;
        animator.Play(this.spellAnimation.name);

        // Add the collider (Copy it from the current gameobject)
        var collider = fireBall.AddComponent<BoxCollider2D>() as BoxCollider2D;
        this.CopyComponent(collider, this.GetComponent<BoxCollider2D>());

        // Add and copy the fireBallObject (Handels movement damage etc for the fireball)
        var fireBallScript = fireBall.AddComponent<FireBall>() as FireBall;
        this.CopyComponent(fireBallScript, this.GetComponent<FireBall>());
    }
}
