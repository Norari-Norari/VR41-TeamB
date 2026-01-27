using System.Collections;
using UnityEngine;
using static Unity.Collections.AllocatorManager;




public class VRAnyButtonWarpController : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin;   // XR Origin（親）
    [SerializeField] private Transform warpPoint;
    [SerializeField] private VRTitleLockController vrLock;

    private bool started = false;

    void Update()
    {
        if (started) return;

        if (VRAnyButtonUtility.AnyPressDown())
        {
            started = true;
            StartCoroutine(WarpSequence());
        }
    }

    private IEnumerator WarpSequence()
    {
        yield return FadeManager.Instance.FadeOut();

        //ワープする
        xrOrigin.SetPositionAndRotation(
            warpPoint.position,
            warpPoint.rotation
        );

        //移動できるようにする
        vrLock?.ExitTitle();

        yield return FadeManager.Instance.FadeIn();
    }
}
