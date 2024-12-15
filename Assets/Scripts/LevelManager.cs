using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const int totalLevels = 9;
    void Start()
    {
        if (!PlayerPrefs.HasKey("Level1"))
        {
            // Az 1. szint elérhető
            PlayerPrefs.SetInt("Level1", 1);
            for (int i = 2; i <= totalLevels; i++)
            {
                // Többi szint zárolva
                PlayerPrefs.SetInt("Level" + i, 0);
            }
        }
    }

    // Ellenőrizzük, hogy a szint elérhető-e
    public bool IsLevelUnlocked(int level)
    {
        return PlayerPrefs.GetInt("Level" + level) == 1;
    }

    // Szint feloldása
    public void UnlockLevel(int level)
    {
        PlayerPrefs.SetInt("Level" + level, 1);
    }
}
