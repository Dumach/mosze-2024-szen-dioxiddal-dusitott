using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{

    // Az aktuális level
    public int currentLevel;

    // Időlimit, ha nincs boss
    public float levelTimeLimit = 140f;

    // Van-e boss ezen a pályán
    public bool hasBoss = false;

    // Szint befejezése követése
    private bool levelCompleted = false;

    // Időzítő a pálya követéséhez
    private float timer; 

    void Start()
    {
        timer = 0f;

        // Ellenőrizzük, hogy a szint fel van-e oldva
        if (!IsLevelUnlocked(currentLevel))
        {
            Debug.LogError("Ez a szint nincs feloldva!");
        }
    }

    void Update()
    {
        if (levelCompleted) return; // Ha a szint már befejezett, nem kell tovább figyelni

        // Ha nincs boss, az idő alapján vége a pályának
        if (!hasBoss)
        {
            timer += Time.deltaTime;
            if (timer >= levelTimeLimit)
            {
                CompleteLevel();
            }
        }
    }

    // Hívás, ha a boss legyőzve
    public void BossDefeated()
    {
        if (hasBoss && !levelCompleted)
        {
            CompleteLevel();
        }
    }

    // Szint befejezésének logikája
    void CompleteLevel()
    {
        levelCompleted = true;
        Debug.Log("Szint teljesítve: " + currentLevel);

        // Következő szint feloldása
        UnlockLevel(currentLevel + 1);
    }

    // Szint feloldásának beállítása
    public void UnlockLevel(int level)
    {
        PlayerPrefs.SetInt($"Level_{level}_Unlock", 1);
        Debug.Log("Feloldva: Szint " + level);
    }

    // Ellenőrizhető, hogy egy szint fel van-e oldva
    public bool IsLevelUnlocked(int level)
    {
        return PlayerPrefs.GetInt($"Level_{level}_Unlock", 0) == 1;
    }
}
