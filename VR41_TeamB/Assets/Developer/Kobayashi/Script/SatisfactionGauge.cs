using UnityEngine;

public class SatisfactionGauge : MonoBehaviour
{
    [SerializeField] private TemperatureGauge temperatureGauge;
    [Header("ゲージ設定")]
    [SerializeField] private float minValue = 0f;     // ゲージの最小値
    [SerializeField] private float maxValue = 100f;   // ゲージの最大値
    [SerializeField] private float increaseSpeed = 10.0f; //上昇値

    [SerializeField] private float maxSatisfaction = 70.0f; //満足度が上昇する時の温度の最大値
    [SerializeField] private float minSatisfaction = 50.0f; //満足度が上昇する時の温度の最小値

    private float currentValue = 0.0f;
    private bool isCleared = false;

    private ClearConfirmation clearConfirmation; // 参照保持
    void Start()
    {
        // シーン内の ClearConfirmation を探して登録
        clearConfirmation = FindObjectOfType<ClearConfirmation>();
        if (clearConfirmation != null)
        {
            clearConfirmation.Register(this);
        }
        else
        {
            Debug.LogWarning("ClearConfirmation がシーン内に見つかりません。");
        }
    }

    void Update()
    {
        if (maxSatisfaction > temperatureGauge.GetValue() &&
            minSatisfaction < temperatureGauge.GetValue())
        {
            currentValue += increaseSpeed * Time.deltaTime;
        }

        if (currentValue >= maxValue)
        {
            Debug.Log("クリア");
            isCleared = true;
            clearConfirmation?.NotifyCleared(this); // 通知
        }
    }

    public bool IsCleared() => isCleared;
}
