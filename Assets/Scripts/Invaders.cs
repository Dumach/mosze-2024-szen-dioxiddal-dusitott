using System.Threading;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader invaderPrefab;
    public int numberOf;
    public float speed;

    [Header("Locations")]
    public Vector3[] moveSpots;
    public float startSpawningTime;
    public float waitTime;
    private Vector3 initialPosition;

    [Header("Missiles")]
    public Projectile missilePrefab;
    public float timeBetweenShoots = 2f;
    public float missileSpeed = 10f;

    private float timer = 0;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if(startSpawningTime > 0)
        {
            startSpawningTime--;
            return;
        }
        if (numberOf <= 0)
        {
            return;
        }
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

    private void createInvader()
    {
        Invader invader = Instantiate(invaderPrefab, transform.position, Quaternion.identity);
        EnemyPatrol patrol = invader.GetComponent<EnemyPatrol>();
        patrol.moveSpots = moveSpots;
        patrol.speed = speed;
        patrol.waitTime = waitTime;
        invader.MissilePrefab = missilePrefab;
        invader.timeBetweenShoots = timeBetweenShoots;
        invader.missileSpeed = missileSpeed;
    }

    private void OnDrawGizmos()
    {
        foreach (var point in moveSpots)
        {
            Gizmos.DrawWireSphere(point, 0.5f);
        }
    }

}
