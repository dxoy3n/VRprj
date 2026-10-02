using UnityEngine;

public class KeyEffects : MonoBehaviour
{
    [Header("Xoay chìa khóa")]
    public float rotateSpeed = 90f;

    [Header("Âm thanh hiệu ứng")]
    public AudioClip spawnSound;
    private AudioSource audioSource;

    [Header("Tự động biến mất")]
    public float destroyDelay = 5f;

    void OnEnable()
    {
        // Hủy các lệnh hẹn giờ cũ nếu có để tránh bị biến mất sai thời điểm
        CancelInvoke("HideKey");

        // 1. Phát âm thanh
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
        else
        {
            PlayProceduralTingSound();
        }

        // 2. Bắt đầu đếm ngược đúng 5s từ thời điểm BẬT chìa khóa lên
        Invoke("HideKey", destroyDelay);
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    void HideKey()
    {
        gameObject.SetActive(false);
    }

    void PlayProceduralTingSound()
    {
        int sampleRate = 44100;
        float frequency = 1760f;
        float duration = 0.4f;
        int sampleCount = (int)(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * t) * Mathf.Exp(-12f * t);
        }

        AudioClip tingClip = AudioClip.Create("ProceduralTing", sampleCount, 1, sampleRate, false);
        tingClip.SetData(samples, 0);
        audioSource.PlayOneShot(tingClip);
    }
}