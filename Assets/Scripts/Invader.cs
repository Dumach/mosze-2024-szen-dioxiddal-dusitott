using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Invader : MonoBehaviour
{
    [Header("Animation")]
    public Sprite[] animationSprites = new Sprite[0];
    public float animationTime = 1f;
    public int score = 10;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    [Header("RotateAndShoot")]
    public GameObject player;
    public bool autoRotate { get; set; } = false;
    public bool autoShoot { get; set; } = false;


    [Header("Missiles")]
    public float timeBetweenShoots = 2f;
    public float missileSpeed = 10f;
    public Projectile MissilePrefab;
    private float gunHeat = 0f;
    private Projectile missile;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animationSprites[0];
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        // Return if there is no player/target to shoot
        if (player == null)
        {
            return;
        }
        
        // Rotate and shoot
        if(autoRotate) RotateTo(player.transform.position);
        if(autoShoot) ShootTo(player.transform);
    }

    public void RotateTo(Vector3 target)
    {
        // Rotating towards player
        float offset = 90f;
        Vector2 direction = target - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    private void ShootTo(Transform target)
    {
        // Shooting lasers generate heat aka. slows down the firing rate
        if (gunHeat > 0)
        {
            gunHeat -= Time.deltaTime;
        }
        if (gunHeat <= 0)
        {
            gunHeat += timeBetweenShoots;
            missile = Instantiate(MissilePrefab, transform.position, transform.rotation);
            missile.SetDirection(target.transform);
            missile.setSpeed(missileSpeed);
        }
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            GameManager.Instance.OnInvaderKilled(this);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            //GameManager.Instance.OnBoundaryReached();
        }
    }

}
