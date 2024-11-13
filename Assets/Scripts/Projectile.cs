using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

/// \class Projectile
/// \brief Handles the behavior of projectiles fired by the player or invaders, including movement and collision detection.
public class Projectile : MonoBehaviour
{
    /// \brief The direction the projectile will move in (default is upward).
    public Vector3 direction = Vector3.up;

    /// \brief The speed of the projectile.
    public float speed = 5f;

    /// \brief Reference to the Rigidbody2D component for physics-based movement.
    private Rigidbody2D rb; 

    /// \brief Reference to the BoxCollider2D component for collision detection.
    private BoxCollider2D boxCollider;

    /// \brief Called once when the projectile is instantiated. Initializes components and sets velocity.
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // Set the sorting layer of the projectile's sprite to ensure visibility.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "4Player";
        //this.gameObject.layer = layer;

        // Set the projectile's velocity to move in the assigned direction at the assigned speed.
        rb.velocity = direction * speed;
    }

    /// \brief Sets the direction of the projectile based on the position of a target.
    /// \param target The Transform of the target (usually the player or an invader).
    public void SetDirection(Transform target)
    {
        // If there is a target, set the direction towards the target's position.
        if (target)
        {
            direction = (target.position - transform.position).normalized;
            
            // Calculate the angle for Z rotation
            float offset = 90f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }
        // If no target is specified, maintain the default upward direction.
        else
        {
            direction = transform.up;
        }
    }

    /// \brief Sets the speed of the projectile.
    /// \param speed The speed at which the projectile will travel.
    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    /// \brief Called when the projectile collides with another object.
    /// \param other The Collider2D of the object the projectile collided with.
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    /// \brief Called when the projectile remains in contact with another object.
    /// \param other The Collider2D of the object the projectile is in contact with.
    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    /// \brief Handles the logic for when the projectile collides with specific layers (Boundary, Invader, or Player).
    /// \param other The Collider2D of the object that was hit.
    private void CheckCollision(Collider2D other)
    {
        // Destroy the projectile if it collides with the Boundary, Invader, or Player layers.
        if (other.gameObject.layer == LayerMask.NameToLayer("Boundary") ||
           other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
           other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            /// \brief Destroy the projectile upon collision.
            /// \param gameObject It destroys the projectile gameobject
            Destroy(gameObject); 
        }
    }
}
