// make a displacement map?
// https://photoshopcontest.com/tutorials/26/displacement-water.html
// https://www.youtube.com/watch?v=kpBnIAPtsj8

Shader "Unlit/WaterShader"
{
    Properties
    {
        // OKAY, so these are kind of like `[SerializeField]`. `Properties` added here show up in the Unity inspector ~on the MATERIAL, not the shader~
        _MainTex ("Texture", 2D) = "white" {}
        _DisplaceTex("Displacement Texture", 2D) = "white" {}
        _Magnitude("Magnitude", Range(0, 1)) = 0.1
        // remember, _Color must also be declared in the CGPROGRUM/SulbShadrer
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque"
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
            };

            // define `_MainTex` in the scope of our CGPROGRAM (since it's out of scope where you define it right at the top)
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DisplaceTex;
            float4 _DisplaceTex_ST;
            float _Magnitude;
            float4 _Color; // defines the `Property` I added up at the top so it can be used in the frag func

            // takes appdata struct and returns a v2f
            v2f vert (appdata v)
            {
                v2f o;
                // looks at the position of the vertex on the mesh (typically a vector3)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // pass along the uv as-is to the fragment function
                // dunno what the TRANSFORM_TEX is doing, but this is equivalent to: o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // takes our v2f struct and returns a color in the form of a float (fixed?) 4
            fixed4 frag (v2f i) : SV_Target
            {
                // get the color value from the texture (_MainTex), at the provided uv value
                // fixed4 col = tex2D(_MainTex, i.uv);
                // col *= _Color;
                // col.gb *= i.uv;


                // this was from the hearthstone thingo, it makes a scrolling effect
				// float2 distScroll = float2(_Time.x, _Time.x);
				// fixed2 dist = (tex2D(_DistTex, i.uv + distScroll).rg - 0.5) * 2;
				// fixed distMask = tex2D(_DistMask, i.uv)[0];
				// fixed4 col = tex2D(_MainTex, i.uv + dist * distMask * 0.025);

                // TODO: put the blur on it as well (from the same video)

                // float2 scroll = float2(frac(_Time.x), frac(_Time.x));
                float2 scroll = float2(_Time.x, _Time.x);
                // float2 disp = tex2D(_DisplaceTex, i.uv + scroll).rg;
                float2 disp = tex2D(_DisplaceTex, i.uv).rg;
                disp = ((disp * 2) - 1) * _Magnitude;

                // up/down movement on the water
                disp.x += _Time.x;
                disp.y += _CosTime.x;

                

                // float4 col = tex2D(_MainTex, i.uv + disp + sin(scroll));
                float4 col = tex2D(_MainTex, i.uv + disp);
                // col.x += sin(_Time.x);
                // col.w += _SinTime.x;

                // https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html
                // col.x += unity_WorldToObject;

                // hmmmmmm
                // float2 offset = mul(unity_ObjectToWorld, col);
                // col.b = offset.xy;

                return col;
            }
            ENDCG
        }
    }
}
