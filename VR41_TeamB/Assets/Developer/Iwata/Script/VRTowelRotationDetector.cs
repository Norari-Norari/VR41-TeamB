using UnityEngine;
using UnityEngine.InputSystem;

public class B_VRTowelRotationDetector : MonoBehaviour
{
    [Header("Hand Tracking")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("Grip Input Actions")]
    public InputActionProperty leftGripAction;
    public InputActionProperty rightGripAction;

    [Header("Detection Settings")]
    [Tooltip("片手で回しているとみなす角速度 (deg/sec)")]
    public float spinRotationThreshold = 300f;

    [Tooltip("両手であおいでいるとみなすY軸移動速度 (m/s)")]
    public float fanMoveThreshold = 0.5f;

    [Tooltip("判定更新間隔 (秒)")]
    public float detectionInterval = 0.1f;

    private Quaternion lastLeftRot;
    private Quaternion lastRightRot;
    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;

    private float timer = 0f;

    public enum Mode { None, TwoHanded, Spin }
    public Mode CurrentMode { get; private set; } = Mode.None;

    public bool IsGrabbing { get; private set; }

    public System.Action OnGrabStart;
    public System.Action OnGrabEnd;
    public bool IsSpinning { get; private set; }
    public bool IsFanning { get; private set; }

    //ヒットボックス出現判定
    public bool IsHitBox { get { return IsFanning || IsSpinning; } }

    [Header("HitBox")]
    public GameObject hitBoxPrefab;
    public float hitBoxDistance = 0.3f;

    private GameObject hitBoxInstance;



    void Start()
    {
        if (leftHand == null || rightHand == null)
        {
            Debug.LogError("両方の手のTransformを設定してください。");
            enabled = false;
            return;
        }

        leftGripAction.action.Enable();
        rightGripAction.action.Enable();

        lastLeftRot = leftHand.rotation;
        lastRightRot = rightHand.rotation;
        lastLeftPos = leftHand.position;
        lastRightPos = rightHand.position;
    }

    void Update()
    {
        bool leftPressed = leftGripAction.action.ReadValue<float>() > 0.5f;
        bool rightPressed = rightGripAction.action.ReadValue<float>() > 0.5f;

        // ---- モード判定 ----
        if (leftPressed && rightPressed)
            CurrentMode = Mode.TwoHanded;
        else if (leftPressed ^ rightPressed)
            CurrentMode = Mode.Spin;
        else 
            CurrentMode = Mode.None;


        // ---- 掴み状態の遷移判定 ----
        if (CurrentMode == Mode.TwoHanded && !IsGrabbing)
        {
            IsGrabbing = true;
            OnGrabStart?.Invoke();
        }
        else if (CurrentMode != Mode.TwoHanded && IsGrabbing)
        {
            IsGrabbing = false;
            OnGrabEnd?.Invoke();
        }

        timer += Time.deltaTime;
        if (timer >= detectionInterval)
        {
            DetectMotion();
            timer = 0f;
        }

        Debug.Log($"Mode: {CurrentMode} | Spinning: {IsSpinning} | Fanning: {IsFanning}");
    }

    /// <summary>
    /// Hit時の判定
    /// </summary>
    /// <param name="baseTransform"></param>
    private void UpdateHitBox(Transform baseTransform)
    {
        if (IsHitBox)
        {
            if (hitBoxInstance == null)
                hitBoxInstance = Instantiate(hitBoxPrefab);

            hitBoxInstance.transform.position =
                baseTransform.position + baseTransform.forward * hitBoxDistance;

            hitBoxInstance.transform.rotation = baseTransform.rotation;

            //コントローラー振動
        }
        else
        {
            if (hitBoxInstance != null)
                Destroy(hitBoxInstance);
        }
    }

    private void DetectMotion()
    {
        IsSpinning = false;
        IsFanning = false;

        // 両手モード → 上下あおぎ判定
        if (CurrentMode == Mode.TwoHanded)
        {
            Vector3 leftVel = (leftHand.position - lastLeftPos) / detectionInterval;
            Vector3 rightVel = (rightHand.position - lastRightPos) / detectionInterval;

            // 両手のY方向の平均速度
            float avgYVel = (leftVel.y + rightVel.y) * 0.5f;

            // 一定以上のY軸変動で「仰いでいる」と判定
            if (Mathf.Abs(avgYVel) > fanMoveThreshold)
                IsFanning = true;

            lastLeftPos = leftHand.position;
            lastRightPos = rightHand.position;
            lastLeftRot = leftHand.rotation;
            lastRightRot = rightHand.rotation;


            Transform dummy = leftHand; // forward基準だけ使うなら片手でOK
            UpdateHitBox(dummy);

            return;
        }

        // 片手モード → 回転判定
        if (CurrentMode == Mode.Spin)
        {
            Transform activeHand = null;
            Quaternion lastRot = Quaternion.identity;
            Quaternion currentRot = Quaternion.identity;

            if (leftGripAction.action.ReadValue<float>() > 0.5f)
            {
                activeHand = leftHand;
                lastRot = lastLeftRot;
                currentRot = leftHand.rotation;
            }
            else if (rightGripAction.action.ReadValue<float>() > 0.5f)
            {
                activeHand = rightHand;
                lastRot = lastRightRot;
                currentRot = rightHand.rotation;
            }

            if (activeHand != null)
            {
                float angleDelta = Quaternion.Angle(lastRot, currentRot);
                float rotationSpeed = angleDelta / detectionInterval;
                IsSpinning = rotationSpeed > spinRotationThreshold;

                if (activeHand == leftHand)
                    lastLeftRot = currentRot;
                else
                    lastRightRot = currentRot;

                UpdateHitBox(activeHand);
            }
        }

        // モードがNoneなら履歴更新だけ
        if (CurrentMode == Mode.None)
        {
            lastLeftPos = leftHand.position;
            lastRightPos = rightHand.position;
            lastLeftRot = leftHand.rotation;
            lastRightRot = rightHand.rotation;
        }
    }
}
