using System.Collections.Generic;
using UnityEngine;

public class ClearConfirmation : MonoBehaviour
{
    public static int clearedCount = 0;
    private List<SatisfactionGauge> gauges = new List<SatisfactionGauge>();

    // SatisfactionGauge から呼び出されて登録される
    public void Register(SatisfactionGauge gauge)
    {
        if (!gauges.Contains(gauge))
        {
            gauges.Add(gauge);
        }
    }
    // SatisfactionGauge から「クリアしたよ」と通知を受ける
    public void NotifyCleared(SatisfactionGauge gauge)
    {
        Debug.Log($"{gauge.gameObject.name} がクリアを報告しました！");

        // 現在クリアしている数を確認
        foreach (var g in gauges)
        {
            if (g.IsCleared()) clearedCount++;
        }

        Debug.Log($"現在クリアしているキャラ数: {clearedCount} / {gauges.Count}");

        // 例：全員クリアしたら何か起こす
        if (clearedCount == gauges.Count)
        {
            Debug.Log("全員クリア！");
        }
    }
}
