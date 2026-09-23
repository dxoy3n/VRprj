using UnityEngine;
using UnityEngine.UI;

public class FishInteractable : MonoBehaviour
{
    public GameObject infoPanel;
    public Image panelImageHolder;
    public Sprite fishSprite;
    public Button closeButton;          // Kéo button_close vào đây!

    public Vector3 minBounds = new Vector3(-10f, 0f, -10f);
    public Vector3 maxBounds = new Vector3(10f, 5f, 10f);

    private void Start()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        ShowPanel();
    }

    public void ShowPanel()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
        }

        if (panelImageHolder != null && fishSprite != null)
        {
            panelImageHolder.sprite = fishSprite;
        }

        // Tự động gán hàm Close cho con cá hiện tại
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners(); // Xóa sự kiện con cá cũ
            closeButton.onClick.AddListener(ClosePanelAndRelocateFish); // Đăng ký cho con cá vừa click
        }
    }

    public void ClosePanelAndRelocateFish()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);

        transform.position = new Vector3(randomX, randomY, randomZ);
    }
}