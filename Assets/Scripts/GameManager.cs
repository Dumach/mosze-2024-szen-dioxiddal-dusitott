using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Codice.CM.Common.Checkin.Partial;
using Codice.Client.Common;

/// \class GameManager
/// \brief This class is responsible for controlling and managing activities in the game
/// such as: updating UI elements, handling killing enemies.
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    /// \brief Singleton instance of the GameManager.
    public static GameManager Instance { get; private set; }

    // UI elements for displaying the score and lives
    //[SerializeField] private GameObject gameOverUI;
    //[SerializeField] private GameObject mainMenuUI;
    //[SerializeField] private GameObject PauseUI;

    /// \brief UI text for displaying the player's score.
    [SerializeField] private Text scoreIndicator;
    [SerializeField] private Text scoreText;

    [SerializeField] private Text highScoreIndicator;
    [SerializeField] private Text highScoreText;
    /// \brief UI text for displaying the player's remaining lives.
    [SerializeField] private Text livesText;

    /// \brief Reference to the Player object in the game.
    private Player player;
    private int maxHealth;

    private int flashCount = 0;             // A villanások számolása
    private bool isFlashing = false;        // Villogás folyamatban van-e

    /// \brief The current score of the player.
    public int score { get; private set; } = 0;
    private int highScore = 0;

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

    /// \brief Finds the Player object at the start of the game.
    private void Start()
    {
        player = FindObjectOfType<Player>();
        maxHealth = player.health;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
            highScoreIndicator.text = highScore.ToString().PadLeft(4, '0');
        }
        // NewGame();
    }

    /// \brief Monitors the player's health and restarts the scene if necessary.
    private void Update()
    {
        // Restart the scene if the player has no health or if the Enter key is pressed
        if (player.health <= 0 || Input.GetKeyDown(KeyCode.Return))
        {
            GameOver();
        }
        // Reset highScore
        if (Input.GetKeyDown(KeyCode.R))
        {
            score = 0;
            PlayerPrefs.SetInt("HighScore", 0); 
            highScoreIndicator.text = "".PadLeft(4, '0');

        }
    }

    /// \brief Restarts the current active scene.
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// \brief Triggers the game over state and restarts the scene.
    private void GameOver()
    {
        RestartScene();
    }

    /// \brief Sets the player's score and updates the score UI.
    /// \param score The new score to be set.
    private void SetScore(int score)
    {
        this.score = score;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreIndicator.text = highScore.ToString().PadLeft(4, '0');
            NewRecord();
        }
        //if(scoreText != null)
        scoreIndicator.text = score.ToString().PadLeft(4, '0');
    }

    private void NewRecord()
    {
        if (!isFlashing)
        {
            StartCoroutine(BlinkHighScoreText());
        }
    }

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

    /// \brief Called when the player is killed. Decreases health and handles game over if necessary.
    public void OnPlayerKilled()
    {
        // Decrease player's health and update the lives UI
        player.health = Mathf.Max(player.health - 1, 0);
        livesText.text = player.health.ToString();

        if (player.health > 0)
        {
            // Make player temporarily unkillable
            player.beUnkillable(1.5f);
        }
        else
        {
            // If the player has no health, trigger game over
            player.gameObject.SetActive(false);
            GameOver();
        }
    }

    /// \brief Called when player picked up a repair kit. It heals the player.
    public void healPlayer()
    {
        // Increase player's health and update the lives UI
        if (player.health < maxHealth)
        {
            player.health++;
            livesText.text = player.health.ToString();
        }
    }

    /// \brief Called when player picked up a weapon upgrade kit.
    /// \brief It switches the players gun template to the next.
    public void upgradeWeapon()
    {
        int current = player.currentTemplate;
        if(current < player.upgradeTemplates.Count)
        {
            current++;
            player.guns = player.upgradeTemplates[current].guns;
        }
    }


    /// \brief Called when an invader is killed. Increases the score and handles the invader's destruction.
    /// \param invader The invader object that was killed.
    public void OnInvaderKilled(Invader invader)
    {
        // Reduce invader's health and check if it should be destroyed
        invader.health = Mathf.Max(invader.health - 1, 0);
        if (invader.health <= 0)
        {
            // Destroy the invader and update the player's score
            Destroy(invader.gameObject);
            SetScore(score + invader.score);
        }
    }

    /// \brief Called when a boss ship is killed. Increases the player's score based on the boss ship's score.
    /// \param mainboss The main boss ship that was killed.
    public void OnBossShipKilled(MainBoss mainboss)
    {
        // Increase the score when the boss ship is killed
        SetScore(score + mainboss.score);
    }
}
