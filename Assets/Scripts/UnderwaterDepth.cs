using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Dùng để bật/tắt ScriptableRendererFeature nếu Wobble là Full Screen Pass

public class UnderwaterDepth : MonoBehaviour
{
    [Header("Depth Settings")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float depth = 8f;

    [Header("Đổi Profile trực tiếp (Cần 1 Volume + 2 Profile Assets)")]
    [SerializeField] private Volume targetVolume;
    [SerializeField] private VolumeProfile surfaceProfile;
    [SerializeField] private VolumeProfile underwaterProfile;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioClip surfaceAmbience;    // Tiếng sóng biển trên bờ
    [SerializeField] private AudioClip underwaterAmbience; // Tiếng ọc ọc / ù ù dưới nước

    [Header("Screen Wobble (Full Screen Pass)")]
    [Tooltip("Thả Full Screen Pass Renderer Feature vào đây (nếu dùng Feature), nếu Wobble nằm trong Volume Profile thì để trống")]
    [SerializeField] private ScriptableRendererFeature wobbleRendererFeature;

    private bool isUnderwater;

    private void Update()
    {
        if (mainCamera == null) return;

        // Kiểm tra vị trí camera so với mực nước
        isUnderwater = mainCamera.position.y < depth;

        // === GÁN PROFILE TRỰC TIẾP ===
        if (targetVolume != null)
        {
            VolumeProfile selectedProfile = isUnderwater ? underwaterProfile : surfaceProfile;
            
            if (targetVolume.profile != selectedProfile)
            {
                targetVolume.profile = selectedProfile;
                RenderSettings.fog = isUnderwater;

                // --- 1. ĐỔI ÂM THANH (AUDIO) ---
                if (ambientAudioSource != null)
                {
                    AudioClip targetClip = isUnderwater ? underwaterAmbience : surfaceAmbience;
                    if (targetClip != null)
                    {
                        ambientAudioSource.clip = targetClip;
                        ambientAudioSource.loop = true;
                        ambientAudioSource.Play();
                    }
                    else
                    {
                        ambientAudioSource.Stop();
                    }
                }

                // --- 2. BẬT/TẮT HIỆU ỨNG WOBBLE  ---
                if (wobbleRendererFeature != null)
                {
                    wobbleRendererFeature.SetActive(isUnderwater);
                }
            }
        }
    }
}