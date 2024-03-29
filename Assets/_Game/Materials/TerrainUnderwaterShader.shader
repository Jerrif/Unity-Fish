Shader "Unlit/TerrainUnderwaterShader" {
    // used to define properties you can set in the inspector
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _WaterlineColor ("Waterline Colour", Color) = (143, 234, 244, 1)
        _UnderwaterColor ("Underwater Colour", Color) = (31, 143, 171, 1)
        _UnderwaterOpacity ("Underwater Opacity", Range(0,1)) = 1
        _WaterHeight ("Water Height", Float) = 3.75
        _WaveMagnitude ("Wave Magnitude", Range(0, 0.2)) = 0.05
    }
    // defines the rendering pass
    SubShader {
        // queue=transparent makes the sprites render after the opaque geometry, w/o this you get weird issues w/ transparency
        Tags { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "PreviewType"="Plane"
            "DisableBatching" = "True"
        }

        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            // this adds transparency somehow
            Blend SrcAlpha OneMinusSrcAlpha

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
                float2 uvWorld : TEXCOORD1; // world space position (?)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _WaterlineColor;
            half4 _UnderwaterColor;
            half _UnderwaterOpacity;
            float _WaterHeight;
            float _WaveMagnitude;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvWorld = UnityWorldToViewPos(v.vertex);
                return o;
            }

            fixed4 fragWithIf (v2f i) : SV_Target {
                // this is a much easier to read version of the below `lerp` based code
                // (only the basic water, not including the waterline or waves)
                fixed4 col = tex2D(_MainTex, i.uv);
                if (i.uvWorld.y < _WaterHeight) {
                    col.a *= _UnderwaterOpacity;
                    return col *= _UnderwaterColor;
                }
                return col;
            }

            fixed4 fragNoWaves (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed height = step(_WaterHeight, i.uvWorld.y);

                fixed waterline = height - step(_WaterHeight+0.03, i.uvWorld.y);
                col.rgb = lerp(col.rgb, _WaterlineColor, waterline); // opaque waterline
                
                col.rgb *= lerp(_UnderwaterColor, 1, height);
                col.a *= lerp(_UnderwaterOpacity, 1, height);
                return col;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed offset = i.uvWorld.y + (sin(i.uvWorld.x * 3 + _Time.y) * _WaveMagnitude);
                fixed height = step(_WaterHeight, offset);
                fixed waterline = height - step(_WaterHeight + 0.03, offset);

                col.rgba += waterline * _WaterlineColor; // transparent waterline
                col.rgb *= lerp(_UnderwaterColor, 1, height);
                col.a *= lerp(_UnderwaterOpacity, 1, height);
                return col;
            }
            ENDCG
        }
    }
}
