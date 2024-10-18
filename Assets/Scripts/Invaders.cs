using System.Threading;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader invaderPrefab;
    public int numberOf;
    public float speed;

    [Header("Locations")]
    public GameObject[] moveSpots = new GameObject[4];
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
            Invader invader = Instantiate(invaderPrefab, transform.position, Quaternion.identity);   
            EnemyPatrol patrol = invader.GetComponent<EnemyPatrol>();
            patrol.moveSpots = moveSpots;
            patrol.speed = speed;
            patrol.waitTime = waitTime;
            invader.MissilePrefab = missilePrefab;
            invader.timeBetweenShoots = timeBetweenShoots;
            invader.missileSpeed = missileSpeed;
            timer = waitTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void ResetInvaders()
    {
        transform.position = initialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf)
            {
                count++;
            }
        }
        return count;
    }

    private void OnDrawGizmos()
    {
        foreach (var point in moveSpots)
        {
            Gizmos.DrawWireSphere(point.transform.position, 0.5f);
        }
    }

}
