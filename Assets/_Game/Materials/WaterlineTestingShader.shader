// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WaterlineTestingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _WaterlineColor ("Waterline Colour", Color) = (143, 234, 244, 1)
        _UnderwaterColor ("Underwater Colour", Color) = (31, 143, 171, 1)
    }
    SubShader
    {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "DisableBatching" = "True"
        }
        LOD 100

        Pass
        {
            ZWrite off
            Cull off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            half3 ObjectScale() {
                return half3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            float4 _WaterlineColor;
            float4 _UnderwaterColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // o.uvTwo = TRANSFORM_TEX(v.uvTwo, _MaskTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half3 s = ObjectScale();
                float scale = length(unity_ObjectToWorld._m00_m10_m20); // scale x axis (assumes uniform scaling)
                float2 offset = mul(unity_ObjectToWorld, i.uv * 2 - 1);
                // float2 offset = mul(unity_ObjectToWorld, float4(i.uv.xy, 0.0, 0.0) * 2-1);
                // fixed4 offset = mul(unity_ObjectToWorld, float4(i.vertex.xy, 0.0, 1.0));
                // float2 offset = i.uv * 2-1;
                fixed4 col = tex2D(_MainTex, i.uv);
                // float2 offset = mul(unity_ObjectToWorld, float4(i.uv, 0.0, 0.5) * 2-1);
                fixed4 mask = tex2D(_MaskTex, i.uv + offset/scale);
                // fixed4 mask = tex2D(_MaskTex, i.uv + offset/s);
                // fixed4 mask = tex2D(_MaskTex, i.uv);
                

                // col.rgb += mask.rgb;
                col.rgb += mask.r * _WaterlineColor;
                col.rgb += mask.g * _UnderwaterColor;
                return col;
            }
            ENDCG
        }
    }
}
