using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed = 5f;
    public float limitX = 4f;

    public float tiltAmount = 15f; // grados máximos de rotación
    public float tiltSmooth = 5f; // velocidad de rotación

    private float targetTiltZ = 0f;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        float inputX = GetInput();

        // Movimiento lateral del personaje
        Vector3 pos = transform.position;
        pos.x += inputX * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -limitX, limitX);
        transform.position = pos;

        // Rotación personaje
        targetTiltZ = -inputX * tiltAmount;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetTiltZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSmooth * Time.deltaTime);
    }
    float GetInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetAxis("Horizontal"); // Teclado si es de PC
#elif UNITY_ANDROID || UNITY_IOS //Giroscopio si es celular
        return Mathf.Clamp(Input.acceleration.x * 2f, -1f, 1f); // Giroscopio
#else
        return 0f;
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(0);
        }
    }
}