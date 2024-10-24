using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]


/// \class Invader
/// \brief This class represents an invader enemy in the game that can rotate, shoot, and be animated.
public class Invader : MonoBehaviour
{
    /// \brief Array of sprites for the invader's animation.
    [Header("Animation")]
    public Sprite[] animationSprites = new Sprite[0];

    /// \brief Time between each frame of the invader's animation.
    public float animationTime = 1f;

    /// \brief Score value the player receives upon killing this invader.
    [Header("InvaderStats")]
    public int score = 10;

    /// \brief Health points of the invader.
    public int health = 1;

    /// \brief SpriteRenderer component used to change the invader's sprite.
    private SpriteRenderer spriteRenderer;

    /// \brief Current frame of the animation.
    private int animationFrame;

    /// \brief Reference to the player GameObject for targeting.
    [Header("RotateAndShoot")]
    public GameObject player;

    /// \brief Whether the invader will automatically rotate towards the player.
    public bool autoRotate;

    /// \brief Whether the invader will automatically shoot at the player.
    public bool autoShoot;

    /// \brief Whether the invader will automatically aim at the player.
    public bool autoAim;

    /// \brief List of Gun components attached to the invader for shooting.
    [Header("Guns")]
    public List<Gun> guns = new List<Gun>();

    /// \brief Initializes the invader, sets the sprite and animation frame.
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// \brief Starts the invader animation and sets the player reference.
    private void Start()
    {
        if (animationSprites.Length > 0)
        {
            spriteRenderer.sprite = animationSprites[0];
            InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
        }
        foreach (Gun gun in guns)
        {
            gun.layer = LayerMask.NameToLayer("InvaderMissile");
        }
        player = GameObject.Find("Player");
    }

    /// \brief Updates the invader's behavior, such as rotating and shooting at the player.
    private void Update()
    {
        // Return if there is no player/target to shoot
        if (player == null)
        {
            return;
        }

        // Rotate and shoot
        if (autoRotate) RotateTo(player.transform.position);
        if (autoShoot) ShootTo(player.transform);
    }

    /// \brief Rotates the invader to face the target position.
    /// \param target The position to rotate towards.
    public void RotateTo(Vector3 target)
    {
        // Rotating towards player
        float offset = 90f;
        Vector2 direction = target - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    /// \brief Shoots towards the target if auto-aim is enabled, otherwise shoots forward.
    /// \param target The target transform to aim at when shooting.
    private void ShootTo(Transform target)
    {
        // Shooting lasers generate heat aka. slows down the firing rate
        foreach (Gun gun in guns)
        {
            if (autoAim)
                gun.Shoot(target);
            else
                gun.Shoot();
        }
    }

    /// \brief Handles the invader's animation, cycling through the provided sprites.
    private void AnimateSprite()
    {
        animationFrame++;

        // Loop back to the start if the animation frame exceeds the length
        if (animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }

        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    /// \brief Detects collisions with lasers and triggers the invader's death when hit.
    /// \param other The collider of the object that triggered the collision.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerMissile"))
        {
            GameManager.Instance.OnInvaderKilled(this);
        }
    }
}
