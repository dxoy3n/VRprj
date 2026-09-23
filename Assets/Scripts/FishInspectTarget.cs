using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem; // Thư viện Input System mới

public class FishInspectTarget : MonoBehaviour
{
    [Header("Cấu hình UI & Âm thanh")]
    public GameObject infoPanel;        // Canvas Panel thông tin
    public AudioSource audioSource;     // AudioSource phát tiếng
    public AudioClip fishInfoAudio;     // File ghi âm thông tin cá

    [Header("Khoảng cách xuất hiện trước mặt người chơi")]
    public float distanceFromCamera = 1f; // Cách người chơi 1.5 mét
    public float moveSpeed = 3.0f;          // Tốc độ cá di chuyển lại gần

    [Header("Tham chiếu Component")]
    public RandomCaBoi fishSwimScript;

    private Transform mainCameraTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isInspecting = false;
    private Coroutine currentMoveCoroutine;

    void Start()
    {
        // Tự động tìm Camera chính của người chơi (XR Origin Camera)
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }

        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    void Update()
    {
        // Làm Panel luôn xoay về phía người chơi
        if (isInspecting && infoPanel != null && mainCameraTransform != null)
        {
            infoPanel.transform.LookAt(infoPanel.transform.position + mainCameraTransform.rotation * Vector3.forward,
                                       mainCameraTransform.rotation * Vector3.up);
        }

        // 1. Test bấm phím SPACE
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleInspect();
        }

        // 2. Test CLICK CHUỘT TRÁI vào con cá (Dành riêng cho Input System mới)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckMouseClickOnFish();
        }
    }

    // Hàm kiểm tra xem con trỏ chuột có đang nhấp vào con cá này không
    private void CheckMouseClickOnFish()
    {
        if (Camera.main == null) return;

        // Bắn 1 tia Raycast từ vị trí con trỏ chuột vào không gian 3D
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Nếu vị trí click trúng chính con cá này (hoặc con của nó)
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                ToggleInspect();
            }
        }
    }

    // Hàm gọi khi bấm/chạm vào cá (Dùng được cho cả phím/chuột lẫn XR Simple Interactable)
    public void ToggleInspect()
    {
        isInspecting = !isInspecting;

        if (currentMoveCoroutine != null)
            StopCoroutine(currentMoveCoroutine);

        if (isInspecting)
        {
            // 1. Tắt script bơi tự do
            if (fishSwimScript != null) fishSwimScript.enabled = false;

            // 2. Lưu lại vị trí & góc quay cũ của cá
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            // 3. Tính vị trí ngay trước mặt người chơi
            Vector3 targetInspectPos = mainCameraTransform.position + (mainCameraTransform.forward * distanceFromCamera);

            // 4. Di chuyển cá tới trước mặt người chơi & mở Panel
            currentMoveCoroutine = StartCoroutine(MoveToTarget(targetInspectPos, Quaternion.LookRotation(-mainCameraTransform.forward), true));
        }
        else
        {
            // Đóng Panel & đưa cá về vị trí cũ
            CloseInspect();
        }
    }

    public void CloseInspect()
    {
        isInspecting = false;
        if (infoPanel != null) infoPanel.SetActive(false);
        if (audioSource != null) audioSource.Stop();

        if (currentMoveCoroutine != null)
            StopCoroutine(currentMoveCoroutine);

        // Di chuyển cá về vị trí bơi cũ rồi bật lại script bơi
        currentMoveCoroutine = StartCoroutine(MoveToTarget(originalPosition, originalRotation, false));
    }

    // Hàm phát loa thông tin
    public void PlayAudioInfo()
    {
        if (audioSource != null && fishInfoAudio != null)
        {
            audioSource.PlayOneShot(fishInfoAudio);
        }
    }

    private IEnumerator MoveToTarget(Vector3 targetPos, Quaternion targetRot, bool showPanelAtEnd)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * moveSpeed);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        if (showPanelAtEnd)
        {
            if (infoPanel != null) infoPanel.SetActive(true);
        }
        else
        {
            // Khi đã về vị trí cũ, bật lại script bơi
            if (fishSwimScript != null) fishSwimScript.enabled = true;
        }
    }
}