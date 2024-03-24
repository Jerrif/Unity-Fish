Shader "Unlit/ScreenTransitionShader" {
    // used to define properties you can set in the inspector
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "black" {}
        _Cutoff ("Cutoff", Range(0,1.05)) = 0.5 
        _Ease ("Ease", Range(0,200)) = 6
        _Color ("Colour", Color) = (0, 0, 0, 1)
		[MaterialToggle] _Distort("Distort", Float) = 0
    }
    SubShader {
        Tags{ "Queue"="Overlay" "RenderType"="Transparent" }

        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragSimple

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
            int _Distort;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            /* this shader takes a texture (the "main texture") and a mask texture
               if the mask texture's red channel is less than the cutoff value,
               the main texture is replaced with a solid color (_Color) */
            fixed4 fragSimple (v2f i) : SV_Target {
                fixed4 mask = tex2D(_MaskTex, i.uv);

                if (mask.r < _Cutoff)
                    return _Color;

                return tex2D(_MainTex, i.uv);
            }

            fixed4 fragFade (v2f i) : SV_Target {
                fixed4 mask = tex2D(_MaskTex, i.uv);
                float cutoff = _Cutoff + _Cutoff / _Ease;

                // we start by grabbing the color from the main texture
                float4 col = tex2D(_MainTex, i.uv);

                /* we use lerp to blend the main color with the mask color
                   lerp takes three values: start, end, and a value between 0 and 1
                   if the mask's r is less than the cutoff, we want the final color to be the masked color
                   if the mask's r is greater than the cutoff, we want the final color to be the main color
                   we use saturate to clamp the ease value to between 0 and 1 */
                float4 c;
                c = lerp(
                    col,       // start color
                    _Color,    // end color (mask color)
                    saturate((cutoff - mask.r) * _Ease) // the value between 0 and 1
                    );

                return c;
            }

            // this one was straight from the `Makin Stuff Look Good` video.
            fixed4 fragDistort(v2f i) : SV_Target {
                fixed4 mask = tex2D(_MaskTex, i.uv);

                fixed2 direction = float2(0, 0);
                if(_Distort)
                    direction = normalize(float2((mask.r - 0.5) * 2, (mask.g - 0.5) * 2));

                fixed4 col = tex2D(_MainTex, i.uv + _Cutoff * direction);

                if (mask.b < _Cutoff)
                    return col = lerp(col, _Color, _Ease);

                return col;
            }					
            ENDCG
        }
    }
}
