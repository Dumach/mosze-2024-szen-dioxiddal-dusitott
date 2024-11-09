using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    [Header("Palya Kezdete és Vege")]
    public Transform startPoint; // Pálya kezdőpontja
    public Transform endPoint;   // Pálya végpontja

    [Header("Haladasjelzo")]
    public Image progressBarFill; // Haladásjelző sáv kitöltése

    [Header("Jatekos")]
    public Transform player; // Játékos pozíciója

    private float totalDistance;

    private void Start()
    {
        // A teljes távolság kiszámítása a pálya elejétől a végéig
        totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
        InvokeRepeating("Fill", 0f, 1f);
    }

    private void Update()
    {

    }

    private void Fill()
    {
        // Játékos távolságának kiszámítása a pálya kezdőpontjától
        float playerDistance = Vector3.Distance(startPoint.position, player.position);

        // Haladás százalékos aránya 0 és 1 között
        float progress = Mathf.Clamp01(playerDistance / totalDistance);

        // A haladásjelző csík kitöltésének frissítése
        if (progressBarFill != null)
        {
            progressBarFill.fillAmount = progress;
        }
    }

    public void TurnOff()
    {
        CancelInvoke("Fill");
    }
}
