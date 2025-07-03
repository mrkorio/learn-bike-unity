using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed = 5f;
    public float despawnZ = -5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = Vector3.forward * -moveSpeed;
    }

    void Update()
    {
        if (transform.position.z < despawnZ)
        {
            Destroy(gameObject);
        }
    }
}