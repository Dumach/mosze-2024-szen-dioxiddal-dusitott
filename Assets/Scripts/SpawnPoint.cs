using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// This class is responsible for spawning invaders in a structured manner
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// The invader prefab to be spawned.
    /// </summary>
    [Header("Invaders")]
    public Invader invaderPrefab;

    /// <summary>
    /// The number of invaders to spawn.
    /// </summary>
    public int numberOf;

    /// <summary>
    /// The speed at which the spawned invaders will move.
    /// </summary>
    public float speed;


    /// <summary>
    /// Whether the spawned invaders will automatically rotate.
    /// </summary>
    [Header("RotateAndShoot")]
    public bool autoRotate;

    /// <summary>
    /// Whether the spawned invaders will automatically shoot.
    /// </summary>
    public bool autoShoot;

    /// <summary>
    /// Whether the spawned invaders will automatically aim.
    /// </summary>
    public bool autoAim;


    /// <summary>
    /// Array of points where the invaders will patrol.
    /// </summary>
    [Header("Locations")]
    public Vector3[] moveSpots;

    /// <summary>
    /// Time to wait before starting to spawn invaders.
    /// </summary>
    public float startSpawningTime;

    /// <summary>
    /// Time to wait between spawning each invader.
    /// </summary>
    public float waitTime;

    /// <summary>
    /// Initial position of the spawn point.
    /// </summary>
    private Vector3 initialPosition;

    /// <summary>
    /// Timer used to control the interval between invader spawns.
    /// </summary>
    private float timer = 0;

    /// <summary>
    /// Initializes the spawn point's initial position.
    /// </summary>
    private void Start()
    {
        initialPosition = transform.position;
    }

    /// <summary>
    /// Handles the countdown to start spawning invaders and manages the spawning process.
    /// </summary>
    private void Update()
    {
        // Countdown before starting to spawn invaders
        if (startSpawningTime > 0)
        {
            startSpawningTime -= Time.deltaTime;
            return;
        }

        // Deactivate the spawn point if no more invaders are left to spawn
        if (numberOf <= 0)
        {
            this.gameObject.SetActive(false);
            return;
        }

        // Spawn invaders when the timer reaches zero
        if (timer <= 0)
        {
            numberOf--;
            createInvader();
            timer = waitTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Creates an invader at the spawn point and configures its behavior.
    /// </summary>
    private void createInvader()
    {
        // Instantiate a new invader at the current position
        Invader invader = Instantiate(invaderPrefab, transform.position, Quaternion.identity);
        invader.autoAim = autoAim;
        invader.autoShoot = autoShoot;
        invader.autoRotate = autoRotate;

        // Set the patrol points and movement speed for the invader
        EnemyPatrol patrol = invader.GetComponent<EnemyPatrol>();
        patrol.moveSpots = moveSpots;
        patrol.speed = speed;
        patrol.waitTime = waitTime;
    }

    /// <summary>
    /// Draws gizmos in the Unity Editor to visualize the patrol points for the invaders.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Draw a wireframe sphere at each patrol point to show their locations
        foreach (var point in moveSpots)
        {
            Gizmos.DrawWireSphere(point, 0.5f);
        }
    }
}
