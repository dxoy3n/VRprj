using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class VRInputHandler : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject instructionsPanel; // Có thể để trống, code sẽ tự động tìm trong scene

    private InputDevice rightController;
    private InputDevice leftController;

    void Start()
    {
        InitializeControllers();
        FindPanelIfNeeded();
    }

    void OnEnable()
    {
        // Tự động tìm lại Panel mỗi khi chuyển sang Scene mới
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPanelIfNeeded();
    }

    void FindPanelIfNeeded()
    {
        if (instructionsPanel == null)
        {
            // Tự động tìm đối tượng có tên "InstructionsPanel" trong Scene hiện tại
            GameObject panelObj = GameObject.Find("InstructionsPanel");
            if (panelObj != null)
            {
                instructionsPanel = panelObj;
            }
        }
    }

    void InitializeControllers()
    {
        List<InputDevice> rightDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightDevices);
        if (rightDevices.Count > 0) rightController = rightDevices[0];

        List<InputDevice> leftDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftDevices);
        if (leftDevices.Count > 0) leftController = leftDevices[0];
    }

    void Update()
    {
        // Kiểm tra kết nối tay cầm liên tục
        if (!rightController.isValid || !leftController.isValid)
        {
            InitializeControllers();
        }

        // 1. Nút B (trên tay phải): Bấm để tắt bảng hướng dẫn (nếu đang ở scene có panel)
        if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool bPressed) && bPressed)
        {
            if (instructionsPanel != null && instructionsPanel.activeSelf)
            {
                instructionsPanel.SetActive(false);
            }
        }

        // 2. Nút X (trên tay trái): Bấm để thoát trò chơi ở bất kỳ scene nào
        if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool xPressed) && xPressed)
        {
            Debug.Log("Thoát game...");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}