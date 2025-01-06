using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public static bool isPaused;
    public AudioSource backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        float volume = 1f;
        if (PlayerPrefs.HasKey("Volume")) volume = PlayerPrefs.GetFloat("Volume");
        backgroundMusic.volume = volume;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
            isPaused = true;

        if(backgroundMusic != null)
        {
            backgroundMusic.Pause();
        }

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
            isPaused = false;

        if ( backgroundMusic != null)
        {
            backgroundMusic.UnPause();
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }

}

