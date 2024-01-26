Shader "Unlit/WaterlineShader"
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

            // defines what data we're getting for each vertex on the mesh
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uvTwo : TEXCOORD1;
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
                o.uvTwo = TRANSFORM_TEX(v.uvTwo, _MaskTex);
                return o;
            }

            // takes our v2f struct and returns a color in the form of a float (fixed?) 4
            fixed4 frag (v2f i) : SV_Target
            {
                // get the color value from the texture (_MainTex), at the provided uv value
                fixed4 c = fixed4(0.0,0.0,0.0,0.0);
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 maskCol = tex2D(_MaskTex, i.uvTwo);

                
                float2 a = step(maskCol.r, i.uv);


                c.rgba = col;
                c.r = step(maskCol.g, maskCol.r);
                // c.r = i.uv;
                // c = lerp(col, maskCol, 1.0);

                // col.rgba += maskCol.rgba;
                // col.rgb += maskCol.rgb;
                // col.rgb += maskCol.g;
                col.r = maskCol.r;
                col.b = maskCol.g;
                // return maskCol;
                
                return c;
                // return fixed4(0.0, 1.0, 0.0, 1.0);
                // return col;
            }
            ENDCG
        }
    }
}
