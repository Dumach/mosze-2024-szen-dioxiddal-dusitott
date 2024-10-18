#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SpawnPointLoaderEditor
{
    [MenuItem("Tools/Load SpawnPoints from XML")]
    public static void LoadSpawnPoints()
    {
        // Keres�nk egy SpawnPointLoader komponenst a jelenetben
        SpawnPointLoader loader = GameObject.FindObjectOfType<SpawnPointLoader>();
        if (loader != null)
        {
            // XML el�r�si �t be�ll�t�sa (itt be kell �ll�tani a f�jl el�r�si �tj�t)
            string filePath = "Assets/Scenes/MissionData/Mission1.xml"; // P�lda f�jl�tvonal
            loader.LoadSpawnPointsFromXML(filePath);
        }
        else
        {
            Debug.LogError("No SpawnPointLoader found in the scene.");
        }
    }
}
#endif
