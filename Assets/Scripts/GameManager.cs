using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
using System.IO;
using System.Globalization;


[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //[SerializeField] private GameObject gameOverUI;
    //[SerializeField] private GameObject mainMenuUI;
    //[SerializeField] private GameObject PauseUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private Player player;

    public int score { get; private set; } = 0;

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
        //NewGame();
    }

    private void Update()
    {
        if (player.health <= 0 || Input.GetKeyDown(KeyCode.Return))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameOver()
    {
        RestartScene();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    public void OnPlayerKilled(Player player)
    {
        player.health = Mathf.Max(player.health - 1, 0);
        livesText.text = player.health.ToString();

        if (player.health > 0)
        {
            player.beUnkillable(1.5f);
        }
        else
        {
            player.gameObject.SetActive(false);
            GameOver();
        }
    }

    public void OnInvaderKilled(Invader invader)
    {
        invader.health = Mathf.Max(invader.health - 1, 0);
        if (invader.health <= 0)
        {
        // TODO fegyver lassu mikor meghal
        //invader.gameObject.SetActive(false);
        Destroy(invader.gameObject);
        SetScore(score + invader.score);
        }
    }

    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
    }
}
