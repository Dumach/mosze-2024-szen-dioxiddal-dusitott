using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    public Vector3 direction = Vector3.up;
    public float speed = 5f;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        // Sorting layer set to 4Player to show missiles on screen
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "4Player";

        rb.velocity = direction * speed;
    }

    public void SetDirection(Transform target)
    {
        if (target)
        {
        direction = (target.position - transform.position).normalized;
        }
        // set direction projectile direction if there is no target
        else
        {
            direction = transform.up;
        }
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    private void Update()
    {
        //transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D other)
    {
        /*Bunker bunker = other.gameObject.GetComponent<Bunker>();

        if (bunker == null || bunker.CheckCollision(boxCollider, transform.position)) {
            Destroy(gameObject);
        }*/
        if(other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
