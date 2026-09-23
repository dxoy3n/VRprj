using UnityEngine;

public class RandomCaBoi : MonoBehaviour
{
    [Header("Cấu hình vùng bơi (15m x 15m)")]
    public Vector3 centerPosition = Vector3.zero;
    public Vector3 swimAreaSize = new Vector3(15f, 5f, 15f);

    [Header("Tốc độ & Chuyển động")]
    public float swimSpeed = 2.0f;        // Tốc độ bơi
    public float rotationSpeed = 1.5f;    // Tốc độ xoay 
    public float reachDistance = 1.5f;    // Tăng khoảng cách nhận diện đích 

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        if (centerPosition == Vector3.zero)
        {
            centerPosition = transform.position;
        }

        GetNewRandomTarget();
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // 1. Chỉ tính toán góc xoay mới khi còn cách xa điểm đích > 0.5m
        if (distanceToTarget > 0.5f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(direction);
            }
        }

        // 2. Xoay mượt mà bằng Slerp không phụ thuộc vào fps
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 3. Di chuyển thẳng theo hướng hiện tại
        transform.position += transform.forward * swimSpeed * Time.deltaTime;

        // 4. Nếu đã vào vùng tiệm cận điểm đích, đổi điểm ngẫu nhiên mới lập tức
        if (distanceToTarget < reachDistance)
        {
            GetNewRandomTarget();
        }
    }

    void GetNewRandomTarget()
    {
        float randomX = Random.Range(-swimAreaSize.x / 2f, swimAreaSize.x / 2f);
        float randomY = Random.Range(-swimAreaSize.y / 2f, swimAreaSize.y / 2f);
        float randomZ = Random.Range(-swimAreaSize.z / 2f, swimAreaSize.z / 2f);

        targetPosition = centerPosition + new Vector3(randomX, randomY, randomZ);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = (centerPosition == Vector3.zero) ? transform.position : centerPosition;
        Gizmos.DrawWireCube(center, swimAreaSize);

        // Vẽ đường nối tới điểm target hiện tại trong Scene view
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPosition);
            Gizmos.DrawSphere(targetPosition, 0.3f);
        }
    }
}