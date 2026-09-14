using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        if (TeleportManager.instance != null)
        {
            transform.position = TeleportManager.instance.spawnPosition;
        }
    }
}