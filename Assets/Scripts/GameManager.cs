using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Random = UnityEngine.Random;

/// \class GameManager
/// \brief This class is responsible for controlling and managing activities in the game
/// such as: updating UI elements, handling killing enemies.
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    /// \brief Singleton instance of the GameManager.
    public static GameManager Instance { get; private set; }

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

    /// \brief Flag to determine if there's an end boss in the mission.
    [SerializeField] private bool hasEndBoss;

    /// \brief Mission time in seconds.
    public float missionTime;
    public GameObject invaderExplosion;
    public GameObject playerExplosion;

    /// \brief UI element for the progress bar fill.
    [SerializeField] private Image progressBarFill;

    /// \brief Elapsed time for tracking mission progress.
    private float elapsedTime = 0f;

    /// \brief Prefab for upgrade items dropped by enemies.
    [SerializeField] private Upgrade upgradePrefab;

    /// \brief Frequency at which upgrade items are dropped.
    [SerializeField] private int upgradeDropRate;

    /// \brief Prefab for repair kits dropped by enemies.
    [SerializeField] private Repair repairkitPrefab;

    /// \brief Frequency at which repair kits are dropped.
    [SerializeField] private int repairkitDropRate;

    /// \brief Reference to the Player object in the game.
    private Player player;

    /// \brief The maximum health of the player.
    private int maxHealth;

    /// \brief Counter for tracking the number of times the high score text flashes.
    private int flashCount = 0;

    /// \brief Flag indicating if the high score text is flashing.
    private bool isFlashing = false;

    /// \brief The current score of the player.
    public int score { get; private set; } = 0;

    /// \brief The high score for the current mission.
    private int highScore = 0;

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

    /// \brief Finds the Player object and initializes game variables at the start of the game.
    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        player = FindObjectOfType<Player>();
        maxHealth = player.health;
        livesText.text = maxHealth.ToString();
        PlayerPrefs.SetInt("CurrentMission", sceneIndex);
        if (PlayerPrefs.HasKey("HighScore" + sceneIndex))
        {
            highScore = PlayerPrefs.GetInt("HighScore" + sceneIndex);
            highScoreIndicator.text = highScore.ToString().PadLeft(4, '0');
        }

        InvokeRepeating("SpawnRepairKit", 0f, 1f);
        InvokeRepeating("UpdateProgressBar", 0f, 0.5f);

        // Starts mission countdown timer if no boss in the end
        if (!hasEndBoss)
            StartCoroutine(MissionTimeCountdown());
    }

    /// \brief Monitors the player's health and restarts the scene if necessary.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameOver();
        }

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

        if (elapsedTime >= missionTime && !hasEndBoss)
        {
            EndOfMission();
        }
    }

    /// \brief Coroutine to count down mission time and end the mission if time expires.
    private IEnumerator MissionTimeCountdown()
    {
        yield return new WaitForSeconds(missionTime);

        if (!this.hasEndBoss) EndOfMission();
    }

    /// \brief Spawns a repair kit from the upper edge of the screen.
    private void SpawnRepairKit()
    {
        int spawnRepairkit = Random.Range(0, repairkitDropRate);
        if (spawnRepairkit == 0)
        {
            if (GameObject.FindGameObjectsWithTag("RepairKit").Length < 1)
            {
                Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
                Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
                Vector3 upperEdge = Camera.main.ViewportToWorldPoint(Vector3.up);
                Vector3 where = new Vector3(Random.Range(leftEdge.x + 2, rightEdge.x - 2), upperEdge.y);
                Instantiate(repairkitPrefab, where, Quaternion.identity);
            }
        }
    }

    /// \brief Restarts the current active scene.
    public void RestartMission()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// \brief Stops all active game elements, such as background scrolling and enemy spawning.
    private void StopGame()
    {
        foreach (var backg in GameObject.FindGameObjectsWithTag("Background"))
        {
            backg.gameObject.GetComponent<BackgroundScroll>().beginScroll = false;
        }
        foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
        {
            invader.gameObject.SetActive(false);
        }
        foreach (var boss in GameObject.FindGameObjectsWithTag("Boss"))
        {
            boss.gameObject.SetActive(false);
        }
        foreach (var spawnp in GameObject.FindGameObjectsWithTag("SpawnPoints"))
        {
            spawnp.GetComponent<SpawnPoint>().turnOff();
        }

        CancelInvoke("UpdateProgressBar");
        CancelInvoke("SpawnRepairKit");
        player.gameObject.SetActive(false);
        infoUI.SetActive(false);
        endUI.SetActive(true);
    }

    /// \brief Triggers the game over state and restarts the scene.
    private void GameOver()
    {
        StopGame();

        GameObject.Find("levelText").GetComponent<Text>().text = "Level " + sceneIndex + " failed!";
        GameObject.Find("scoresText").GetComponent<Text>().text = "Scores: " + score;
        GameObject.Find("NextButton").SetActive(false);
    }

    /// \brief Sets the player's score and updates the score UI.
    /// \param score The new score to be set.
    private void SetScore(int score)
    {
        this.score = score;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore" + sceneIndex, highScore);
            if (highScoreIndicator != null) highScoreIndicator.text = highScore.ToString().PadLeft(4, '0');
            NewRecord();
        }
        if(scoreIndicator != null) scoreIndicator.text = score.ToString().PadLeft(4, '0');
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

    /// \brief Called when the player is killed. Decreases health and handles game over if necessary.
    public void OnPlayerKilled()
    {
        player.health = Mathf.Max(player.health - 1, 0);
        if(livesText != null) livesText.text = player.health.ToString();

        if (player.health > 0)
        {
            player.beUnkillable(1.0f);
            player.ActivateShieldBubble();
        }
        else
        {
            if (playerExplosion != null)
            {
                Instantiate(playerExplosion, player.transform.position, Quaternion.identity);
            }
            // If the player has no health, trigger game over
            GameOver();
        }
    }

    /// \brief Called when player picked up a repair kit. It heals the player.
    public void HealPlayer()
    {
        // Increase player's health and update the lives UI
        if (player.health < maxHealth)
        {
            player.health++;
            if(livesText != null) livesText.text = player.health.ToString();
        }
    }

    /// \brief Called when player picked up a weapon upgrade kit.
    /// \brief It switches the players gun template to the next.
    public void UpgradeWeapons()
    {
        int currentWpnIndex = player.currentTemplate;

        if (currentWpnIndex < player.upgradeTemplates.Count - 1)
        {
            // Increase speed
            player.speed += 1;
            currentWpnIndex++;
            // Deactivate old weapons
            foreach (var gun in player.guns)
            {
                gun.gameObject.SetActive(false);
                //Destroy(gun);

            }
            player.guns.Clear();

            // Activate new weapons from template
            player.currentTemplate = currentWpnIndex;
            foreach (var gun in player.upgradeTemplates[currentWpnIndex].guns)
            {
                Gun newGun = new GameObject(gun.name).AddComponent<Gun>();
                newGun.timeBetweenShoots = gun.timeBetweenShoots;
                newGun.projectilePrefab = gun.projectilePrefab;
                newGun.missileSpeed = gun.missileSpeed;
                newGun.pewPewSFX = gun.pewPewSFX;
                newGun.pewpewVolume = gun.pewpewVolume;
                newGun.layerIndex = LayerMask.NameToLayer("PlayerMissile");
                newGun.transform.SetParent(player.transform, false);
                newGun.transform.localPosition = gun.transform.localPosition;
                newGun.transform.transform.localRotation = gun.transform.localRotation;
                player.guns.Add(newGun);
            }
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
            if (invaderExplosion != null)
            {
                Instantiate(invaderExplosion, invader.transform.position, Quaternion.identity);
            }

            // Upon invader die, there is a chance of dropping an upgraded weapon
            int spawnUpgrade = Random.Range(0, upgradeDropRate);
            if (spawnUpgrade == 0 && GameObject.FindGameObjectsWithTag("Upgrade").Length < 1)
            {
                Instantiate(upgradePrefab, invader.transform.position, Quaternion.identity);
            }

            // Destroy the invader and update the player's score
            Destroy(invader.gameObject);

            // IDE animáció
            SetScore(score + invader.score);

            // If a boss destroyed than end the mission
            if (invader.gameObject.tag == "Boss")
            {
                // GAME END UI
                EndOfMission();
            }
        }
    }

    /// \brief Handle's the event when a mission end, such as: stopping spawnpoints, background scroll, etc.
    public void EndOfMission()
    {
        StopGame();

        PlayerPrefs.SetInt("Mission" + sceneIndex, score);

        if (sceneIndex >= SceneManager.sceneCountInBuildSettings - 1)
        {
            // Ha utolso mission volt
            GameObject.Find("NextButton").SetActive(false);
            GameObject.Find("levelText").GetComponent<Text>().text = "You win the game!";
            int totalScore = 0;
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                totalScore += PlayerPrefs.GetInt("Mission" + i);
            }
            GameObject.Find("scoresText").GetComponent<Text>().text = "Total score: " + totalScore;
        }
        else
        {
            GameObject.Find("levelText").GetComponent<Text>().text = "Level " + sceneIndex + " completed!";
            GameObject.Find("scoresText").GetComponent<Text>().text = "Scores: " + score;
        }
    }

    /// \brief Handle's the Exit button event
    public void ExitMission()
    {
        SceneManager.LoadScene(0);
    }

    /// \brief Handle's the Next mission button event
    public void NextMission()
    {
        int tmp = SceneManager.GetActiveScene().buildIndex;
        if (tmp + 1 < SceneManager.sceneCountInBuildSettings)
        {
            // Van még mission hátra
            tmp += 1;
            SceneManager.LoadScene(tmp);
        }
    }
}
