using UnityEngine;

/// \class Gun
/// \brief This class handles shooting projectiles with heat-based cooldown for firing.
public class Gun : MonoBehaviour
{
    /// \brief The projectile prefab that will be instantiated when shooting.
    [Header("Gun properties")]
    public Projectile projectilePrefab;

    /// \brief The time interval between each shot.
    public float timeBetweenShoots = 0.5f;

    /// \brief The speed at which the projectile will travel.
    public float missileSpeed = 10f;

    /// \brief The current heat level of the gun, which affects firing rate.
    private float gunHeat = 0f;

    /// \brief The layer that is inherited by player or invader class
    public int layerIndex;

    /// \brief Initializes the sound of projectile 
    public AudioClip pewPewSFX;

    /// \brief Initializes the volume of projectile sound
    public float pewpewVolume = 1.0f;

    /// \brief Initializes the audio source of projectile
    private AudioSource audioSource;

    /// \brief Reference to the instantiated projectile.
    private Projectile projectile;

    /// \brief Shoots a projectile towards a target if available and applies cooldown.
    /// \param target Optional. The target at which the projectile will be aimed.
    public void Shoot(Transform target = null)
    {
        // Shooting lasers generate heat, which slows down the firing rate
        if (gunHeat <= 0)
        {
            // Projectile is being cloned from a prefab, then general settings applied
            gunHeat += timeBetweenShoots;
            projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

            // Play laser sound
            if (pewPewSFX != null)
            {
                audioSource.PlayOneShot(pewPewSFX, pewpewVolume);
            }

            projectile.setSpeed(missileSpeed);
            projectile.SetDirection(target);
            projectile.gameObject.layer = layerIndex;
        }
    }

    /// \brief Reduces gun heat over time to allow firing after a cooldown period.
    private void Update()
    {
        if (gunHeat > 0)
        {
            gunHeat -= Time.deltaTime;
        }
    }

    /// \brief Initializes the volume of projectile sound
    private void Start()
    {
        InitializeAudioSource();
    }

    /// \brief Initializes the audio source of projectile
    private void InitializeAudioSource()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
}
