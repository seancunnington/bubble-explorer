Shader "Custom/sh_ObjectClipping"
{
    Properties
    {
        _ClipCheck ("Clip Check", Range(0,1)) = 1.0
        _Color ("Color", Color) = (0,0,0,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Smoothness ("Smoothness", float) = 0
        _ClipDistanceOffsetTop ("Clip Distance Offset", float) = 0.4
        _ClipDistanceOffsetBelow ("Clip Distance Offset - Below", float) = 0
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
        float _ClipCheck;
        fixed4 _Color;
        half3 _Emission;
        half _Smoothness;
        float4 _PlaneTop;
        float4 _PlaneBottom;
        float _ClipDistanceOffsetTop;
        float _ClipDistanceOffsetBelow;
        float4 _CutoffColor;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
            float facing : VFACE;
        };

        void surf (Input i, inout SurfaceOutputStandard o)
        { 
            
            // Calculate the signed distance to both planes
            // Top Plane
            float distance = dot(i.worldPos, _PlaneTop.xyz);
            distance = distance + _PlaneTop.w;
            // Bottom Plane
            float distance2 = dot(i.worldPos, _PlaneBottom.xyz);
            distance2 = distance2 + _PlaneBottom.w;

            if (_ClipCheck != 0){  // Can turn clipping off for easy viewing
                // Clip at the both planes' intersections
                clip(-distance + _ClipDistanceOffsetTop);
                clip(-(-distance2 + _ClipDistanceOffsetBelow));
            }

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
