using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweetTicker : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform maskRect;
    [SerializeField] private RectTransform textRectA;
    [SerializeField] private RectTransform textRectB;
    [SerializeField] private Text textA;
    [SerializeField] private Text textB;

    [Header("Data")]
    [SerializeField] private List<TweetData> tweetList = new();

    [Header("Settings")]
    [SerializeField] private float scrollSpeed = 150f;
    [SerializeField] private float textSpacing = 50f;

    private RectTransform _current;
    private RectTransform _next;

    void Start()
    {
        _current = textRectA;
        _next = textRectB;

        SetRandomText(textA);
        SetRandomText(textB);

        float maskWidth = maskRect.rect.width;

        // current を画面右外に配置
        _current.anchoredPosition =
            new Vector2(maskWidth, 0);

        // current の右に next を配置
        float currentWidth = GetTextWidth(_current);
        _next.anchoredPosition =
            new Vector2(maskWidth + currentWidth + textSpacing, 0);
    }


    void Update()
    {
        MoveTexts();
        CheckRecycle();
    }

    //==================================================
    // 移動処理
    //==================================================
    void MoveTexts()
    {
        Vector2 move = Vector2.left * scrollSpeed * Time.deltaTime;
        _current.anchoredPosition += move;
        _next.anchoredPosition += move;
    }

    //==================================================
    // 左に完全に消えたら再利用
    //==================================================
    void CheckRecycle()
    {
        float currentRightEdge =
            _current.anchoredPosition.x + GetTextWidth(_current);

        if (currentRightEdge < 0)
        {
            RecycleText();
        }
    }

    //==================================================
    // Textを右側に再配置
    //==================================================
    void RecycleText()
    {
        // 今消えた方の Text を取得
        Text recycledText = (_current == textRectA) ? textA : textB;

        SetRandomText(recycledText);

        // 次のTextの右端に配置
        float nextWidth = GetTextWidth(_next);
        float newX = _next.anchoredPosition.x + nextWidth + textSpacing;

        _current.anchoredPosition = new Vector2(newX, 0);

        // 入れ替え
        (_current, _next) = (_next, _current);
    }

    //==================================================
    // ランダム文言セット
    //==================================================
    void SetRandomText(Text target)
    {
        if (tweetList.Count == 0) return;

        TweetData data = tweetList[Random.Range(0, tweetList.Count)];

        // 表示文字列を組み立て
        string content = $"[@{data.userName}]{data.message}";
        target.text = content;

        // レイアウトを即時更新（超重要）
        LayoutRebuilder.ForceRebuildLayoutImmediate(target.rectTransform);

        // フォントサイズ・内容に基づいた実寸取得
        float width = target.preferredWidth;
        float height = target.preferredHeight;

        // RectTransform に反映
        RectTransform rect = target.rectTransform;
        rect.sizeDelta = new Vector2(width, height);
    }


    //==================================================
    // Textの横幅取得
    //==================================================
    float GetTextWidth(RectTransform rect)
    {
        return rect.sizeDelta.x;
    }
}
