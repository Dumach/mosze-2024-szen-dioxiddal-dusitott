using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun properties")]
    public Projectile projectilePrefab;
    public float timeBetweenShoots = 0.5f;
    public float missileSpeed = 10f;
    private float gunHeat = 0f;
    private Projectile projectile;

    public void Shoot(Transform target = null)
    {
        // Shooting lasers generate heat aka. slows down the firing rate
        if (gunHeat <= 0)
        {
            gunHeat += timeBetweenShoots;
            projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.setSpeed(missileSpeed);
            projectile.SetDirection(target);
        }
    }

    private void Update()
    {
        if (gunHeat > 0)
        {
            gunHeat -= Time.deltaTime;
        }
    }
}
