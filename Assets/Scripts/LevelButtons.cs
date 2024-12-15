using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    public int levelNumber;   // Szint száma
    public Image lockIcon;    // Lock ikon Image komponens
    private Button button;     // Gomb referencia

    void Start()
    {
        // Ellenőrizzük, hogy a gomb és a lockIcon nem null
        button = GetComponent<Button>();

        // Ellenőrizzük, hogy a button és lockIcon nem null
        if (button == null)
        {
            Debug.LogError("Button referencia nem található a " + gameObject.name + " objektumon!");
            return;
        }

        if (lockIcon == null)
        {
            Debug.LogError("LockIcon referencia nem található a " + gameObject.name + " objektumon!");
            return;
        }

        // Megkeressük, hogy a szint feloldott-e
        bool isUnlocked = FindObjectOfType<LevelManager>().IsLevelUnlocked(levelNumber);

        if (isUnlocked)
        {
            button.interactable = true;      // A gomb működőképessé tétele
            lockIcon.gameObject.SetActive(false);  // Lakat elrejtése
        }
        else
        {
            button.interactable = false;     // A gomb letiltása
            lockIcon.gameObject.SetActive(true);   // Lakat megjelenítése
        }

        // Hozzáadjuk az eseményt a gombhoz
        button.onClick.AddListener(OnLevelButtonClick);
    }

    // A szint indítása gombra kattintva
    public void OnLevelButtonClick()
    {
        SceneManager.LoadScene(levelNumber + 1);  // Betöltjük a szintet
    }
}
