using System.Collections;
using System.Drawing;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Locations")]
    public Vector3[] moveSpots;
    public float waitTime;
    private int nthPoint = 0;

    public float speed;

    private Rigidbody2D RB;
    private Vector3 currentPoint;
    private Invader invader;

    private float timer = 0;

    // Start is called before the first frame update
    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        invader = GetComponent<Invader>();
        // Starting from the outside of map
        if (moveSpots.Length > 0)
        {
        currentPoint = moveSpots[nthPoint];
        }
        timer = waitTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if(invader == null)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, currentPoint, speed * Time.deltaTime);
        if(!invader.autoRotate)
            invader.RotateTo(currentPoint);

        if (Vector2.Distance(transform.position, currentPoint) < 0.5f)
        {
            // Starts shooting...
            invader.autoRotate = true;
            invader.autoShoot = true;

            if (timer <= 0.25)
            {
                timer = waitTime;

                // to next location
                if (nthPoint < moveSpots.Length - 1)
                {
                    nthPoint++;
                    currentPoint = moveSpots[nthPoint];//.transform;
                }
                else
                {
                    Destroy(gameObject);
                    //invader.gameObject.SetActive(false);
                }

                // Stops shooting...
                invader.autoRotate = false;
                invader.autoShoot = false;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }
}
