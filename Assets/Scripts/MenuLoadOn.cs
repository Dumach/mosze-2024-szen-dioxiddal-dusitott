using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLoadOn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("CurrentMission"))
        {
            if(PlayerPrefs.GetInt("CurrentMission") > 0)
            {
                var continueTxt = GameObject.Find("ContinueText");
                
                // Make it Clickable and light color
                ColorUtility.TryParseHtmlString("#D1F8FF", out Color lightColor);
                continueTxt.GetComponent<Text>().color = lightColor;                
            }
        }
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
