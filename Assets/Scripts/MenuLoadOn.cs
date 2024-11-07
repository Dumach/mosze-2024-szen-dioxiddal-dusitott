using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoadOn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float volume = 0f;
        if (PlayerPrefs.HasKey("Volume")) volume = PlayerPrefs.GetFloat("Volume");
        else volume = 0.25f;
        gameObject.GetComponent<AudioSource>().volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
