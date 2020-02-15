Shader "Custom/sh_Scan_2"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        [Header(Wave)]
        _WaveDistance ("Distance from player", float) = 10
        _WaveTrail ("Length of the trail", Range(0,20)) = 1
        _LeadColor ("Lead Color", Color) = (0, 0, 0, 1)
        _MidColor ("Mid Color", Color) = (0, 0, 0, 1)
        _TrailColor ("Trail Color", Color) = (0, 0, 0, 1)
        _HBarColor ("Horizontal Bar Color", Color) = (0.5, 0.5, 0.5, 0)
        _LeadSharp ("Lead Sharp", float) = 1
        _Opacity ("Opacity", float) = 1
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // The rendered screen so far
            sampler2D _MainTex;
            // The depth texture
            sampler2D _CameraDepthTexture;

            // Variables to control the wave
            float _WaveDistance;
            float _WaveTrail;
            float4 _LeadColor;
            float4 _MidColor;
            float _MidPoint;
            float4 _TrailColor;
            float4 _HBarColor;
            float _LeadSharp;
            float _Opacity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 horizBars (float2 p)
            {
                return 1 - saturate(round(abs(frac(p.y * 100) * 2)));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Screen source color
                fixed4 sourceCol = tex2D(_MainTex, i.uv);
                // Scanner wave color
                fixed4 scanCol = 0;


                // get depth from depth texture
                float depth = tex2D(_CameraDepthTexture, i.uv).r;
                // linear depth between camera and far clipping plane
                depth = Linear01Depth(depth);
                // depth as distance from camera in units
                depth = depth * _ProjectionParams.z;


                // skip wave and return source color if we're at the skybox
                if (depth >= _ProjectionParams.z)
                    return sourceCol;

                // wave color
                float4 waveColor = float4(0,0,0,0);

                // Calculate the wave
                float waveFront = step(depth, _WaveDistance);

                if (depth < _WaveDistance && depth > _WaveDistance - _WaveTrail)
                {
                    float diff = 1 - (_WaveDistance - depth) / (_WaveTrail);
                    half4 edge = lerp(_MidColor, _LeadColor, pow(diff, _LeadSharp));
                    scanCol = lerp(_TrailColor, edge, diff) + horizBars(i.uv) * _HBarColor;
                    scanCol *= diff;
                    // 1 is fully opaque, 0 is fully transparent.
                    scanCol *= _Opacity;
                }

                return sourceCol + scanCol;

            }
            ENDCG
        }
    }
}
