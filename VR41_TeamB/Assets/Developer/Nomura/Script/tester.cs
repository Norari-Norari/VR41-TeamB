using UnityEngine;
using UnityEngine.InputSystem;

public class tester : MonoBehaviour
{
    [SerializeField] private PercentMeterUI UI;

    // 加算・減算量
    [SerializeField] private float changeValue = 0.1f;

    void Start()
    {
        // 湿度が増えた
        UI.OnHumidityIncreased += OnHumidityIncreased;

        // 湿度が減った
        UI.OnHumidityDecreased += OnHumidityDecreased;

        // 湿度MAX到達
        UI.OnReachedHumidityMax += OnReachedHumidityMax;
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // 1キー：加算
        if (keyboard.digit1Key.isPressed)
        {
            UI.AddHumidity(changeValue);
        }

        // 2キー：減算
        if (keyboard.digit2Key.isPressed)
        {
            UI.SubHumidity(changeValue);
        }
    }


    private void OnDestroy()
    {
        // ★ 正しい登録解除
        UI.OnHumidityIncreased -= OnHumidityIncreased;
        UI.OnHumidityDecreased -= OnHumidityDecreased;
        UI.OnReachedHumidityMax -= OnReachedHumidityMax;
    }

    // =============================
    // イベント受信
    // =============================

    private void OnHumidityIncreased(float value)
    {
        Debug.Log($"湿度が増えた：{value}");
    }

    private void OnHumidityDecreased(float value)
    {
        Debug.Log($"湿度が減った：{value}");
    }

    private void OnReachedHumidityMax()
    {
        Debug.Log("🔥 湿度がMAXに到達！");
    }
}
