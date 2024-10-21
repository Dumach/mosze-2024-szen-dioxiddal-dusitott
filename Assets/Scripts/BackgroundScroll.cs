using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class is responsible for the unlimited background effect
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScroll : MonoBehaviour
{
    /// <summary>
    /// The speed at which the background scrolls vertically.
    /// </summary>
    public float scrollSpeed = 2f;

    /// <summary>
    /// The height of the background sprite.
    /// </summary>
    private float height;

    /// <summary>
    /// The vertical offset of the background, used for resetting the position.
    /// </summary>
    private float offset;

    /// <summary>
    /// The initial starting position of the background.
    /// </summary>
    private Vector3 startPosition;

    /// <summary>
    /// Initializes the background's position, offset, and calculates the height of the background sprite.
    /// </summary>
    void Start()
    {
        startPosition = transform.position;
        offset = transform.position.y;
        height = GetComponent<SpriteRenderer>().size.y;
    }

    /// <summary>
    /// Updates the background's position every frame to create a scrolling effect. 
    /// Resets the position when the background scrolls off-screen.
    /// </summary>
    void Update()
    {
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
