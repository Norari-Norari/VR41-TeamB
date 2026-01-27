using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomTextEffect : BaseMeshEffect
{
    /// <summary>
    /// テキストに適用する効果の種類
    /// </summary>
    public enum EffectMode
    {
        None,               // 効果なし
        Shadow,             // シャドウのみ
        Outline,            // アウトラインのみ
        ShadowAndOutline    // シャドウ＋アウトライン
    }

    /// <summary>
    /// アウトラインの配置方法
    /// </summary>
    public enum OutlinePlacement
    {
        Outside,    // 外側に描画（先にアウトライン→後に本体）
        Inside      // 内側に描画（先に影→後にアウトライン）
    }

    [Header("基本設定")]
    public EffectMode mode = EffectMode.ShadowAndOutline;
    public OutlinePlacement placement = OutlinePlacement.Outside;

    [Header("アウトライン設定")]
    public Color outlineColor = Color.black;
    public float outlineThickness = 1f;

    [Header("シャドウ設定")]
    public Color shadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 shadowOffset = new Vector2(2f, -2f);
    public float shadowThickness = 1f;

    private readonly List<UIVertex> _verts = new List<UIVertex>();

    /// <summary>
    /// メッシュの頂点情報を変更して、テキストにエフェクトを適用する。
    /// Unity内部で自動的に呼び出される。
    /// </summary>
    public override void ModifyMesh(VertexHelper vh)
    {
        // 非アクティブまたはモードがNoneなら何もしない
        if (!IsActive() || mode == EffectMode.None)
            return;

        // 元の頂点データを取得
        vh.GetUIVertexStream(_verts);
        var baseVerts = new List<UIVertex>(_verts);
        var result = new List<UIVertex>();

        // ====================================================
        // OutlinePlacement に応じた描画順序
        // 
        // Outside : (文字 + アウトライン) → 影 → 本体
        // Inside  : (文字 + 影) → アウトライン → 本体
        // ====================================================

        if (placement == OutlinePlacement.Outside)
        {
            // ------------------------------
            // 1. 文字 + アウトライン
            // ------------------------------
            List<UIVertex> current = new List<UIVertex>(baseVerts);
            if (mode == EffectMode.Outline || mode == EffectMode.ShadowAndOutline)
                current.AddRange(AddOutline(baseVerts, outlineColor, outlineThickness));

            // ------------------------------
            // 2. 全体に影を追加
            // ------------------------------
            if (mode == EffectMode.Shadow || mode == EffectMode.ShadowAndOutline)
                result.AddRange(AddShadow(current, shadowColor, shadowOffset * shadowThickness));

            // ------------------------------
            // 3. 文字＋アウトラインを最前面に追加
            // ------------------------------
            result.AddRange(current);
        }
        else // Inside
        {
            // ------------------------------
            // 1. 文字 + 影
            // ------------------------------
            List<UIVertex> current = new List<UIVertex>(baseVerts);
            if (mode == EffectMode.Shadow || mode == EffectMode.ShadowAndOutline)
                current.AddRange(AddShadow(baseVerts, shadowColor, shadowOffset * shadowThickness));

            // ------------------------------
            // 2. アウトラインを全体に追加
            // ------------------------------
            if (mode == EffectMode.Outline || mode == EffectMode.ShadowAndOutline)
                result.AddRange(AddOutline(current, outlineColor, outlineThickness));

            // ------------------------------
            // 3. 文字＋影を最前面に追加
            // ------------------------------
            result.AddRange(current);
        }

        // 常にベース文字（元の文字）は最前面に追加
        result.AddRange(baseVerts);

        // 頂点情報を更新
        vh.Clear();
        vh.AddUIVertexTriangleStream(result);
    }

    // =====================================================================
    // Utility Functions
    // =====================================================================

    /// <summary>
    /// 指定した頂点群にシャドウを追加する。
    /// </summary>
    /// <param name="source">元となる頂点リスト</param>
    /// <param name="color">シャドウの色</param>
    /// <param name="offset">シャドウのずらし量</param>
    private List<UIVertex> AddShadow(List<UIVertex> source, Color color, Vector2 offset)
    {
        var output = new List<UIVertex>(source.Count);
        foreach (var v in source)
        {
            UIVertex vt = v;
            vt.position.x += offset.x;
            vt.position.y += offset.y;
            vt.color = color;
            output.Add(vt);
        }
        return output;
    }

    /// <summary>
    /// 指定した頂点群にアウトラインを追加する。
    /// </summary>
    /// <param name="source">元となる頂点リスト</param>
    /// <param name="color">アウトラインの色</param>
    /// <param name="thickness">太さ（ピクセル単位）</param>
    private List<UIVertex> AddOutline(List<UIVertex> source, Color color, float thickness)
    {
        var output = new List<UIVertex>();

        // 8方向（上下左右＋斜め）にずらして複製
        Vector2[] offsets = new Vector2[]
        {
            new Vector2( thickness,  0),
            new Vector2(-thickness,  0),
            new Vector2( 0,  thickness),
            new Vector2( 0, -thickness),
            new Vector2( thickness,  thickness),
            new Vector2(-thickness,  thickness),
            new Vector2( thickness, -thickness),
            new Vector2(-thickness, -thickness),
        };

        foreach (var off in offsets)
        {
            foreach (var v in source)
            {
                UIVertex vt = v;
                vt.position.x += off.x;
                vt.position.y += off.y;
                vt.color = color;
                output.Add(vt);
            }
        }
        return output;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクター上の変更を即時反映させる。
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
        if (graphic != null)
            graphic.SetVerticesDirty();
    }
#endif
}
