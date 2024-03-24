Shader "Unlit/ScreenTransitionShader" {
    // used to define properties you can set in the inspector
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "black" {}
        _Cutoff ("Cutoff", Range(0,1.05)) = 0.5 
        _Color ("Colour", Color) = (0, 0, 0, 1)
    }
    SubShader {
        Tags{ "Queue"="Overlay" "RenderType"="Transparent" }

        ZTest Off
        ZWrite Off
        Lighting Off
        // Cull Off

        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            struct v2f {
                float2 uv : TEXCOORD0; // clip space position
                float4 vertex : SV_POSITION; // texture coordinate
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4 _MainTex_ST;
            float _Cutoff;
            fixed4 _Color;
            float _Ease;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            /* this shader takes a texture (the "main texture") and a mask texture
               if the mask texture's red channel is less than the cutoff value,
               the main texture is replaced with a solid color (_Color) */
            fixed4 frag (v2f i) : SV_Target {
                fixed4 mask = tex2D(_MaskTex, i.uv);
                float cutoff = _Cutoff;

                if (mask.r < _Cutoff)
                    return _Color;

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
