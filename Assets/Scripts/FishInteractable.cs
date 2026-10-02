using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class FishInteractable : MonoBehaviour
{
    [Header("1. Dữ liệu riêng con cá này")]
    public Sprite fishSprite;           // Ảnh riêng của con cá
    public AudioClip fishInfoAudio;     // File ghi âm thông tin con cá này

    [Header("2. UI Dùng Chung (Kéo cùng 1 Panel cho cả 16 con)")]
    public GameObject infoPanel;        // Panel UI chung
    public Image panelImageHolder;      // Khung chứa ảnh trên Panel chung
    public Button closeButton;          // Nút Đóng trên Panel chung
    public Button audioButton;          // Nút Icon Loa trên Panel chung
    public AudioSource audioSource;     // Component AudioSource dùng để phát âm thanh

    [Header("3. Cấu hình di chuyển & Vị trí")]
    public float distanceFromCamera = 1.3f;
    public float heightOffset = 0.3f;
    public float panelOffsetX = 0.55f;      // Panel nằm lệch sang PHẢI cá bao nhiêu (mét)
    public float panelOffsetY = 0.1f;       // Panel nằm lệch lên TRÊN cá bao nhiêu (mét)
    public float moveDuration = 0.8f;       // Thời gian cá bay tới mặt camera (giây)
    public bool lookSideWays = true;

    [Header("4. Tham chiếu Component")]
    public MonoBehaviour fishSwimScript; // Script bơi tự động (RandomCaBoi)

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
        if (isInspecting && infoPanel != null && infoPanel.activeSelf && cam != null)
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

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
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

        // Tắt voice ngay khi đóng Panel
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (infoPanel != null) infoPanel.SetActive(false);

        if (currentMoveCoroutine != null)
            StopCoroutine(currentMoveCoroutine);

        currentMoveCoroutine = StartCoroutine(MoveToTarget(originalPosition, originalRotation, false));
    }

    public void PlayAudioInfo()
    {
        if (audioSource != null && fishInfoAudio != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(fishInfoAudio);
        }
    }

    private IEnumerator MoveToTarget(Vector3 targetPos, Quaternion targetRot, bool showPanelAtEnd)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, smoothT);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        if (showPanelAtEnd)
        {
            Camera cam = Camera.main;

            if (infoPanel != null && cam != null)
            {
                // Đặt Panel bên cạnh cá
                Vector3 panelPos = targetPos + (cam.transform.right * panelOffsetX) + (cam.transform.up * panelOffsetY);
                infoPanel.transform.position = panelPos;

                infoPanel.transform.LookAt(infoPanel.transform.position + cam.transform.rotation * Vector3.forward,
                                           cam.transform.rotation * Vector3.up);

                // Gán ảnh riêng
                if (panelImageHolder != null && fishSprite != null)
                {
                    panelImageHolder.sprite = fishSprite;
                }

                // Gán sự kiện Nút Đóng
                if (closeButton != null)
                {
                    closeButton.onClick.RemoveAllListeners();
                    closeButton.onClick.AddListener(CloseInspect);
                }

                // Gán sự kiện Nút Loa
                if (audioButton != null)
                {
                    audioButton.onClick.RemoveAllListeners();
                    audioButton.onClick.AddListener(PlayAudioInfo);
                }

                infoPanel.SetActive(true);
            }
        }
        else
        {
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