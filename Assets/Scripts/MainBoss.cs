using Codice.Client.Common.GameUI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainBoss : MonoBehaviour
{
    [Header("Boss stats")]
    public float speed = 4f;               // Speed of movement across the screen
    public float cycleTime = 5f;          // Interval between direction changes
    public float startSpawningTime = 5f;  // Countdown before initial entry from top
    public bool secondPhase = false;
    public BossHealthBar healthBar;

    [SerializeField] private Text BossWarning;

    private Vector2 centerRightDestination;
    private Vector2 centerLeftDestination;
    private Vector2 centerScreen;          // Center point on the screen
    private bool movingRight = true;       // Direction control

    private Invader invaderComponent;
    private int healthStart;
    private bool isInSecondPhase = false;

    private void Start()
    {
        invaderComponent = gameObject.GetComponent<Invader>();
        invaderComponent.autoAim = false;
        invaderComponent.autoShoot = false;
        invaderComponent.GetComponent<BoxCollider2D>().enabled = false;
        //healthStart = invaderComponent.health;


        // Convert viewport edges to world coordinates to set destination points
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        //Vector3 topOutside = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.2f, 0)); // Start above screen
        Vector3 centerScreenPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.8f, 0));

        // Set destinations to halfway points
        centerScreen = new Vector2(centerScreenPoint.x, centerScreenPoint.y);
        centerRightDestination = new Vector2((centerScreenPoint.x + rightEdge.x) / 2, centerScreenPoint.y);
        centerLeftDestination = new Vector2((centerScreenPoint.x + leftEdge.x) / 2, centerScreenPoint.y);


        // Start the countdown for the initial spawn
        StartCoroutine(StartSpawnCountdown());
    }

    private IEnumerator StartSpawnCountdown()
    {
        // 120s - 10s = 110s
        float firstphase = startSpawningTime - 10;
        yield return new WaitForSeconds(firstphase);

        BossWarning.gameObject.SetActive(true);

        // 120s - 110s = 10s
        // Wait until startSpawningTime reaches 0
        yield return new WaitForSeconds(startSpawningTime - firstphase);

        // Slowly move from top to the center of the screen
        while (Vector2.Distance(transform.position, centerScreen) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, centerScreen, speed * Time.deltaTime);
            yield return null;
        }

        BossWarning.gameObject.SetActive(false);
        //invaderComponent.health = healthStart;
        invaderComponent.GetComponent<BoxCollider2D>().enabled = true;

        // Start monitoring health for phase change
        StartCoroutine(CheckHealthForSecondPhase());

        // Start alternating movement once in the center
        StartCoroutine(AlternateMovement());
    }

    private IEnumerator AlternateMovement()
    {
        invaderComponent.autoAim = true;
        invaderComponent.autoShoot = true;

        while (true)
        {
            // Set target based on direction
            Vector2 target = movingRight ? centerRightDestination : centerLeftDestination;

            // Move towards the target
            while (Vector2.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }

            // Reverse direction when reaching the target
            movingRight = !movingRight;

            // Wait for cycleTime before moving again
            yield return new WaitForSeconds(cycleTime);
        }
    }
    private IEnumerator CheckHealthForSecondPhase()
    {
        while (secondPhase && !isInSecondPhase)
        {
            if (invaderComponent.health <= healthStart * 0.5f)
            {
                ActivateSecondPhase();
            }
            yield return null;
        }
    }
    private void ActivateSecondPhase()
    {
        isInSecondPhase = true;

        // Adjust boss behavior for second phase
        speed *= 1.5f; // Increase speed
        cycleTime /= 1.5f; // Reduce time between movement cycles



        Debug.Log("Second phase activated!");
    }
}



