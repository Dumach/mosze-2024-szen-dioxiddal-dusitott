using UnityEngine;
using System.Collections.Generic;
using Codice.CM.Common.Merge;
using System;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

/// \class Player
/// \brief This class represents the player character, handling movement, shooting, and shield mechanics.
public class Player : MonoBehaviour
{
    /// \brief Speed of the player's movement.
    [Header("Player")]
    public float speed = 5f;

    /// \brief Player's health.
    public int health = 3;

    /// \brief SpriteRenderer component used to change the player's color for visual feedback.
    private SpriteRenderer spriteRenderer;

    /// \brief The current position of the player.
    private Vector3 currentPos;

    [Header("Weapon Templates")]
    /// \brief Current weapon template.
    public int currentTemplate = -1;

    /// \brief All weapon templates.
    public List<Player> upgradeTemplates = new List<Player>();

    /// \brief List of guns the player can shoot with.
    [Header("Guns")]
    public List<Gun> guns = new List<Gun>();

    /// \brief Duration of the player's shield (temporary invincibility).
    private float shieldDuration = 0f;

    /// \brief Timer to control the cooldown of the player's shield ability.
    private float shieldTimer = 0f;

    /// \brief Shield prefab reference
    public GameObject shieldBubblePrefab;

    /// \brief GameObject to store shield bubble instance
    private GameObject activeShieldBubble;

    private AudioSource audioSource;

    /// \brief Updates the player's position, shooting behavior, and shield activation every frame.
    private void Update()
    {
        // Update the position of the player based on the input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            currentPos.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            currentPos.x += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            currentPos.y += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            currentPos.y -= speed * Time.deltaTime;
        }

        // Shield activation and cooldown
        if (Input.GetKeyDown(KeyCode.E) && shieldTimer <= 0)
        {
            /// \brief Shield lasts for 2 seconds.
            shieldDuration = 2f; 
            
            /// \brief Shield can be used again after 30 seconds.
            shieldTimer = 30f;

            ActivateShieldBubble();
        }
        else
        {
            shieldTimer -= Time.deltaTime;
        }

        // Shield effect wears off
        if (shieldDuration > 0)
        {
            shieldDuration -= Time.deltaTime;
        }
        else
        {
            DectivateShieldBubble();
        }

        // Clamp the position of the player within the screen bounds
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        currentPos.x = Mathf.Clamp(currentPos.x, leftEdge.x + 1, rightEdge.x - 1);
        Vector3 upperEdge = Camera.main.ViewportToWorldPoint(Vector3.up);
        Vector3 bottomEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        currentPos.y = Mathf.Clamp(currentPos.y, bottomEdge.y + 1, upperEdge.y - 1);

        // Set the new position
        transform.position = currentPos;

        // Shooting mechanism
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (Gun gun in guns)
            {
                gun.Shoot();
            }
        }
    }

    public void acquireSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// \brief Initializes the player's position, normal color, and sprite renderer.
    private void Start()
    {
        currentPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.Instance.UpgradeWeapons();
        audioSource = GetComponent<AudioSource>();
    }

    /// \brief Makes the player temporarily invincible for a specified duration.
    /// \param toSeconds Duration for which the player will be invincible.
    public void beUnkillable(float toSeconds)
    {
        shieldDuration = toSeconds;
    }

    /// \brief Handles collision with missiles or invaders, triggering the player's death if they are not shielded.
    /// \param other The collider of the object that triggered the collision.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shieldDuration > 0)
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("InvaderMissile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            GameManager.Instance.OnPlayerKilled();
        }
    }

    public void ActivateShieldBubble()
    {
        if (activeShieldBubble == null)
        {
            activeShieldBubble = Instantiate(shieldBubblePrefab, transform.position, Quaternion.identity);
            activeShieldBubble.transform.SetParent(transform);
            activeShieldBubble.transform.localPosition = Vector3.zero;
        }
    }
    
    public void DectivateShieldBubble()
    {
        if (activeShieldBubble != null)
        {
            Destroy(activeShieldBubble);
            activeShieldBubble = null;
        }
    }
}
