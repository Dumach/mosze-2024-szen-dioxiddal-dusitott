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
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;

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

        //NewGame();
    }

    private void Update()
    {
        if (lives <= 0 || Input.GetKeyDown(KeyCode.Return))
        {
            RestartScene();
        }
    }

    /*private void LoadSpawnPointsFromXML(string filepath)
    {
        // XML dokumentum bet�lt�se
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filepath);

        // �sszes "SpawnPoint" elem lek�r�se az XML f�jlb�l
        XmlNodeList spawnPointNodes = xmlDoc.SelectNodes("/SpawnPointsCollection/SpawnPoint");

        foreach (XmlNode node in spawnPointNodes)
        {
            // SpawnPoint neve az XML-ben
            string spawnPointName = node.Attributes["name"].Value;

            // SpawnPoint l�trehoz�sa
            GameObject spawnPoint = new GameObject(spawnPointName);
            DontDestroyOnLoad(spawnPoint);
            spawnPoint.tag = "SpawnPoints";
            spawnPoint.layer = LayerMask.NameToLayer("Invader");
            spawnPoint.AddComponent<Invaders>();
            Invaders settings = spawnPoint.GetComponent<Invaders>();

            // Adatok visszat�lt�se 
            settings.numberOf = int.Parse(node.SelectSingleNode("NumberOf").InnerText);
            settings.speed = float.Parse(node.SelectSingleNode("Speed").InnerText);
            settings.transform.position = StringToVector3(node.SelectSingleNode("Position").InnerText);
            settings.moveSpots = ParseVector3Array(node.SelectSingleNode("MoveSpots").InnerText);
            settings.startSpawningTime = float.Parse(node.SelectSingleNode("StartSpawningTime").InnerText);
            settings.waitTime = float.Parse(node.SelectSingleNode("WaitTime").InnerText);
            settings.timeBetweenShoots = float.Parse(node.SelectSingleNode("TimeBetweenShoots").InnerText);
            settings.missileSpeed = float.Parse(node.SelectSingleNode("MissileSpeed").InnerText);
        }
    }

    // Helper function: Vector3 t�mb parszol�sa a stringb�l
    private Vector3[] ParseVector3Array(string data)
    {
        string[] vectorStrings = data.Split(';');
        Vector3[] vectors = new Vector3[vectorStrings.Length];

        for (int i = 0; i < vectorStrings.Length; i++)
        {
            if (!string.IsNullOrEmpty(vectorStrings[i].Trim()))
            {
                vectors[i] = StringToVector3(vectorStrings[i]);
            }
        }

        return vectors;
    }

    // Helper function: Egy Vector3 sztringb�l val� parszol�sa
    private Vector3 StringToVector3(string sVector)
    {
        // Elt�vol�tjuk a z�r�jeleket �s vessz�vel sz�tv�lasztjuk a sz�mokat
        sVector = sVector.Trim(new char[] { '(', ')' });
        string[] sArray = sVector.Split(',');

        // Visszaadunk egy �j Vector3-at
        return new Vector3(
            float.Parse(sArray[0], CultureInfo.InvariantCulture),
            float.Parse(sArray[1], CultureInfo.InvariantCulture),
            float.Parse(sArray[2], CultureInfo.InvariantCulture));
    }*/

    /*private void SaveSpawnPointsToXML()
    {
        // �j XML dokumentum l�trehoz�sa
        XmlDocument xmlDoc = new XmlDocument();

        // Gy�k�r elem l�trehoz�sa
        XmlElement rootElement = xmlDoc.CreateElement("SpawnPointsCollection");
        xmlDoc.AppendChild(rootElement);

        // �sszes "SpawnPoints" tag� GameObject keres�se
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");


        foreach (GameObject spawnPoint in spawnPoints)
        {
            // Megpr�b�ljuk megtal�lni a SpawnPointSettings komponenst
            Invaders settings = spawnPoint.GetComponent<Invaders>();
            if (settings != null)
            {
                // �j SpawnPoint elem l�trehoz�sa
                XmlElement spElement = xmlDoc.CreateElement("SpawnPoint");
                spElement.SetAttribute("name", spawnPoint.name);

                // Adatok hozz�ad�sa az XML-hez
                //spElement.AppendChild(CreateElement(xmlDoc, "InvaderPrefab", settings.invaderPrefab != null ? settings.invaderPrefab.name : "null"));
                spElement.AppendChild(CreateElement(xmlDoc, "NumberOf", settings.numberOf.ToString()));
                spElement.AppendChild(CreateElement(xmlDoc, "Speed", settings.speed.ToString()));
                spElement.AppendChild(CreateElement(xmlDoc, "Position", settings.transform.position.ToString()));
                spElement.AppendChild(CreateElement(xmlDoc, "MoveSpots", Vec3ArrToStr(settings.moveSpots)));
                spElement.AppendChild(CreateElement(xmlDoc, "StartSpawningTime", settings.startSpawningTime.ToString()));
                spElement.AppendChild(CreateElement(xmlDoc, "WaitTime", settings.waitTime.ToString()));
                //spElement.AppendChild(CreateElement(xmlDoc, "MissilePrefab", settings.missilePrefab != null ? settings.missilePrefab.name : "null"));
                spElement.AppendChild(CreateElement(xmlDoc, "TimeBetweenShoots", settings.timeBetweenShoots.ToString()));
                spElement.AppendChild(CreateElement(xmlDoc, "MissileSpeed", settings.missileSpeed.ToString()));

                // Hozz�adjuk a spawnPoint elemet a gy�k�r elemhez
                rootElement.AppendChild(spElement);
            }
            else
            {
                Debug.LogWarning("nincsenek adatok");
            }

            // XML f�jl ment�se
            string sceneID = SceneManager.GetActiveScene().name;
            string filePath = Path.Combine(Application.dataPath, "Scenes","missiondata");
            filePath = Path.Combine(filePath, sceneID + ".xml");
            xmlDoc.Save(filePath);

            Debug.Log("SpawnPoints data has been saved to: " + filePath);
        }
    }

    // Helper function az XML elemek l�trehoz�s�hoz
    private XmlElement CreateElement(XmlDocument doc, string name, string value)
    {
        XmlElement element = doc.CreateElement(name);
        element.InnerText = value;
        return element;
    }

    // Helper function a Vector3 t�mb stringg� alak�t�s�hoz
    private string Vec3ArrToStr(Vector3[] vectors)
    {
        string result = "";
        foreach (Vector3 v in vectors)
        {
            result += v.ToString() + "; ";
        }
        return result.TrimEnd(' ', ';');
    }*/

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

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = this.lives.ToString();
    }

    public void OnPlayerKilled(Player player)
    {
        SetLives(lives - 1);

        if (lives > 0)
        {
            player.beUnkillable(1);
        }
        else
        {
            player.gameObject.SetActive(false);
            GameOver();
        }
    }

    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        Destroy(invader.gameObject);
        // TODO fegyver lassu mikor meghal
        SetScore(score + invader.score);
    }

    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
    }
}
