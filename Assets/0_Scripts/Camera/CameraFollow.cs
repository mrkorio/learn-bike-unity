using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform player;       // Referencia a Bart
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Altura y distancia
    public float smoothSpeed = 5f;
    public bool followX = true;

    void LateUpdate()
    {
        Vector3 desiredPosition = transform.position;

        if (followX)
        {
            // Solo seguimos el eje X del jugador
            desiredPosition.x = player.position.x + offset.x;
        }

        // Mantener Z y Y fijos (desde offset)
        desiredPosition.y = player.position.y + offset.y;
        desiredPosition.z = player.position.z + offset.z;

        // Suavizado
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}