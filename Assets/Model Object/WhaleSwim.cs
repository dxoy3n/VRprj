using UnityEngine;

public class WhaleSwim : MonoBehaviour
{
 
    public float swimSpeed = 2.5f;   
    public float swimAmount = 4.0f;  
    public float waveSpeed = 1.2f;   
    public float waveHeight = 0.2f;  

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        
        float tilt = Mathf.Sin(Time.time * swimSpeed) * swimAmount;
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, tilt, transform.localRotation.eulerAngles.z);

       
        float newY = startPos.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}