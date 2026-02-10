using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SatisfactionGauge : MonoBehaviour
{
    [SerializeField] private TemperatureGauge temperatureGauge;
    [Header("ゲージ設定")]
    [SerializeField] private float minValue = 0f;     // ゲージの最小値
    [SerializeField] private float maxValue = 100f;   // ゲージの最大値
    [SerializeField] private float increaseSpeed = 10.0f; //上昇値

    [SerializeField] private float maxSatisfaction = 70.0f; //満足度が上昇する時の温度の最大値
    [SerializeField] private float minSatisfaction = 50.0f; //満足度が上昇する時の温度の最小値

    [Header("フェード設定")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private float fadeDuration = 3.5f;

    [Header("満足度スライダー関係")]
    [SerializeField] private Slider satisFactionSlider;
    [SerializeField] private Canvas sliderCanvas;


    [SerializeField]
    private Animator anim;

    [SerializeField]
    private bool resurrection = false;

    private float currentValue = 0.0f;
    private bool isCleared = false;

    private bool isResurrecting = false;

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
        if (resurrection && !isResurrecting)
        {
            StartCoroutine(ResurrectionAfterDelay(0.5f));
        }

        if (!isCleared)
        {
            // スライダーの数値を変更
            satisFactionSlider.value = currentValue / maxValue;
            sliderCanvas.transform.rotation = Camera.main.transform.rotation;

            // 満足する温度の最大＆最小値
            if (maxSatisfaction > temperatureGauge.GetValue() &&
            minSatisfaction < temperatureGauge.GetValue())
            {
                currentValue += increaseSpeed * Time.deltaTime;
            }

            // 満足度達成
            if (currentValue >= maxValue)
            {
                Debug.Log("クリア");
                isCleared = true;
                anim.SetBool("StandUp",true);
                clearConfirmation?.NotifyCleared(this); // 通知
            }
        }
        else
        {
            FadeCharacter();
        }
    }

    public bool IsCleared() => isCleared;

    private void FadeCharacter()
    {
        float deltaAlpha = Time.deltaTime / fadeDuration;

        foreach (var r in renderers)
        {
            foreach (var mat in r.materials)
            {
                Color c = mat.color;
                c.a = Mathf.Clamp01(c.a - deltaAlpha);
                mat.color = c;
            }
        }
    }

    private IEnumerator ResurrectionAfterDelay(float delay)
    {
        isResurrecting = true;

        anim.Play("Beck", 0, 0f);

        yield return new WaitForSeconds(delay);

        Resurrection();

        // 1回きりにするなら
        resurrection = false;
    }

    private void Resurrection()
    {
        foreach (var r in renderers)
        {
            foreach (var mat in r.materials)
            {
                Color c = mat.color;
                c.a = 1.0f;
                mat.color = c;
            }
        }
    }
}
