using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// \class ChangeScene
/// \brief This class is responsible for loading the missions
public class ChangeScene : MonoBehaviour
{

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject NameMenu;
    [SerializeField] private GameObject LeaderBoard;

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

    public void ShowLeaderboard()
    {
        LeaderBoard.SetActive(true);
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