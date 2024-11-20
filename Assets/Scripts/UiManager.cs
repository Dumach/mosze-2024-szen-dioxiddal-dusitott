using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class UiManager : MonoBehaviour
{
    /// \brief Singleton instance of the GameManager.
    public static UiManager Instance { get; private set; }

    /// \brief UI text for displaying the player's score.
    [SerializeField] private Text scoreIndicator;
    [SerializeField] private Text scoreText;

    /// \brief UI text for displaying the high score.
    [SerializeField] private Text highScoreIndicator;
    [SerializeField] private Text highScoreText;

    /// \brief UI text for displaying the player's remaining lives.
    [SerializeField] private Text livesText;

    /// \brief UI element for displaying information.
    [SerializeField] private GameObject infoUI;

    /// \brief UI element for displaying the pause menu.
    [SerializeField] private GameObject pauseUI;

    /// \brief UI element for displaying the end of mission menu.
    [SerializeField] private GameObject endUI;

    /// \brief UI element for the progress bar fill.
    [SerializeField] private Image progressBarFill;

    /// \brief Elapsed time for tracking mission progress.
    private float elapsedTime = 0f;

    /// \brief Mission time in seconds.
    private float missionTime;

    /// \brief Counter for tracking the number of times the high score text flashes.
    private int flashCount = 0;

    /// \brief Flag indicating if the high score text is flashing.
    private bool isFlashing = false;
    
    /// \brief The high score for the current mission.
    private int highScore = 0;

    /// \brief The maximum health of the player.
    private int maxHealth;

    /// \brief The current scene index.
    private int sceneIndex;

    /// \brief Initializes the GameManager as a singleton and ensures only one instance exists.
    public void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// \brief Ensures the instance is null when this object is destroyed.
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        missionTime = GameObject.Find("GameManager").GetComponent<GameManager>().missionTime;

        maxHealth = player.health;
        if(livesText != null) livesText.text = maxHealth.ToString();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        PlayerPrefs.SetInt("CurrentMission", sceneIndex);
        if (PlayerPrefs.HasKey("HighScore" + sceneIndex))
        {
            highScore = PlayerPrefs.GetInt("HighScore" + sceneIndex);
            highScoreIndicator.text = highScore.ToString().PadLeft(4, '0');
        }

        InvokeRepeating("UpdateProgressBar", 0f, 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.SetInt("HighScore" + sceneIndex, 0);
            highScoreIndicator.text = "".PadLeft(4, '0');
        }
    }

    /// \brief Updates the progress bar UI element based on elapsed time.
    private void UpdateProgressBar()
    {
        if (progressBarFill == null) return;

        elapsedTime += 0.5f;
        float progress = Mathf.Clamp01(elapsedTime / missionTime);

        progressBarFill.fillAmount = progress;
    }

    public void StartGameUI()
    {
        CancelInvoke("UpdateProgressBar");
        infoUI.SetActive(false);
        endUI.SetActive(true);
    }

    public void HandleGameOverUI(int score)
    {
        GameObject.Find("levelText").GetComponent<Text>().text = "Level " + sceneIndex + " failed!";
        GameObject.Find("scoresText").GetComponent<Text>().text = "Scores: " + score;
        pauseUI.SetActive(false);
    }

    public void UpdateScoreUI(int score)
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore" + sceneIndex, highScore);
            if (highScoreIndicator != null) highScoreIndicator.text = highScore.ToString().PadLeft(4, '0');
            NewRecord();
        }
        if (scoreIndicator != null) scoreIndicator.text = score.ToString().PadLeft(4, '0');
    }

    /// \brief Initiates a flashing effect on the high score text when a new high score is achieved.
    private void NewRecord()
    {
        if (!isFlashing)
        {
            StartCoroutine(BlinkHighScoreText());
        }
    }

    /// \brief Coroutine to flash the high score text a few times.
    private IEnumerator BlinkHighScoreText()
    {
        isFlashing = true;

        while (flashCount < 3)
        {
            ColorUtility.TryParseHtmlString("#0A940F", out Color highlightColor);
            highScoreText.color = highlightColor;
            highScoreText.text = "New Record";

            yield return new WaitForSeconds(1);

            ColorUtility.TryParseHtmlString("#C57C04", out Color normalColor);
            highScoreText.color = normalColor;
            highScoreText.text = "High Score";

            yield return new WaitForSeconds(1);

            flashCount++;
        }

        flashCount = 0;
        isFlashing = false;
    }

    public void UpdatePlayerHealthUI(int health)
    {
        if (livesText != null) livesText.text = health.ToString();
    }

    public void HandleEndOfMissionUI(bool isLastMission, int totalScore = 0, int score = 0)
    {
        if (isLastMission)
        {
            GameObject.Find("NextButton").SetActive(false);
            GameObject.Find("levelText").GetComponent<Text>().text = "You win the game!";
            GameObject.Find("scoresText").GetComponent<Text>().text = "Total score: " + totalScore;
        }
        else
        {
        GameObject.Find("levelText").GetComponent<Text>().text = "Level " + sceneIndex + " completed!";
        GameObject.Find("scoresText").GetComponent<Text>().text = "Scores: " + score;
        }
    }
}
