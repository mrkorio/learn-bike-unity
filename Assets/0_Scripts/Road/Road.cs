using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public float speed;
    public float despawnZ;
    void Update()
    {
        // Mover hacia adelante
        transform.position += Vector3.forward * -speed * Time.deltaTime;

        if (transform.position.z < despawnZ)
        {
            Destroy(gameObject);
        }
    }
}
