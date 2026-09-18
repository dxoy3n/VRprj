using UnityEngine;
using UnityEngine.InputSystem; // Thêm thư viện Input System mới

namespace vnc.FX
{
    public class CameraController : MonoBehaviour
    {
        public WaterCamera waterCamera;
        public float minHeight = 20f;
        public float speed = 6f;

        private void Update()
        {
            float direction = 0f;

            // Đọc phím theo New Input System
            if (Keyboard.current != null)
            {
                if (Keyboard.current.upArrowKey.isPressed) direction += 1f;
                if (Keyboard.current.downArrowKey.isPressed) direction -= 1f;
            }

            transform.position += Vector3.up * direction * speed * Time.deltaTime;

            if (waterCamera != null)
            {
                waterCamera.effectActive = transform.position.y < minHeight;
            }
        }
    }
}