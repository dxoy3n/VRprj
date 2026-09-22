using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleTeleportController : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Scene_Room1";
    [SerializeField] private string prevSceneName = "Scene_Room2";

    private float teleportCooldown = 0f;
    private const float COOLDOWN_TIME = 1f;

    private void Update()
    {
        teleportCooldown -= Time.deltaTime;

        // XR Device Simulator - Test bằng keyboard
        if (Input.GetKeyDown(KeyCode.T)) // E = tay phải
        {
            if (teleportCooldown <= 0)
            {
                TeleportToNextScene();
                teleportCooldown = COOLDOWN_TIME;
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) // Q = tay trái
        {
            if (teleportCooldown <= 0)
            {
                TeleportToPreviousScene();
                teleportCooldown = COOLDOWN_TIME;
            }
        }
    }

    private void TeleportToNextScene()
    {
        Debug.Log($"🔵 Teleporting to: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }

    private void TeleportToPreviousScene()
    {
        Debug.Log($"🔴 Teleporting to: {prevSceneName}");
        SceneManager.LoadScene(prevSceneName);
    }
}