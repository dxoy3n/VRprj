using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager instance;
    public Vector3 spawnPosition = Vector3.zero;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TeleportToScene(string sceneName, Vector3 newSpawnPos)
    {
        spawnPosition = newSpawnPos;
        SceneManager.LoadScene(sceneName);
    }
}