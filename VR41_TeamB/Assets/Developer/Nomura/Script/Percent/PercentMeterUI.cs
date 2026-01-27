using System;
using UnityEngine;
using UnityEngine.UI;

public class PercentMeterUI : MonoBehaviour
{
    // ゲージ最低時の回転角度(初期値)
    private float minRotation = 135;
    [Header("ゲージ最大時の回転角度")]
    [SerializeField] private float maxRotation = -135;

    // 現在の回転角度
    private float currentRotation = 0;
    // 現在の湿度
    private float currentHumidity = 0;

    [Header("最大湿度")]
    [SerializeField] private float maxHumidity = 100f;

    [SerializeField] private Image HariImage;

    // =============================
    // コールバック（イベント）
    // =============================

    /// <summary>湿度を増やす</summary>
    public event Action<float> OnHumidityIncreased;

    /// <summary>湿度を減らす</summary>
    public event Action<float> OnHumidityDecreased;

    /// <summary>湿度MAX到達</summary>
    public event Action OnReachedHumidityMax;

    // =============================
    // 外部取得用プロパティ
    // =============================

    /// <summary>湿度がMAXか？</summary>
    public bool IsHumidityMax => currentHumidity >= maxHumidity;

    // =============================
    // 湿度操作
    // =============================

    /// <summary>
    /// 指定した値分だけ湿度を加算する
    /// </summary>
    public void AddHumidity(float value)
    {
        SetHumidity(currentHumidity + value);
    }

    /// <summary>
    /// 指定した値分だけ湿度を減算する
    /// </summary>
    public void SubHumidity(float value)
    {
        SetHumidity(currentHumidity - value);
    }

    /// <summary>指定した湿度に設定</summary>
    public void SetHumidity(float value)
    {
        float prevHumidity = currentHumidity;
        currentHumidity = Mathf.Clamp(value, 0, maxHumidity);

        // 上下判定
        if (currentHumidity > prevHumidity)
        {
            OnHumidityIncreased?.Invoke(currentHumidity);
        }
        else if (currentHumidity < prevHumidity)
        {
            OnHumidityDecreased?.Invoke(currentHumidity);
        }

        // MAX到達（瞬間のみ）
        if (prevHumidity < maxHumidity && currentHumidity >= maxHumidity)
        {
            OnReachedHumidityMax?.Invoke();
        }

        UpdateRotation();
    }

    // =============================
    // UI更新
    // =============================

    private void UpdateRotation()
    {
        float t = currentHumidity / maxHumidity;
        currentRotation = Mathf.Lerp(minRotation, maxRotation, t);
        HariImage.transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
