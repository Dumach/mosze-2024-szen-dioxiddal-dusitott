using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Invaders")]
    public Invader invaderPrefab;
    public int numberOf;
    public float speed;

    [Header("RotateAndShoot")]
    public bool autoRotate;
    public bool autoShoot;
    public bool autoAim;

    [Header("Locations")]
    public Vector3[] moveSpots;
    public float startSpawningTime;
    public float waitTime;
    private Vector3 initialPosition;

    private float timer = 0;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if(startSpawningTime > 0)
        {
            startSpawningTime -= Time.deltaTime;
            return;
        }
        if (numberOf <= 0)
        {
            this.gameObject.SetActive(false);
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
        invader.autoAim = autoAim;
        invader.autoShoot = autoShoot;
        invader.autoRotate = autoRotate;

        EnemyPatrol patrol = invader.GetComponent<EnemyPatrol>();
        patrol.moveSpots = moveSpots;
        patrol.speed = speed;
        patrol.waitTime = waitTime;
    }

    private void OnDrawGizmos()
    {
        foreach (var point in moveSpots)
        {
            Gizmos.DrawWireSphere(point, 0.5f);
        }
    }

}
