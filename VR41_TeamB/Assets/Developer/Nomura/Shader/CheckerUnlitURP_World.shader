Shader "Custom/CheckerUnlitURP_Complete"
{
    Properties
    {
        _Color1("Color 1", Color) = (0,0,0,1)
        _Color2("Color 2", Color) = (1,1,1,1)
        _TileSize("Tile Size", Float) = 1.0
        _Offset("Offset", Float) = 0.01
    }

        SubShader
    {
        Tags{"RenderPipeline" = "UniversalRenderPipeline"}
        LOD 100

        Pass
        {
            Tags{"LightMode" = "UniversalForward"}
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _Color1;
            float4 _Color2;
            float _TileSize;
            float _Offset;

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 posWS : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 posWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(posWS);
                OUT.posWS = posWS;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // World 座標をタイルサイズで割って floor して Checker を作る
                float3 p = IN.posWS / _TileSize + _Offset; // Offset で原点付近でも2色出る
                float sum = floor(p.x) + floor(p.y) + floor(p.z);
                float checker = fmod(sum, 2);
                return checker < 1 ? _Color1 : _Color2;
            }
            ENDHLSL
        }
    }
}
