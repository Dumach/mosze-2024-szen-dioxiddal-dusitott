using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScroll : MonoBehaviour
{

    public float scrollSpeed = 2f;
    private float height;
    private float offset;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        offset = transform.position.y;
        height = GetComponent<SpriteRenderer>().size.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Loop back the first background if it reaches the end
        // the image is offset by 16 so that the bottom of the camera is aligned with the bottom of the image
        // Math: background height = 62.5, offset = 16
        // 16 - 62.5 = -46,5
        if (transform.position.y < offset - height)
        {
            transform.position = startPosition;
        }
    }
}
