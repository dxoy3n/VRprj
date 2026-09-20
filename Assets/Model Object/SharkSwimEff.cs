using UnityEngine;

public class SharkSwimEff : MonoBehaviour
{
    public float swimSpeed = 3f;   
    public float swimAmount = 4f;   
    public float waveSpeed = 1.5f;  
    public float waveHeight = 0.15f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
       
        float tilt = Mathf.Sin(Time.time * swimSpeed) * swimAmount;
        transform.localRotation = Quaternion.Euler(0, tilt, 0);

        
        float newY = startPos.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}