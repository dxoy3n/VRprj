using UnityEngine;

public class ClickCa : MonoBehaviour
{
    public FishInspectTarget inspectScript;

    // Hàm này tự động gọi khi bồ nhấp Chuột Trái vào Collider của con cá
    void OnMouseDown()
    {
        if (inspectScript != null)
        {
            inspectScript.ToggleInspect();
        }
    }
}