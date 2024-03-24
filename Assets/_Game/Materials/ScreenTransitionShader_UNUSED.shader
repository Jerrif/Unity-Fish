Shader "Unlit/ScreenTransitionShaderUNUSED" {
    // used to define properties you can set in the inspector
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "black" {}
        _Cutoff ("Cutoff", Range(0,1.05)) = 0.5 
        _Ease ("Ease", Range(0,200)) = 6
        _Color ("Colour", Color) = (0, 0, 0, 1)
    }
    // defines the rendering pass
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
               the main texture is replaced with a solid color (black)
               the ease value controls how smooth the transition is */
            fixed4 frag (v2f i) : SV_Target {
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

                /* NOTE: gradients used in this shader need to be in linear space (not sRGB),
                   else the start / dark areas go much quicker (roughly square or sqrt)
                   this can be done in PS/Gimp, but I could also use a formula in the shader, something like:
                   float4 destinationTex = srcTex * smoothstep(_Cutout, _Cutout + _CutoutFalloff, _TransitionTex);
                   https://chilliant.blogspot.com/2012/08/srgb-approximations-for-hlsl.html
                   https://gamedev.stackexchange.com/questions/92015/optimized-linear-to-srgb-glsl */
            }
            ENDCG
        }
    }
}

// resources used to make this shader
// https://medium.com/@humhuhhah/implementing-the-making-stuff-look-good-in-unity-fancy-transition-shader-in-unity-a7864177306e
// https://www.youtube.com/watch?v=LnAoD7hgDxw
// https://twitter.com/jumpquestgame/status/1573383293269155840

// (maybe)
// https://www.youtube.com/watch?v=gWe8xWb6prU
// https://www.youtube.com/watch?v=WvvvzupH18s

// ALSO
// https://angrytools.com/gradient/
// https://www.google.com/search?q=TextureLab&rlz=1C1ONGR_enAU1073AU1073&sourceid=chrome&ie=UTF-8