using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [Header("Player")]
    public float speed = 5f;
    public int health = 3;
    private Color normalColor;
    private SpriteRenderer spriteRenderer;
    private Vector3 currentPos;


    [Header("Guns")]
    public List<Gun> guns = new List<Gun>();

    // Shield or respawn protection
    private float shieldDuration = 0f;
    private float shieldTimer = 0f;

    private void Update()
    {
        // Update the position of the player based on the input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            currentPos.x -= speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            currentPos.x += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.UpArrow)) {
            currentPos.y += speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            currentPos.y -= speed * Time.deltaTime;
        }

        // Pajzs visszatoltodesi ido
        if (Input.GetKeyDown(KeyCode.E) && shieldTimer <= 0){
            shieldDuration = 2f;
            shieldTimer = 30f;
        }
        else
        {
            shieldTimer -= Time.deltaTime;
        }

        // pajzs letelik
        if (shieldDuration > 0)
        {
            shieldDuration -= Time.deltaTime;
            spriteRenderer.color = Color.blue;
        }
        else
        {
            spriteRenderer.color = normalColor;
        }

        // Clamp the position of the character so they do not go out of bounds
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        currentPos.x = Mathf.Clamp(currentPos.x, leftEdge.x, rightEdge.x);

        // Set the new position
        transform.position = currentPos;

        if (Input.GetKey(KeyCode.Space))
        {
            foreach (Gun gun in guns)
            {
                gun.Shoot();
            }
        }

    }
    private void Start()
    {
        currentPos = transform.position;
        ColorUtility.TryParseHtmlString("#7EE62C", out normalColor);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void beUnkillable(float toSeconds)
    {
        shieldDuration = toSeconds;
        spriteRenderer.color = Color.blue;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shieldDuration > 0)
        {
            return;
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            GameManager.Instance.OnPlayerKilled(this);
        }
    }

}
