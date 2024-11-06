using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// \class SpawnPoint
/// \brief This class is responsible for spawning invaders in a structured manner.
public class SpawnPoint : MonoBehaviour
{
    /// \brief The invader prefab to be spawned.
    [Header("Invaders")]
    public Invader invaderPrefab;

    /// \brief The number of invaders to spawn.
    public int numberOf;

    /// \brief The speed at which the spawned invaders will move.
    public float speed;

    /// \brief Whether the spawned invaders will automatically rotate.
    [Header("RotateAndShoot")]
    public bool autoRotate;

    /// \brief Whether the spawned invaders will automatically shoot.
    public bool autoShoot;

    /// \brief Whether the spawned invaders will automatically aim.
    public bool autoAim;

    /// \brief To enable or disable enemy shooting.
    public bool shootingEnabled = true;

    /// \brief Array of points where the invaders will patrol.
    [Header("Locations")]
    public Vector3[] moveSpots;

    /// \brief Time to wait before starting to spawn invaders.
    public float startSpawningTime;

    /// \brief Time to wait between spawning each invader.
    public float waitTime;

    /// \brief Initial position of the spawn point.
    private Vector3 initialPosition;

    /// \brief Timer used to control the interval between invader spawns.
    private float timer = 0;

    /// \brief Initializes the spawn point's initial position.
    private void Start()
    {
        initialPosition = transform.position;
    }

    /// \brief Handles the countdown to start spawning invaders and manages the spawning process.
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
            CreateInvader();
            timer = waitTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    /// \brief Creates an invader at the spawn point and configures its behavior.
    private void CreateInvader()
    {
        // Instantiate a new invader at the current position
        Invader invader = Instantiate(invaderPrefab, transform.position, Quaternion.identity);
        invader.autoAim = autoAim;
        invader.autoShoot = autoShoot;
        invader.shootingEnabled = shootingEnabled;
        invader.autoRotate = autoRotate;

        // Set the patrol points and movement speed for the invader
        EnemyPatrol patrol = invader.gameObject.AddComponent<EnemyPatrol>();
        patrol.moveSpots = moveSpots;
        patrol.speed = speed;
        patrol.waitTime = waitTime;
    }

    /// \brief Draws gizmos in the Unity Editor to visualize the patrol points for the invaders.
    private void OnDrawGizmos()
    {
        // Draw a wireframe sphere at each patrol point to show their locations
        foreach (var point in moveSpots)
        {
            Gizmos.DrawWireSphere(point, 0.5f);
        }
    }
}
