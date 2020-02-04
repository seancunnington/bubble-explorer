Shader "Custom/sh_Clip_Dither"
{
    Properties
    {
        _ClipCheck ("Clip Check", Range(0,1)) = 1.0
        _Color ("Color", Color) = (0,0,0,1)
        //_MainTex ("Main Texture", 2D) = "white" {}
        _SCREENSPACE_CUTOFF_TEXTURE ("_SCREENSPACE_CUTOFF_TEXTURE", 2D) = "white" {}
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

        #pragma surface surf Standard vertex:vert fullforwardshadows 
        #pragma target 3.0

        //sampler2D _MainTex;
        sampler2D _SCREENSPACE_CUTOFF_TEXTURE;
        float3 cutoff;
        float _ClipCheck;
        fixed4 _Color;
        half3 _Emission;
        float4 _PlaneTop;
        float4 _PlaneBottom;
        float _ClipDistanceOffsetTop;
        float _ClipDistanceOffsetBelow;
        float4 _CutoffColor;

        // For Dithering
        float3 wVertex;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
            float4 screenPos;
            float facing : VFACE;
            float dither;
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            wVertex = mul( unity_ObjectToWorld, v.vertex);
            o.dither = lerp(1.0, 0.0, (length(wVertex.xyz - _WorldSpaceCameraPos.xyz) - 1.0) / 5.0);
        }

        void surf (Input i, inout SurfaceOutputStandard o)
        { 

            // Dither first, then Plane Clip second (will test if this secondary clipping is even necessary)

            // DITHERING //
            // Apply screen space cutoff
            float2 screenUV = i.screenPos.xy / i.screenPos.w;
            float screenSpaceCutoffTiling = 5.0;
            screenUV *= float2(screenSpaceCutoffTiling, screenSpaceCutoffTiling);
            cutoff = tex2D(_SCREENSPACE_CUTOFF_TEXTURE, screenUV).rgb;
            clip(max(max(cutoff.r, cutoff.g), cutoff.b) * i.dither);

/*
            // PLANE CLIPPING //
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
*/
            // Regular color stuff
            //fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            fixed4 col = _Color;
            o.Albedo = col.rgb;
            //o.Emission = lerp(_CutoffColor, _Emission, facing);
        }
        ENDCG
    }
    FallBack "Standard"
}
