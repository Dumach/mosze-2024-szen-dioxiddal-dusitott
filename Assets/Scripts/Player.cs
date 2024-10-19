using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [Header("Player")]
    public float speed = 5f;
    private Color normalColor;
    private SpriteRenderer spriteRenderer;

    [Header("Guns")]
    public List<Gun> guns = new List<Gun>();

    // Shield or respawn protection
    private float unkillableTimer = 0f;

    private void Update()
    {
        Vector3 position = transform.position;

        // Update the position of the player based on the input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            position.x -= speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            position.x += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.UpArrow)) {
            position.y += speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            position.y -= speed * Time.deltaTime;
        }

        // Clamp the position of the character so they do not go out of bounds
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);

        // Set the new position
        transform.position = position;

        if (Input.GetKey(KeyCode.Space))
        {
            foreach (Gun gun in guns)
            {
                gun.Shoot();
            }
        }

        if (unkillableTimer > 0)
        {
            unkillableTimer -= Time.deltaTime;
        }
        else
        {
            spriteRenderer.color = normalColor;
        }
    }

    public void beUnkillable(float toSeconds)
    {
        this.unkillableTimer = toSeconds;
        spriteRenderer.color = Color.blue;
    }

    private void Start()
    {
        ColorUtility.TryParseHtmlString("#7EE62C", out normalColor);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (unkillableTimer > 0)
        {
            return;
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            GameManager.Instance.OnPlayerKilled(this);
        }
    }

}
