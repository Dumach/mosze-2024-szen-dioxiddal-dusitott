using UnityEngine;

/// \class Upgrade
/// \brief This class is responsible for interacting with the Upgrade-kit item
public class Upgrade : MonoBehaviour
{
    /// \brief Initial speed that the Upgrade-kit is falling towards bottom of the screen
    [Header("Movement and sound")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private AudioClip sound;
    [SerializeField] private float volume = 1.0f;

    private void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.down * speed;
    }

    /// \brief Detects collisions with player or boundry.
    /// \param other The collider of the object that triggered the collision.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.UpgradeWeapons();
            if (sound != null)
            {
                GameObject sfxPlayer = GameObject.Find("SFXPlayer");
                AudioSource aud = sfxPlayer.GetComponent<AudioSource>();
                aud.PlayOneShot(sound, volume);
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
}
