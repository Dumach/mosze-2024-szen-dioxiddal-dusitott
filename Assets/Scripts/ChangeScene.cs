using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("CurrentMission"))
        {
            int index = PlayerPrefs.GetInt("CurrentMission");
            if (index > 0) SceneManager.LoadScene(index);
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}