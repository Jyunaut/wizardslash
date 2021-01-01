using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Actor : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    
    [Range(0f,15f)] public float moveSpeed;
    [Range(0f,20f)] public float jumpForce;
    [Range(0f,0.5f)] public float runAcceleration;
    [Range(0f,1f)] public float knockbackResist;

    public Vector2 velocity;

    // Calculate default non-flying unit velocity
    public virtual void Run(float direction)
    {
        float runSpeed = direction * moveSpeed;
        Vector2 targetVelocity = new Vector2(runSpeed, rigidbody2d.velocity.y);
        rigidbody2d.velocity = Vector2.SmoothDamp(rigidbody2d.velocity, targetVelocity, ref velocity, runAcceleration);
    }
}