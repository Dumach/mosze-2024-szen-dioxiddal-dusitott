using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for controlling and managing activites in game
/// such as: updating UI elements, handling killing enemies
/// </summary>
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    // UI elements for displaying the score and lives
    //[SerializeField] private GameObject gameOverUI;
    //[SerializeField] private GameObject mainMenuUI;
    //[SerializeField] private GameObject PauseUI;

    /// <summary>
    /// UI text for displaying the player's score.
    /// </summary>
    [SerializeField] private Text scoreText;

    /// <summary>
    /// UI text for displaying the player's remaining lives.
    /// </summary>
    [SerializeField] private Text livesText;

    /// <summary>
    /// Reference to the Player object in the game.
    /// </summary>
    private Player player;

    /// <summary>
    /// The current score of the player.
    /// </summary>
    public int score { get; private set; } = 0;

    /// <summary>
    /// Initializes the GameManager as a singleton and ensures only one instance exists.
    /// </summary>
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

    /// <summary>
    /// Ensures the instance is null when this object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    /// <summary>
    /// Finds the Player object at the start of the game.
    /// </summary>
    private void Start()
    {
        player = FindObjectOfType<Player>();
        // NewGame();
    }

    /// <summary>
    /// Monitors the player's health and restarts the scene if necessary.
    /// </summary>
    private void Update()
    {
        // Restart the scene if the player has no health or if the Enter key is pressed
        if (player.health <= 0 || Input.GetKeyDown(KeyCode.Return))
        {
            RestartScene();
        }
    }

    /// <summary>
    /// Restarts the current active scene.
    /// </summary>
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Triggers the game over state and restarts the scene.
    /// </summary>
    private void GameOver()
    {
        RestartScene();
    }

    /// <summary>
    /// Sets the player's score and updates the score UI.
    /// </summary>
    /// <param name="score">The new score to be set.</param>
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    /// <summary>
    /// Called when the player is killed. Decreases health and handles game over if necessary.
    /// </summary>
    /// <param name="player">The player object that was killed.</param>
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

    /// <summary>
    /// Called when an invader is killed. Increases the score and handles the invader's destruction.
    /// </summary>
    /// <param name="invader">The invader object that was killed.</param>
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

    /// <summary>
    /// Called when a boss ship is killed. Increases the player's score based on the boss ship's score.
    /// </summary>
    /// <param name="mainboss">The main boss ship that was killed.</param>
    public void OnBossShipKilled(MainBoss mainboss)
    {
        // Increase the score when the boss ship is killed
        SetScore(score + mainboss.score);
    }
}
