using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission
{
    // Invader
    public List<Invader> invaderPrefab;
    public List<int> numberOf;
    public List<float> speed;

    // Locations
    public List<Vector3[]> moveSpots;
    public List<float> startSpawningTime;
    public List<float> waitTime;
    private List<Vector3> initialPosition;

    // Missile
    public List<Projectile> missilePrefab;
    public List<float> timeBetweenShoots;   // = 2f
    public List<float> missileSpeed;    // = 10f

    // TODO!
    // Do save and load here
}
