using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTextDisplay : MonoBehaviour
{
    public Text levelText;
    public int levelNumber; 
    void Start()
    {
        if (levelText != null)
        {
            levelText.text = "Level " + levelNumber;
        }
    }
}
