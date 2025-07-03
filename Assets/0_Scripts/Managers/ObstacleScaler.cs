using UnityEngine;

public class ObstacleScaler : MonoBehaviour
{
    public Transform player;
    public float nearDistance = 50f;
    public float farDistance = 600f;
    public float minScale = 0.5f;
    public float maxScale = 2f;
    public float offSet;
    public float rotationX;
    public bool hasToScaleX;

    private void Awake()
    {
        player = PlayerController.instance.transform;
    }

    void Update()
    {
        float distance = Mathf.Abs(transform.position.z - player.position.z);
        float t = Mathf.InverseLerp(farDistance, nearDistance, distance);
        float scale = Mathf.Lerp(minScale, maxScale, t);
        if (hasToScaleX)
            transform.localScale = new Vector3(scale * 1.4f, scale, scale);
        else if (!hasToScaleX)
            transform.localScale = new Vector3(scale, scale, scale);
    }
}