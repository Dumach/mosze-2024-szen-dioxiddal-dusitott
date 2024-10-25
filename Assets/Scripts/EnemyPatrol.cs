using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Invader))]
/// \class EnemyPatrol
/// \brief Handles patrol behavior for enemy invaders, moving them between defined spots and controlling their shooting behavior.
public class EnemyPatrol : MonoBehaviour
{
    /// \brief Array of locations (patrol points) where the enemy will move.
    [Header("Locations")]
    public Vector3[] moveSpots;

    /// \brief The time the enemy will wait at each patrol point before moving to the next one.
    public float waitTime;

    /// \brief The speed at which the enemy moves between points.
    public float speed;

    /// \brief Current patrol point index.
    private int nthPoint = 0;
    /// \brief Rigidbody2D component for controlling the enemy's physics.
    private Rigidbody2D RB;
    /// \brief Current target point where the enemy is moving.
    private Vector3 currentPoint;
    /// \brief Reference to the associated Invader component.
    private Invader invader;
    /// \brief Timer for controlling the waiting time at patrol points.
    private float timer = 0;

    /// \brief Initializes the Rigidbody2D, Invader components, and sets the first patrol point.
    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        invader = GetComponent<Invader>();

        if (moveSpots.Length > 0)
        {
            /// \brief Set the first patrol point as the current target.
            currentPoint = moveSpots[0];
        }

        /// \brief Initialize the timer with the wait time at patrol points.
        timer = waitTime;
    }

    /// \brief Handles the movement and behavior of the enemy at each frame.
    private void Update()
    {
        // Exit if there is no invader component or no patrol points.
        if (invader == null || currentPoint == null)
        {
            return;
        }

        // Move towards the current patrol point.
        transform.position = Vector2.MoveTowards(transform.position, currentPoint, speed * Time.deltaTime);

        // Rotate towards the current point if auto-rotate is disabled.
        if (!invader.autoRotate)
        {
            invader.RotateTo(currentPoint);
        }

        // Check if the enemy has reached the current patrol point.
        if (Vector2.Distance(transform.position, currentPoint) < 0.5f)
        {
            // Enable automatic shooting and rotation while waiting at the patrol point.
            invader.autoRotate = true;
            invader.autoShoot = true;
            invader.autoAim = true;

            // Start countdown before moving to the next patrol point.
            if (timer <= 0.25f)
            {
                timer = waitTime;

                // Move to the next location or destroy the invader if all points are visited.
                if (nthPoint < moveSpots.Length - 1)
                {
                    nthPoint++;
                    currentPoint = moveSpots[nthPoint];
                }
                else
                {
                    /// \brief Destroy the invader after reaching the last patrol point.
                    Destroy(gameObject); 
                }

                // Disable automatic shooting and rotation while moving to the next point.
                invader.autoRotate = false;
                invader.autoShoot = false;
                invader.autoAim = false;
            }
            else
            {
                // Continue the waiting countdown.
                timer -= Time.deltaTime;
            }
        }
    }
}
