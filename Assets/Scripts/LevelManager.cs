using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] buttons;

    public Image Lock;

    private void Awake()
    {
        int unlockLevel = PlayerPrefs.GetInt("Level", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
            buttons[i].GetComponent<Image>().sprite = Lock.sprite;
            buttons[i].GetComponentInChildren<Text>().text = "";
        }
        for (int i = 0; i < unlockLevel; i++)
        {
            buttons[i].interactable = true;
            buttons[i].GetComponent<Image>().sprite = null;
            buttons[i].GetComponentInChildren<Text>().text = (i + 1).ToString();

        }
    }
    public void OpenLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

}