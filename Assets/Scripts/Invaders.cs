using System.Collections.Generic;
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

    [Header("Guns")]
    public List<Gun> guns = new List<Gun>();
    public Vector3[] gunPosition;
    public Quaternion[] gunRotation;
    public Projectile[] projectilePrefabArr;
    public float[] timeBetweenShootsArr;
    public float[] missileSpeedArr;

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
        List<Gun> guns = new List<Gun>();
        for(int i = 0; i <  projectilePrefabArr.Length; i++)
        {
            Gun newgun = new Gun();
            newgun.transform.position = gunPosition[i];
            newgun.transform.rotation = gunRotation[i];
            newgun.projectilePrefab = projectilePrefabArr[i];
            newgun.timeBetweenShoots = timeBetweenShootsArr[i];
            newgun.missileSpeed = missileSpeedArr[i];
            guns.Add(newgun);
        }
        
        /*invader.MissilePrefab = missilePrefab;
        invader.timeBetweenShoots = timeBetweenShoots;
        invader.missileSpeed = missileSpeed;*/
    }

    private void OnDrawGizmos()
    {
        foreach (var point in moveSpots)
        {
            Gizmos.DrawWireSphere(point, 0.5f);
        }
    }

}
