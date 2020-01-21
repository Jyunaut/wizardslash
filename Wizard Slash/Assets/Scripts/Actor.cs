using UnityEngine;

// Base properties and functions for an actor in the game
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Actor : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    
    [Range(0f, 15f)] public float moveSpeed;
    [Range(0f, 20f)] public float jumpForce;
    [Range(0f, 0.5f)] public float runAcceleration;

    public string currentAction;

    public bool onGround;
    public bool facingRight;
    public bool canMove;
    public bool canAttack;
    public bool isAttacking;

    private Vector2 velocity;
    private float knockbackTime;

    public Actor()
    {
        currentAction = "Neutral Air";

        onGround = false;
        facingRight = true;
        canMove = true;
        canAttack = true;
        isAttacking = false;
    }

    // Calculate default non-flying unit velocity
    public virtual void Run(float direction)
    {
        float runSpeed = direction * moveSpeed;
        Vector2 targetVelocity = new Vector2(runSpeed, rigidbody2d.velocity.y);
        rigidbody2d.velocity = Vector2.SmoothDamp(rigidbody2d.velocity, targetVelocity, ref velocity, runAcceleration);
    }

    public virtual void Jump()
    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
    }

    // Check which way player is facing depending on player input and flip
    // if player wants to turn around
    public void CheckDirection(float direction)
    {
        if      (direction > 0)
            facingRight = true;
        else if (direction < 0)
            facingRight = false;
            
        Vector2 currentScale = transform.localScale;
        currentScale.x = (facingRight == true) ? 1 : -1;
        transform.localScale = currentScale;
    }
}