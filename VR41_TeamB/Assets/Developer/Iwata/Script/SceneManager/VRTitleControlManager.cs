using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class VRTitleLockController : MonoBehaviour
{
    public GameObject locomotionSystem;
    private ContinuousMoveProvider moveProvider;
    private ContinuousTurnProvider turnProvider;
    private SnapTurnProvider snapTurnProvider;
    private TeleportationProvider teleportProvider;

    private bool isLocked;

    void Awake()
    {
        //locomotionSystem = GetComponentInChildren<LocomotionMediator>(true);
        moveProvider = GetComponentInChildren<ContinuousMoveProvider>(true);
        turnProvider = GetComponentInChildren<ContinuousTurnProvider>(true);
        snapTurnProvider = GetComponentInChildren<SnapTurnProvider>(true);
        teleportProvider = GetComponentInChildren<TeleportationProvider>(true);
    }

    void OnEnable()
    {
        EnterTitle();
    }

    public void EnterTitle()
    {
        if (isLocked) return;
        isLocked = true;

        // ★ 最優先で止める
        if (locomotionSystem) locomotionSystem.SetActive(false);

        if (moveProvider) moveProvider.enabled = false;
        if (turnProvider) turnProvider.enabled = false;
        if (snapTurnProvider) snapTurnProvider.enabled = false;
        if (teleportProvider) teleportProvider.enabled = false;
    }

    public void ExitTitle()
    {
        if (!isLocked) return;
        isLocked = false;

        if (locomotionSystem) locomotionSystem.SetActive(true);

        if (moveProvider) moveProvider.enabled = true;
        if (turnProvider) turnProvider.enabled = true;
        if (snapTurnProvider) snapTurnProvider.enabled = true;
        if (teleportProvider) teleportProvider.enabled = true;
    }
}
