using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLoadOn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Decides if player can continue a game
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

        // Setting the already saved volume to audiosource
        float volume = 1f;
        if (PlayerPrefs.HasKey("Volume")) volume = PlayerPrefs.GetFloat("Volume");
        gameObject.GetComponent<AudioSource>().volume = volume;

        // Setting the highest score from persistent storage
        List<HighscoreElement> highscoreList = new List<HighscoreElement>();
        highscoreList = FileHandler.ReadListFromJSON<HighscoreElement>("scores.json");
        if (highscoreList.Count > 0) PlayerPrefs.SetInt("HighScore", highscoreList[0].points);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
