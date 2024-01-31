Shader "Unlit/WaterlineShader"
{
    Properties
    {
        // OKAY, so these are kind of like `[SerializeField]`. `Properties` added here show up in the Unity inspector ~on the MATERIAL, not the shader~
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Waterline Mask", 2D) = "white" {}
        // remember, these properties must also be declared in the CGPROGRUM/SulbShadrer
        _WaterlineColor ("Waterline Colour", Color) = (143, 234, 244, 1) // these are really good default values
        _WaterlineOpacity ("Waterline Opacity", Range(0,1)) = 0
        _UnderwaterColor ("Underwater Colour", Color) = (31, 143, 171, 1)
        _UnderwaterOpacity ("Underwater Opacity", Range(0,1)) = 1
        // honstly, scale & offset are the same as the already-included `Tiling` & `Offset` from a sampler2D
        _Scale ("X Y Scale", Vector) = (1, 1, 0, 0)
        _Offset ("X Y Offset", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        // queue=transparent makes the sprites render after the opaque geometry, w/o this you get weird issues w/ transparency
        // DisableBatching=true fixes an issue where if you use unity_ObjectToWorld to do stuff based on obj position,
        //      the same value gets applied to all objects that share the same material (this shader)
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
                float2 uvMask : TEXCOORD1;
            };

            // define `_MainTex` in the scope of our CGPROGRAM (since it's out of scope where you define it right at the top)
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            // defines the `Property` I added up at the top so it can be used in the frag func
            float4 _WaterlineColor;
            float4 _UnderwaterColor;
            float _WaterlineOpacity;
            float _UnderwaterOpacity;
            float2 _Scale;
            float2 _Offset;

            // takes appdata struct and returns a v2f
            v2f vert (appdata v)
            {
                v2f o;
                // looks at the position of the vertex on the mesh (typically a vector3)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // pass along the uv as-is to the fragment function
                // equivalent to: o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // this is what allows the mask texture to remain at 0 rotation, regardless of the main tex
                float2 maskUV = UnityObjectToWorldDir(v.vertex);
                o.uvMask = TRANSFORM_TEX(maskUV, _MaskTex);
                o.uvMask.xy *= _Scale.xy;
                return o;
            }

            // takes our v2f struct and returns a color in the form of a float (fixed?) 4
            fixed4 frag (v2f i) : SV_Target
            {
                // get the color value from the texture (_MainTex), at the provided uv value
                fixed4 o = fixed4(0, 0, 0, 1);
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 mask = tex2D(_MaskTex, i.uvMask + _Offset);

                // WOOOOW this is how you make the _UnderwaterColor opaque. WTF I even tried something like this earlier!
                col.rgb = lerp(col.rgb, _UnderwaterColor, mask.g * _UnderwaterOpacity);
                col.rgb = lerp(col.rgb, _WaterlineColor, mask.r * _WaterlineOpacity);
                col.rgb += mask.r * _WaterlineColor;

                // NOTE: this is how I did it before; it lightens the colour of the under/water, and is transparent.
                // I don't know how transparent it is (50%?), but it does look really nice too. Not sure which is better
                // col.rgb += mask.r * _WaterlineColor;
                // col.rgb += mask.g * _UnderwaterColor;

                return col;
            }
            ENDCG
        }
    }
}
