using UnityEngine;

public class WhaleSwim : MonoBehaviour
{
  
    public float swimSpeed = 1.8f;   
    public float swimAmount = 6.0f;  
    public float waveSpeed = 1.0f;   
    public float waveHeight = 0.35f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float pitch = Mathf.Sin(Time.time * swimSpeed) * swimAmount;
        transform.localRotation = Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        float newY = startPos.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}