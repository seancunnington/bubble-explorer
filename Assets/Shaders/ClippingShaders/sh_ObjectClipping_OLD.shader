Shader "Custom/sh_ObjectClipping_OLD"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Smoothness ("Smoothness", float) = 0
        _ClipDistanceOffset ("Clip Distance Offset", float) = 0
        [HDR] _Emission ("Emission", Color) = (0,0,0)

        [HDR] _CutoffColor ("Cutoff Color", Color) = (1,0,0,0)
    }
    
    SubShader
    {
        // This material is completely non-transparent and is rendered at the same time as other opaque geometry
        Tags { "RenderType"="Opaque" "Queue" = "Geometry"}

        // Render faces regardless if they point towards the camera or away from it
        Cull Off

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        half3 _Emission;
        half _Smoothness;
        float4 _Plane;
        float _ClipDistanceOffset;
        float4 _CutoffColor;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
            float facing : VFACE;
        };

        void surf (Input i, inout SurfaceOutputStandard o)
        { 
            
            // Calculate the signed distance to plane
            float distance = dot(i.worldPos, _Plane.xyz);
            distance = distance + _Plane.w;

            // Clip at the plane's intersection
            clip(-distance + _ClipDistanceOffset);

            // Identify inside and outside faces
            float facing = i.facing * 0.5 + 0.5;

            // Regular color stuff
            fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            col *= _Color;
            o.Albedo = col.rgb;
            o.Smoothness = _Smoothness;

            o.Emission = lerp(_CutoffColor, _Emission, facing);
            
        }
        ENDCG
    }
    FallBack "Standard"
}
