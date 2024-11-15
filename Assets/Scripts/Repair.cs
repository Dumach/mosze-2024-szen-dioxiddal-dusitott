using UnityEngine;

/// \class Repair
/// \brief This class is responsible for interacting with the Repair-kit item
public class Repair : MonoBehaviour
{
    /// \brief Initial speed that the Repair-kit is falling towards bottom of the screen
    [SerializeField] private float speedHeal = 1f;

    public AudioClip healSound;
    public float healVolume = 1.0f;
    private AudioSource audioSource;


    private void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.down * speedHeal;

        InitializeAudioSource();
    }

    /// \brief Detects collisions with player or boundry.
    /// \param other The collider of the object that triggered the collision.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.HealPlayer();


            // Play the heal sound if it's assigned
            if (healSound != null)
            {
                audioSource.PlayOneShot(healSound, healVolume);
            }
            Destroy(this.gameObject);

        }
        // If bottom boundry reached, destroy item
        if (other.gameObject.layer == LayerMask.NameToLayer("Boundry") && 
            other.gameObject.name == "BoundaryDown")
        {
            Destroy(this.gameObject);
        }
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
