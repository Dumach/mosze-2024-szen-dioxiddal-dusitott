using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public GameManager GameManager;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = GameManager.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
