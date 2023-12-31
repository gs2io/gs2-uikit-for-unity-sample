Shader "Unlit/GrayScaleUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "w$$anonymous$$te" { }
        _Color ("Tint", Color) = (1.000000,1.000000,1.000000,1.000000)
        _StencilComp ("Stencil Comparison", Float) = 8.000000
        _Stencil ("Stencil ID", Float) = 0.000000
        _StencilOp ("Stencil Operation", Float) = 0.000000
        _StencilWriteMask ("Stencil Write Mask", Float) = 255.000000
        _StencilReadMask ("Stencil Read Mask", Float) = 255.000000
        _ColorMask ("Color Mask", Float) = 15.000000
        [Toggle(UNITY_UI_ALPHACLIP)]  _UseUIAlphaClip ("Use Alpha Clip", Float) = 0.000000
    }
    SubShader
    {
        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _MainTex_ST;

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                half4 texcol = tex2D(_MainTex, i.uv);
                texcol.rgb = dot(texcol.rgb, float3(0.3, 0.59, 0.11));
                return texcol;
            }
            ENDCG

        }
    }
    Fallback "VertexLit"
}