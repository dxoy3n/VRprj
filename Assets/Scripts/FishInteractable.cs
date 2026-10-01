using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class FishInteractable : MonoBehaviour
{
    [Header("1. Dữ liệu riêng con cá này")]
    public Sprite fishSprite;           // Kéo ảnh riêng của con cá này vào đây

    [Header("2. UI Dùng Chung (Kéo cùng 1 Panel cho cả 16 con)")]
    public GameObject infoPanel;        // Panel UI chung
    public Image panelImageHolder;      // Khung chứa ảnh trên Panel chung
    public Button closeButton;          // Nút Đóng trên Panel chung

    [Header("3. Cấu hình di chuyển")]
    public float distanceFromCamera = 1.3f;
    public float heightOffset = 0.3f;
    public float moveDuration = 0.8f;   // Thời gian cá bay tới mặt camera (0.8 giây)
    public bool lookSideWays = true;

    [Header("4. Script bơi tự động")]
    public MonoBehaviour fishSwimScript; // Script bơi (RandomCaBoi / FishSwim)

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isInspecting = false;
    private Coroutine currentMoveCoroutine;

    void Start()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    void Update()
    {
        Camera cam = Camera.main;

        // Xoay Panel UI hướng về phía Camera
        if (isInspecting && infoPanel != null && cam != null)
        {
            infoPanel.transform.LookAt(infoPanel.transform.position + cam.transform.rotation * Vector3.forward,
                                       cam.transform.rotation * Vector3.up);
        }

        // Bắt click chuột trái
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckMouseClickOnFish();
        }
    }

    private void CheckMouseClickOnFish()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Bắn tia Raycast từ vị trí con trỏ chuột
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Kiểm tra click trúng con cá hoặc object con chứa Collider của cá
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                ToggleInspect();
            }
        }
    }

    public void ToggleInspect()
    {
        isInspecting = !isInspecting;

        if (currentMoveCoroutine != null)
            StopCoroutine(currentMoveCoroutine);

        Camera cam = Camera.main;

        if (isInspecting && cam != null)
        {
            // Tắt bơi tự động khi đang xem cá
            if (fishSwimScript != null) fishSwimScript.enabled = false;

            originalPosition = transform.position;
            originalRotation = transform.rotation;

            Vector3 targetInspectPos = cam.transform.position + (cam.transform.forward * distanceFromCamera);
            targetInspectPos.y += heightOffset;

            Vector3 lookDir = -cam.transform.forward;
            lookDir.y = 0;
            if (lookDir == Vector3.zero) lookDir = Vector3.forward;

            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            if (lookSideWays)
            {
                targetRotation *= Quaternion.Euler(0, 90, 0);
            }

            currentMoveCoroutine = StartCoroutine(MoveToTarget(targetInspectPos, targetRotation, true));
        }
        else
        {
            CloseInspect();
        }
    }

    public void CloseInspect()
    {
        isInspecting = false;

        if (infoPanel != null) infoPanel.SetActive(false);

        if (currentMoveCoroutine != null)
            StopCoroutine(currentMoveCoroutine);

        currentMoveCoroutine = StartCoroutine(MoveToTarget(originalPosition, originalRotation, false));
    }

    private IEnumerator MoveToTarget(Vector3 targetPos, Quaternion targetRot, bool showPanelAtEnd)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsedTime = 0f;

        // Di chuyển mượt mà dựa trên thời gian cố định (không lo kẹt vòng lặp)
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            // Sử dụng SmoothStep giúp di chuyển mượt ở 2 đầu
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, smoothT);
            yield return null;
        }

        // Đảm bảo vị trí và góc xoay chính xác khi kết thúc
        transform.position = targetPos;
        transform.rotation = targetRot;

        if (showPanelAtEnd)
        {
            // BẬT PANEL VÀ ĐỔI CẢNH (ĐẢM BẢO CHẠY 100%)
            if (panelImageHolder != null && fishSprite != null)
            {
                panelImageHolder.sprite = fishSprite;
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(CloseInspect);
            }

            if (infoPanel != null)
            {
                infoPanel.SetActive(true);
            }
        }
        else
        {
            // Kích hoạt lại script bơi
            if (fishSwimScript != null)
            {
                fishSwimScript.enabled = true;
                fishSwimScript.CancelInvoke();
                fishSwimScript.Invoke("Start", 0.05f);
                fishSwimScript.SendMessage("OnEnable", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}