﻿Shader "Custom/sh_WaterDisplacement"
{
    Properties
    {
        [HideInInspector]_MainTex("Texture", 2D) = "white" {}
        _DisplaceTex ("Displacement Texture", 2D) = "white" {}
        _Magnitude ("Magnitude", Range(0,0.05)) = 0
        _DisplaceSpeed ("Displacement Scroll Speed", Range(0,0.1)) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _DisplaceTex;
            float _Magnitude;
            float _DisplaceSpeed;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 disp = tex2D(_DisplaceTex, i.uv + _Time.y*_DisplaceSpeed).xy;
                disp = ((disp * 2) - 1) * _Magnitude;
                

                fixed4 col = tex2D(_MainTex, i.uv + disp);
                return col;
            }
            ENDCG
        }
    }
}
