Shader "Custom/sh_CharacterClipping_Unlit"
{
    Properties
    {
        _ClipCheck ("Clip Check", Range(0,1)) = 1.0
        _Color ("Color", Color) = (0,0,0,1)
        _ClipDistanceOffsetTop ("Clip Distance Offset - Above", float) = 0.4
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

        Pass
        {
            
            
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                //float facing : VFACE;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                //float facing : VFACE;
            };

            fixed4 _Color;
            float _ClipCheck;
            half3 _Emission;
            float4 _PlaneTop;
            float4 _PlaneBottom;
            float _ClipDistanceOffsetTop;
            float _ClipDistanceOffsetBelow;
            float4 _CutoffColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
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
                fixed facing_in = facing * 0.5 + 0.5;
                

                // Set up the color
                fixed4 col = _Color;

                // Set up the emission
                //col *= lerp(_CutoffColor, _Emission, facing_in);

                return col;
            }
            ENDCG
        }
    }
}
