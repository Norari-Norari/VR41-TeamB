using UnityEngine;
using System;

public class TemperatureGauge : MonoBehaviour
{
    [Header("ゲージ設定")]
    [SerializeField] private float minValue = 0f;     // ゲージの最小値
    [SerializeField] private float maxValue = 100f;   // ゲージの最大値
    [SerializeField] private float increaseSpeed = 10.0f; //上昇値
    [SerializeField] private float decreaseSpeed = 0f;  // 減衰速度

    [Header("現在の値（確認用）")]
    private float currentValue = 0.0f;

    // 行動関数を登録できるようにする
    public Func<bool> ActionFunc { get; set; }

    void Update()
    {
        if (ActionFunc != null && ActionFunc.Invoke())
        {
            // 行動がtrue → 値を上げる
            currentValue += increaseSpeed * Time.deltaTime;
        }
        else if (decreaseSpeed > 0f)
        {
            // 行動がfalse → 値を下げる（任意）
            currentValue -= decreaseSpeed * Time.deltaTime;
        }

        // 値を範囲内にクランプ
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
    }

    // 外部から現在値を取得
    public float GetValue() => currentValue;

    // 外部から直接設定も可能
    public void SetValue(float value)
    {
        currentValue = Mathf.Clamp(value, minValue, maxValue);
    }
}
