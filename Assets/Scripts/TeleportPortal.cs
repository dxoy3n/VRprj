using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private float teleportDelay = 0.5f;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    private System.Collections.IEnumerator TeleportPlayer()
    {
        yield return new WaitForSeconds(teleportDelay);
        TeleportManager.instance.TeleportToScene(targetScene, spawnPosition);
    }
}