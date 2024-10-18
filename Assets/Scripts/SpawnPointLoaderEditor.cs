#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SpawnPointLoaderEditor
{
    [MenuItem("Tools/Load SpawnPoints from XML")]
    public static void LoadSpawnPoints()
    {
        // Keresünk egy SpawnPointLoader komponenst a jelenetben
        SpawnPointLoader loader = GameObject.FindObjectOfType<SpawnPointLoader>();
        if (loader != null)
        {
            // XML elérési út beállítása (itt be kell állítani a fájl elérési útját)
            string filePath = "Assets/Scenes/MissionData/Mission1.xml"; // Példa fájlútvonal
            loader.LoadSpawnPointsFromXML(filePath);
        }
        else
        {
            Debug.LogError("No SpawnPointLoader found in the scene.");
        }
    }
}
#endif
