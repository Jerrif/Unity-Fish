Shader "Unlit/ScreenTransitionShader" {
    // used to define properties you can set in the inspector
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Cutoff", Range(0,1)) = 0.5 
        _Color ("Colour", Color) = (0, 0, 0, 1)
    }
    // defines the rendering pass
    SubShader {
        Tags { "RenderType"="Opaque" }
        // Tags { "RenderType"="Transparent" "Queue"="Overlay+50" }

        ZTest Off
        ZWrite Off
        Lighting Off
        Cull Off

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



            // TODO:
            // https://medium.com/@humhuhhah/implementing-the-making-stuff-look-good-in-unity-fancy-transition-shader-in-unity-a7864177306e
            // https://www.youtube.com/watch?v=LnAoD7hgDxw
            // https://twitter.com/jumpquestgame/status/1573383293269155840
            
            // (maybe)
            // https://www.youtube.com/watch?v=gWe8xWb6prU
            // https://www.youtube.com/watch?v=WvvvzupH18s


            // ALSO
            // https://angrytools.com/gradient/
            // https://www.google.com/search?q=TextureLab&rlz=1C1ONGR_enAU1073AU1073&sourceid=chrome&ie=UTF-8



            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;
            fixed4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // if (i.uv.x < _Cutoff)
                if (0.5 - abs(i.uv.y - 0.5) < abs(_Cutoff) * 0.5)
                    return _Color;

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
