using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// \class BackgroundScroll
/// \brief This class is responsible for the unlimited background effect.
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScroll : MonoBehaviour
{
    /// \brief The speed at which the background scrolls vertically.
    public float scrollSpeed = 2f;

    /// \brief The height of the background sprite.
    private float height;

    /// \brief The vertical offset of the background, used for resetting the position.
    private float offset;

    /// \brief The initial starting position of the background.
    private Vector3 startPosition;

    public bool beginScroll { get; set; } = true;

    /// \brief Initializes the background's position, offset, and calculates the height of the background sprite.
    void Start()
    {
        startPosition = transform.position;
        offset = transform.position.y;
        height = GetComponent<SpriteRenderer>().size.y;
    }

    /// \brief Updates the background's position every frame to create a scrolling effect. 
    /// Resets the position when the background scrolls off-screen.
    void Update()
    {
        if (!beginScroll) return;

        // Move the background downward based on the scroll speed and time.
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Check if the background has moved past its height and reset its position.
        // Math: background height = 62.5, offset = 16, reset position when y < offset - height (16 - 62.5 = -46.5).
        if (transform.position.y < offset - height)
        {
            transform.position = startPosition;
        }
    }
}
