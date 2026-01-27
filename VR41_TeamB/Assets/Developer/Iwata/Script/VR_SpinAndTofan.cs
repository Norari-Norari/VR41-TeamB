using UnityEngine;


public class VR_SpinAndTofan : MonoBehaviour
{
    [Header("Hand Tracking")]
    public Transform handTransform; // タオルを持つ手のTransform

    [Header("Detection Settings")]
    public float rotationThreshold = 300f; // 1秒あたりの回転量(degree/sec)
    public float detectionInterval = 0.1f;

    private Quaternion lastRotation;
    private float timer = 0f;

    public bool IsSpinning { get; private set; } = false;

    void Start()
    {
        if (handTransform == null)
        {
            Debug.LogError("HandTransform is not assigned!");
            enabled = false;
            return;
        }

        lastRotation = handTransform.rotation;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= detectionInterval)
        {
            Quaternion currentRotation = handTransform.rotation;

            float angleDelta = Quaternion.Angle(lastRotation, currentRotation);
            float rotationSpeed = angleDelta / timer;

            IsSpinning = rotationSpeed > rotationThreshold;

            // デバッグ
            Debug.Log($"RotationSpeed: {rotationSpeed:F1} deg/sec | Spinning: {IsSpinning}");

            lastRotation = currentRotation;
            timer = 0f;
        }
    }
}
