using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class FishInspectTarget : MonoBehaviour
{
    [Header("Cấu hình UI & Âm thanh")]
    public GameObject infoPanel;        // Canvas Panel thông tin
    public AudioSource audioSource;     // AudioSource phát tiếng
    public AudioClip fishInfoAudio;     // File ghi âm thông tin cá

    [Header("Khoảng cách & Góc quay")]
    [Tooltip("Khoảng cách cá xuất hiện trước mặt người chơi")]
    public float distanceFromCamera = 1.3f;

    [Tooltip("Độ cao cộng thêm so với Camera (Căng chỉnh nếu cá bị chui xuống đất)")]
    public float heightOffset = 0.3f; // Cộng thêm 0.3m cho cá nâng lên vừa tầm mắt
    public float moveSpeed = 3.0f;

    [Tooltip("Tích chọn để cá xoay thân ngang nghiêng 90 độ")]
    public bool lookSideWays = true;

    [Header("Tham chiếu Component")]
    public RandomCaBoi fishSwimScript;
    private Transform mainCameraTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isInspecting = false;
    private Coroutine currentMoveCoroutine;

    void Start()
    {
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }

        if (infoPanel != null)
            infoPanel.SetActive(false);
    }



    void Update()
    {

        if (isInspecting && infoPanel != null && mainCameraTransform != null)
        {
            infoPanel.transform.LookAt(infoPanel.transform.position + mainCameraTransform.rotation * Vector3.forward,
                                       mainCameraTransform.rotation * Vector3.up);
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleInspect();
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckMouseClickOnFish();
        }
    }

    private void CheckMouseClickOnFish()
    {
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
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
        if (isInspecting)
        {
            if (fishSwimScript != null) fishSwimScript.enabled = false;
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            // Tính vị trí trước mặt người chơi và CỘNG THÊM ĐỘ CAO heightOffset
            Vector3 targetInspectPos = mainCameraTransform.position + (mainCameraTransform.forward * distanceFromCamera);
            targetInspectPos.y += heightOffset; // Nâng độ cao lên không bị chui xuống đất
            // Hướng nhìn ngang song song mặt đất
            Vector3 lookDir = -mainCameraTransform.forward;
            lookDir.y = 0; // Giữ góc nhìn nằm ngang phẳng, không bị chúc đầu xuống
            if (lookDir == Vector3.zero) lookDir = Vector3.forward;
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            if (lookSideWays)
            {
                targetRotation *= Quaternion.Euler(0, 90, 0); // Xoay nghiêng thân cá 90 độ
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
        if (audioSource != null) audioSource.Stop();

        if (currentMoveCoroutine != null)
            StopCoroutine(currentMoveCoroutine);
        currentMoveCoroutine = StartCoroutine(MoveToTarget(originalPosition, originalRotation, false));

    }

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
            if (infoPanel != null) 
                infoPanel.SetActive(true); 
        } 
        else 
        { 
            if (fishSwimScript != null) 
            {
                fishSwimScript.enabled = true;
            }
        }
    }
}