using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;

    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();

        EnterMainMenu();
    }

    private void Update()
    {
        // Exit from existing game
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // Exit to Menu
            if (lives >= 0 && PauseUI.activeSelf)
            {
                GameOver();
                SetLives(0);
            }
            // Pause game
            if(lives > 0 && !PauseUI.activeSelf && !mainMenuUI.activeSelf)
            {
                PauseOrResume();
            }
        }

        // Start new Game
        if(Input.GetKeyUp(KeyCode.Return))
        {
            // Start new Game
            if (lives <= 0 || mainMenuUI.activeSelf)
            {
                NewGame();
            }
            // Resume a game
            if(PauseUI.activeSelf)
            {
                PauseOrResume();
            }
        }
    }

    private void PauseOrResume()
    {
        PauseUI.SetActive(!PauseUI.activeSelf);
        invaders.gameObject.SetActive(!invaders.isActiveAndEnabled);
        mysteryShip.gameObject.SetActive(!mysteryShip.isActiveAndEnabled);
        player.gameObject.SetActive(!player.isActiveAndEnabled);
    }

    private void EnterMainMenu()
    {
        mainMenuUI.SetActive(true);
        gameOverUI.SetActive(false);
        PauseUI.SetActive(false);
        invaders.gameObject.SetActive(false);
        mysteryShip.gameObject.SetActive(false);
    }

    private void NewGame()
    {
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        for (int i = 0; i < bunkers.Length; i++) {
            bunkers[i].ResetBunker();
        }

        Respawn();
    }

    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        PauseUI.SetActive(false);
        invaders.gameObject.SetActive(false);
        mysteryShip.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = this.lives.ToString();
    }

    public void OnPlayerKilled(Player player)
    {
        SetLives(lives - 1);

        player.gameObject.SetActive(false);

        if (lives > 0) {
            Invoke(nameof(NewRound), 1f);
        } else {
            GameOver();
        }
    }

    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);

        SetScore(score + invader.score);

        if (invaders.GetAliveCount() == 0) {
            NewRound();
        }
    }

    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
    }

    public void OnBoundaryReached()
    {
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);
            OnPlayerKilled(player);
        }
    }

}
