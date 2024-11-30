using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// \class ChangeScene
/// \brief This class is responsible for loading the missions
public class ChangeScene : MonoBehaviour
{
    [Header("UI Stuff")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject NameMenu;
    [SerializeField] private GameObject LeaderBoard;
    [SerializeField] private GameObject elementWrapper;
    [SerializeField] private GameObject HighScoreElementPrefab;

    List<GameObject> uiElements = new List<GameObject>();
    List<HighscoreElement> highscoreList = new List<HighscoreElement>();

    /// \brief Handle's the logic of ContinueGame button
    public void ContinueGame()
    {
        // Saved game is an index of last played mission
        if (PlayerPrefs.HasKey("CurrentMission"))
        {
            int index = PlayerPrefs.GetInt("CurrentMission");
            if (index > 0) SceneManager.LoadScene(index);
        }
    }

    /// \brief Upon pressing the leaderboard button, it shows the top 7 highest scores with names
    public void ShowLeaderboard()
    {

        for (int i = 0; i < highscoreList.Count; i++)
        {
            HighscoreElement el = highscoreList[i];

            if (el != null && el.points > 0)
            {
                if (i >= uiElements.Count)
                {
                    // instantiate new entry
                    var inst = Instantiate(HighScoreElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(elementWrapper.transform, false);

                    uiElements.Add(inst);
                }

                // write or overwrite name & points
                var texts = uiElements[i].GetComponentsInChildren<Text>();
                texts[0].text = el.playerName;
                texts[1].text = el.points.ToString();
            }
        }
        LeaderBoard.SetActive(true);
    }

    private void Start()
    {
        highscoreList = FileHandler.ReadListFromJSON<HighscoreElement>("scores.json");
        if(highscoreList.Count > 0) PlayerPrefs.SetInt("HighScore", highscoreList[0].points);
        else PlayerPrefs.SetInt("HighScore", 0);
    }

    public void HideLeaderboard()
    {
        LeaderBoard.SetActive(false);
    }

    /// \brief Handle's the logic of NewGame button
    public void NewGame()
    {
        MainMenu.SetActive(false);
        NameMenu.SetActive(true);
    }

    /// \brief Handle's the logic of submitting the players name
    public void SubmitName()
    {
        var inputField = GameObject.Find("NameInputField");
        var name = inputField.GetComponent<InputField>().text;
        
        if (name == null || name == "") return;

        PlayerPrefs.SetString("Name", name);
        PlayerPrefs.SetInt("Score", 0);

        SceneManager.LoadScene(1);
    }

    /// \brief Handle's the logic of Cancel button
    public void Cancel()
    {
        var inputField = GameObject.Find("NameInputField");
        inputField.GetComponent<InputField>().text = "";
        MainMenu.SetActive(true);
        NameMenu.SetActive(false);
    }

    /// \brief Handle's the logic of Exit button
    public void Quit()
    {
        Application.Quit();
    }
}