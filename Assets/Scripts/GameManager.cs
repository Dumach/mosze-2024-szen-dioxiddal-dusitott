using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Xml.Linq;

/// \class GameManager
/// \brief This class is responsible for controlling and managing activities in the game
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{

    /// \brief Singleton instance of the GameManager.
    public static GameManager Instance { get; private set; }

    /// \brief Flag to determine if there's an end boss in the mission.
    [SerializeField] private bool hasEndBoss;
    [SerializeField] private bool isLastMission = false;

    /// \brief Mission time in seconds.
    public float missionTime;

    /// \brief Explosions
    public GameObject invaderExplosion;
    public GameObject playerExplosion;

    /// \brief Sounds
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private float gameOverVolume = 1.0f;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private float enemyDeathVolume = 1.0f;

    /// \brief Prefab for upgrade items dropped by enemies.
    [SerializeField] private Upgrade upgradePrefab;

    /// \brief Frequency at which upgrade items are dropped.
    [SerializeField] private int upgradeDropRate;

    /// \brief Prefab for repair kits dropped by enemies.
    [SerializeField] private Repair repairkitPrefab;

    /// \brief Frequency at which repair kits are dropped.
    [SerializeField] private int repairkitDropRate;


    [SerializeField] private int maxCount = 7;

    /// \brief Reference to the Player object in the game.
    private Player player;

    /// \brief Reference to the UI manager object that controls the UI.
    private UiManager uiManager;

    /// \brief The maximum health of the player.
    private int maxHealth;

    /// \brief Name of the player.
    private string playerName;

    /// \brief The current score of the player.
    public int score { get; private set; } = 0;

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
        uiManager = FindObjectOfType<UiManager>().GetComponent<UiManager>();
        player = FindObjectOfType<Player>();
        maxHealth = player.health;
        if (PlayerPrefs.HasKey("Score"))
            score = PlayerPrefs.GetInt("Score");
        if (PlayerPrefs.HasKey("Name"))
            playerName = PlayerPrefs.GetString("Name");

        InvokeRepeating("SpawnRepairKit", 0f, 1f);

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
    }

    /// \brief Coroutine to count down mission time and end the mission if time expires.
    private IEnumerator MissionTimeCountdown()
    {
        yield return new WaitForSeconds(missionTime);

        if (!this.hasEndBoss)
        {
            EndOfMission();
        }
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
        HandleHighScore();
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

        uiManager.StopGameUI();

        CancelInvoke("SpawnRepairKit");
        player.gameObject.SetActive(false);
    }

    /// \brief Triggers the game over state and restarts the scene.
    private void GameOver()
    {
        StopGame();

        // score
        uiManager.HandleGameOverUI(score);

        var nextbtn = GameObject.Find("NextButton");
        if (nextbtn) nextbtn.SetActive(false);
    }

    /// \brief Sets the player's score and updates the score UI.
    /// \param score The new score to be set.
    private void SetScore(int score)
    {
        this.score = score;
        uiManager.UpdateScoreUI(score);
    }

    /// \brief Called when the player is killed. Decreases health and handles game over if necessary.
    public void OnPlayerKilled()
    {
        int health = Mathf.Max(player.health - 1, 0);
        player.health = health;
        uiManager.UpdatePlayerHealthUI(health);

        if (health > 0)
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

            // Play the gameover sound if it's assigned
            OnGameOverSounds();

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
            if (uiManager != null) uiManager.UpdatePlayerHealthUI(player.health);
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

            // Play the death sound if it's assigned
            if (enemyDeathSound != null)
            {
                GameObject sfxPlayer = GameObject.Find("SFXPlayer");
                AudioSource aud = sfxPlayer.GetComponent<AudioSource>();
                aud.PlayOneShot(enemyDeathSound, enemyDeathVolume);
            }

            // Upon invader die, there is a chance of dropping an upgraded weapon
            int spawnUpgrade = Random.Range(0, upgradeDropRate);
            if (spawnUpgrade == 0 && GameObject.FindGameObjectsWithTag("Upgrade").Length < 1)
            {
                if (upgradePrefab != null)
                    Instantiate(upgradePrefab, invader.transform.position, Quaternion.identity);
            }

            // Destroy the invader and update the player's score
            Destroy(invader.gameObject);

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
        PlayerPrefs.SetInt("Score", score);

        // Ha utolso mission volt
        if (isLastMission)
        {
            LoadEndScene();
            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
            uiManager.HandleEndOfMissionUI(true, score);
        }
        else
        {
            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
            uiManager.HandleEndOfMissionUI(false, score);
        }
    }

    /// \brief Handle's the Exit button event
    public void ExitMission()
    {
        HandleHighScore();
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }

    /// \brief Handle's the Next mission button event
    public void NextMission()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;        
        HandleHighScore();
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings && !isLastMission)
        {
            // Van meg mission hatra
            currentSceneIndex += 1;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    private void HandleHighScore()
    {
        HighscoreElement element = new HighscoreElement(playerName, score);
        List<HighscoreElement> highscoreList = new List<HighscoreElement>();
        highscoreList = FileHandler.ReadListFromJSON<HighscoreElement>("scores.json");

        while (highscoreList.Count > maxCount)
        {
            highscoreList.RemoveAt(maxCount);
        }

        for (int i = 0; i < maxCount; i++)
        {
            if (i >= highscoreList.Count || element.points > highscoreList[i].points)
            {
                // add new high score
                highscoreList.Insert(i, element);

                while (highscoreList.Count > maxCount)
                {
                    highscoreList.RemoveAt(maxCount);
                }

                FileHandler.SaveToJSON<HighscoreElement>(highscoreList, "scores.json");

                break;
            }
        }
    }

    public void OnGameOverSounds()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.clip != gameOverSound)
            {
                audioSource.Stop();
            }
        }

        AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position, gameOverVolume);

    }
    private void LoadEndScene()
    {
        SceneManager.LoadScene("EndCredit");
    }
}
