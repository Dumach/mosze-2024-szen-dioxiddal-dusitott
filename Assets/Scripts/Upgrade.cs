using UnityEngine;

/// \class Upgrade
/// \brief This class is responsible for interacting with the Upgrade-kit item
public class Upgrade : MonoBehaviour
{
    /// \brief Initial speed that the Upgrade-kit is falling towards bottom of the screen
    [SerializeField] private float speedUpgrade = 1f;

    private void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.down * speedUpgrade;
    }

    /// \brief Detects collisions with player or boundry.
    /// \param other The collider of the object that triggered the collision.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.UpgradeWeapons();
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
