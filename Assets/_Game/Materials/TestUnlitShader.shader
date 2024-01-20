Shader "Unlit/TestUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

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
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // fixed4 col = tex2D(_MainTex, i.vertex);
                col.x = i.uv.x;
                col.y = i.uv.y;
                // col.z = i.uv.y;
                col.z = 1;
                // col.xy = i.uv;
                // col.xyz = i.vertex;
                // col.xy = i.uv;
                // col.yz = i.uv;
                // col.xz = i.uv;
                return col;
            }
            ENDCG
        }
    }
}
