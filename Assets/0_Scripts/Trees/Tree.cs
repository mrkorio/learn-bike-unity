using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public float offSet;
    public bool isLeftSide;
    public float despawnZ = -5f;

    public float speed = 100f;

    void Update()
    {
        // Mover hacia adelante del mundo (Z+)
        transform.position += Vector3.forward * -speed * Time.deltaTime;

        if (transform.position.z < despawnZ)
        {
            Destroy(gameObject);
        }
    }
}
