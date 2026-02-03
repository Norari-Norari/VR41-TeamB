using UnityEngine;

public class B_VRTowelGrabController : MonoBehaviour
{
    public B_VRTowelRotationDetector detector;
    public GameObject handlePrefab;

    private GameObject handleInstance;

    void Start()
    {
        detector.OnGrabStart += HandleGrabStart;
        detector.OnGrabEnd += HandleGrabEnd;
    }

    void HandleGrabStart()
    {
        handleInstance = Instantiate(handlePrefab);

        // èâä˙à íuÅFóºéËÇÃíÜä‘
        handleInstance.transform.position =
            (detector.leftHand.position + detector.rightHand.position) * 0.5f;
    }

    void HandleGrabEnd()
    {
        Destroy(handleInstance);
    }

    void Update()
    {
        if (detector.IsGrabbing && handleInstance != null)
        {
            handleInstance.transform.position =
                (detector.leftHand.position + detector.rightHand.position) * 0.5f;
        }
    }
}
