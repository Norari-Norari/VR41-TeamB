using UnityEngine;
using UnityEngine.XR;

public class HapticsManager : B_SingletonMonoBehaviour<HapticsManager>
{
    public float amplitude = 0.3f;
    public float pulseDuration = 0.1f;
    public float resendInterval = 0.08f;

    InputDevice leftDevice;
    InputDevice rightDevice;

    bool isPlaying;
    int requestCount;
    float timer;

    protected override void Awake()
    {
        base.Awake();
        FindDevices();
    }

    void FindDevices()
    {
        leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        if (!isPlaying) return;

        timer += Time.deltaTime;
        if (timer >= resendInterval)
        {
            SendPulse();
            timer = 0f;
        }
    }

    public void Request()
    {
        requestCount++;
        if (!isPlaying)
            StartHaptics();
    }

    public void Release()
    {
        requestCount = Mathf.Max(0, requestCount - 1);
        if (requestCount == 0)
            StopHaptics();
    }

    void StartHaptics()
    {
        isPlaying = true;
        timer = 0f;
        SendPulse();
    }

    void StopHaptics()
    {
        isPlaying = false;
        leftDevice.StopHaptics();
        rightDevice.StopHaptics();
    }

    void SendPulse()
    {
        Send(leftDevice);
        Send(rightDevice);
    }

    void Send(InputDevice device)
    {
        if (!device.isValid) return;

        if (device.TryGetHapticCapabilities(out var cap) && cap.supportsImpulse)
            device.SendHapticImpulse(0, amplitude, pulseDuration);
    }
}
