using UnityEngine;
using UnityEngine.SceneManagement;

/// \class ChangeScene
/// \brief This class is responsible for loading the missions
public class ChangeScene : MonoBehaviour
{
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

    /// \brief Handle's the logic of NewGame button
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    /// \brief Handle's the logic of Exit button
    public void Quit()
    {
        Application.Quit();
    }
}