using UnityEngine;

public class HeatCameraWobble : MonoBehaviour
{
    [Range(0f, 0.1f)] public float posAmount = 0.01f;
    [Range(0f, 10f)] public float speed = 1.2f;

    Vector3 basePos;

    void Start() => basePos = transform.localPosition;

    void Update()
    {
        float t = Time.time * speed;
        float x = (Mathf.PerlinNoise(t, 0f) - 0.5f) * 2f * posAmount;
        float y = (Mathf.PerlinNoise(0f, t) - 0.5f) * 2f * posAmount;
        transform.localPosition = basePos + new Vector3(x, y, 0f);
    }
}
