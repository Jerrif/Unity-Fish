// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// https://www.youtube.com/watch?v=T-HXmQAMhG0&t=1s
Shader "Unlit/TestFishShader"
{
    Properties
    {
        // OKAY, so these are kind of like `[SerializeField]`. `Properties` added here show up in the Unity inspector ~on the MATERIAL, not the shader~
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Waterline Mask Texture", 2D) = "white" {}
        // remember, _Color must also be declared in the CGPROGRUM/SulbShadrer
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        // queue=transparent makes the sprites render after the opaque geometry, w/o this you get weird issues w/ transparency
        // DisableBatching=true fixes an issue where if you use unity_ObjectToWorld to do stuff based on obj position,
        //      the same value gets applied to all objects that share the same material (this shader)
        Tags { 
            "RenderType"="Opaque"
            "Queue"="Transparent"
            "DisableBatching" = "True"
        }
        LOD 100

        Pass
        {
            // this adds transparency somehow
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float greyscale(float4 col) {
                float lum = 0.3 * col.r + 0.59 * col.g + 0.11 * col.b;
                return col.rgb = lum;
            }

            // defines what data we're getting for each vertex on the mesh
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // defines what information we're passing into the fragment function
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uvTwo : TEXCOORD1;
            };

            // define `_MainTex` in the scope of our CGPROGRAM (since it's out of scope where you define it right at the top)
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color; // defines the `Property` I added up at the top so it can be used in the frag func
            float _Tween;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;

            // takes appdata struct and returns a v2f
            v2f vert (appdata v)
            {
                v2f o;
                // looks at the position of the vertex on the mesh (typically a vector3)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // pass along the uv as-is to the fragment function
                // dunno what the TRANSFORM_TEX is doing, but this is equivalent to: o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvTwo = TRANSFORM_TEX(v.uv, _MaskTex);
                // o.color = v.color;
                return o;
            }

            // takes our v2f struct and returns a color in the form of a float (fixed?) 4
            fixed4 frag (v2f i) : SV_Target
            {
                // get the color value from the texture (_MainTex), at the provided uv value
                fixed4 col = tex2D(_MainTex, i.uv);
                // fixed4 maskCol = tex2D(_MaskTex, i.uv);

                // float curve = 1-smoothstep(distance(i.uv, float2(.5, 1.8)), 1.5, i.uv.y+.99); // smooth curve, just middle line
                // float2 offset = mul(unity_ObjectToWorld, i.uv.xy * 2 - 1);

                // apparently getting the scale is pretty expensive? Idk if true
                // float scale = length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x)); // scale x axis
                float scale = length(unity_ObjectToWorld._m00_m10_m20); // scale x axis (assumes uniform scaling)

                // TODO: comment/explain all this
                float2 offset = mul(unity_ObjectToWorld, float4(i.uv.xy, 0.0, 0.5) * 2-1);
                // TODO: parameterize these magic numbers
                float curve = 1-step(distance(offset, float2(0, 2.5 * scale)), 2.8 * scale); // mask off the bottom, curved

                // col.rgb += curve;
                // col.b += curve;
                // col.rgb += step(i.uv.y, 0.35);

                // float2 offset = mul(unity_ObjectToWorld, float2(i.uv.xy, curve));
                // float2 offset = i.uv.xy * 2-1;
                // col.b += offset;
                col.b += curve;
                col.rgb += curve * .2;
                
                return col;
            }
            ENDCG
        }
    }
}
