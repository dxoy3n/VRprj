using UnityEngine;
using UnityEngine.Rendering;

namespace vnc.FX
{
    public class WaterCamera : MonoBehaviour
    {
        [Header("Wobble Shader Effect")]
        [Tooltip("Gán Material xài Shader WobbleFx vào đây")]
        public Material Wobble;

        [Header("URP Volume Settings")]
        [Tooltip("Gán GameObject UnderwaterVolume vào đây")]
        public Volume underwaterVolume;

        [Tooltip("Tốc độ chuyển đổi hiệu ứng khi chìm/nổi")]
        public float transitionSpeed = 5f;

        [HideInInspector] public bool effectActive;

        private void Update()
        {
            // 1. Quản lý Weight của Volume dưới nước
            if (underwaterVolume != null)
            {
                float targetWeight = effectActive ? 1f : 0f;
                underwaterVolume.weight = Mathf.Lerp(underwaterVolume.weight, targetWeight, Time.deltaTime * transitionSpeed);
            }

            // 2. Kích hoạt hiệu ứng Wobble trên Material
            if (Wobble != null)
            {
                // Truyền trạng thái bật/tắt hoặc điều khiển thuộc tính Material ở đây nếu cần
            }
        }
    }
}