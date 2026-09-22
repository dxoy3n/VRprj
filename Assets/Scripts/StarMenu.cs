using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;     // Kéo Menu chính (chứa các nút) vào đây
    public GameObject instructionsPanel; // Kéo Panel Hướng dẫn vào đây

    [Header("Scene Settings")]
    public string gameSceneName = "SampleScene"; // Đổi tên thành tên Scene game chính của bạn

    // 1. Nút Bắt Đầu: Chuyển sang Scene game
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Level1_TinyOcean");
    }

    // 2. Nút Hướng Dẫn: Hiển thị Panel hướng dẫn tay cầm VR
    public void OnInstructionsButtonClicked()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(true);
    }

    // 3. Nút Quay Lại (Dùng cho trong bảng Hướng dẫn nếu cần quay về menu chính)
    public void OnBackButtonClicked()
    {
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // 4. Nút Thoát: Thoát game (Chạy thực tế hoặc dừng trong Editor)
    public void OnQuitButtonClicked()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();


        UnityEditor.EditorApplication.isPlaying = false;


    }
}