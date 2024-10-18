using System.Collections;
using System.Drawing;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Locations")]
    public GameObject[] moveSpots = new GameObject[4];
    public float waitTime;
    private int nthPoint = 0;

    public float speed;

    private Rigidbody2D RB;
    private Transform currentPoint;
    private Invader invader;

    private float timer = 0;

    // Start is called before the first frame update
    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        invader = GetComponent<Invader>();
        // Starting from the outside of map
        currentPoint = moveSpots[nthPoint].transform;
        timer = waitTime;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, speed * Time.deltaTime);
        if(!invader.autoRotate)
            invader.RotateTo(currentPoint);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
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
                    currentPoint = moveSpots[nthPoint].transform;
                }
                else
                {
                    invader.gameObject.SetActive(false);
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