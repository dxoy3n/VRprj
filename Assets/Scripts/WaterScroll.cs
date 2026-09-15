using UnityEngine;

public class WaterRipple : MonoBehaviour
{
    public float speed = 1.2f;      // Tốc độ nhấp nhô
    public float amount = 0.02f;     // Biên độ lăn tăn (nhẹ nhàng cho bé)

    private Material mat;
    private static readonly int BumpMap = Shader.PropertyToID("_BumpMap");

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Tạo dao động tròn nhè nhẹ quanh vị trí gốc
        float x = Mathf.Sin(Time.time * speed) * amount;
        float y = Mathf.Cos(Time.time * speed * 0.8f) * amount;

        Vector2 offset = new Vector2(x, y);

        // Gán offset cho cả texture chính lẫn Normal Map
        mat.mainTextureOffset = offset;
        
        if (mat.HasProperty(BumpMap))
        {
            mat.SetTextureOffset(BumpMap, offset);
        }
    }
}