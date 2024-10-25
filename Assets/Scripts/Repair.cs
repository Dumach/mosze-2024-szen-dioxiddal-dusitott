using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : MonoBehaviour
{
    private void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.down * 1;
    }

    /// \brief Detects collisions with player or boundry.
    /// \param other The collider of the object that triggered the collision.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.healPlayer();
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
