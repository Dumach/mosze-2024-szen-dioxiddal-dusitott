using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Text scoreText;

    /// \brief UI text for displaying the player's remaining lives.
    [SerializeField] private Text livesText;

    /// \brief Reference to the Player object in the game.
    private Player player;

    /// \brief The current score of the player.
    public int score { get; private set; } = 0;

    /// \brief Initializes the GameManager as a singleton and ensures only one instance exists.
    private void Awake()
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
        // NewGame();
    }

    /// \brief Monitors the player's health and restarts the scene if necessary.
    private void Update()
    {
        // Restart the scene if the player has no health or if the Enter key is pressed
        if (player.health <= 0 || Input.GetKeyDown(KeyCode.Return))
        {
            RestartScene();
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
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    /// \brief Called when the player is killed. Decreases health and handles game over if necessary.
    /// \param player The player object that was killed.
    public void OnPlayerKilled(Player player)
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
