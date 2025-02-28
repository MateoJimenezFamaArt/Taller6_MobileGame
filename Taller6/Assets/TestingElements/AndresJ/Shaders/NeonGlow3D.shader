Shader "Unlit/NeonGlow3D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range(0,10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            Blend One OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _EmissionColor;
            fixed _GlowIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= col.a;
                col.rgb = (col.rgb * col.a) + (_EmissionColor.rgb * _GlowIntensity * col.a);
                return col;
            }
            ENDCG
        }
    }
}
