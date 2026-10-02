using UnityEngine;
using UnityEngine.EventSystems;

public class StarfishCollect : MonoBehaviour, IPointerClickHandler
{
    // Bắn tia Ray / Nhấp chuột vào sao biển
    public void OnPointerClick(PointerEventData eventData)
    {
        Collect();
    }

    // Hoặc nếu dùng OnMouseDown
    private void OnMouseDown()
    {
        Collect();
    }

    void Collect()
    {
        // Gọi hàm cộng điểm bên StarfishManager
        if (StarfishManager.Instance != null)
        {
            StarfishManager.Instance.AddStarfish();
        }

        // Biến mất con sao biển vừa nhặt
        Destroy(gameObject);
    }
}