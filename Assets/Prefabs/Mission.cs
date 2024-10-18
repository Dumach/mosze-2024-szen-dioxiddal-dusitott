using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
    // Invaders
    public Invader invaderPrefab;
    public int numberOf;
    public float speed;

    // Locations
    public GameObject[] moveSpots;
    public float startSpawningTime;
    public float waitTime;
    public Vector3 initialPosition;

    // Missiles
    public Projectile missilePrefab;
    public float timeBetweenShoots = 2f;
    public float missileSpeed = 10f;
}
