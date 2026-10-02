using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Thêm thư viện TextMeshPro

public class StarfishManager : MonoBehaviour
{
    // 1. Khai báo Instance để các script khác (như StarfishCollect) gọi tới không bị lỗi
    public static StarfishManager Instance;

    public TextMeshProUGUI starfishText; // Đổi sang TextMeshProUGUI
    public int totalStarfishNeeded = 3; 
    public GameObject keyObject;

    void Awake()
    {
        // Khởi tạo Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Nếu là Scene đầu tiên (Level2_TinyOcean) -> Reset sao biển về 0
        // Sang Level3_TinyOcean -> Tự động giữ nguyên điểm cũ để cộng dồn!
        if (SceneManager.GetActiveScene().name == "Level2_TinyOcean")
        {
            PlayerPrefs.SetInt("StarfishCount", 0);
            PlayerPrefs.Save();
        }

        UpdateStarfishUI();
        CheckKeyUnlock(); // Kiểm tra ngay khi vừa bước vào Scene mới
    }

    public void AddStarfish()
    {
        int currentCount = PlayerPrefs.GetInt("StarfishCount", 0) + 1;
        PlayerPrefs.SetInt("StarfishCount", currentCount);
        PlayerPrefs.Save();

        UpdateStarfishUI();
        CheckKeyUnlock();
    }

    void UpdateStarfishUI()
    {
        int currentCount = PlayerPrefs.GetInt("StarfishCount", 0);
        if (starfishText != null)
        {
            starfishText.text = "Sao biển đã thu thập: " + currentCount + "/" + totalStarfishNeeded;
        }
    }

    void CheckKeyUnlock()
    {
        int currentCount = PlayerPrefs.GetInt("StarfishCount", 0);
        if (currentCount >= totalStarfishNeeded)
        {
            if (keyObject != null)
            {
                // Kích hoạt chìa khóa
                keyObject.SetActive(true);

                // Đặt chìa khóa hiện ngay trước mặt Camera người chơi
                if (Camera.main != null)
                {
                    Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
                    spawnPosition.y -= 0.2f; // Hạ nhẹ tầm mắt cho đẹp
                    
                    keyObject.transform.position = spawnPosition;
                    keyObject.transform.rotation = Camera.main.transform.rotation;
                }
            }
        }
    }
}