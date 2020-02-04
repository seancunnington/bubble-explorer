Shader "Custom/sh_Scan"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        [Header(Wave)]
        _WaveDistance ("Distance from player", float) = 10
        _WaveTrail ("Length of the trail", Range(0,5)) = 1
        _LeadColor ("Lead Color", Color) = (0, 0, 0, 1)
        _DistanceColor ("Mid Color", Color) = (0, 0, 0, 1)
        _TrailColor ("Trail Color", Color) = (0, 0, 0, 1)
        _HBarColor ("Horizontal Bar Color", Color) = (0.5, 0.5, 0.5, 0)
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
            float4 _DistanceColor;
            float _MidPoint;
            float4 _TrailColor;
            float4 _HBarColor;

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
                // get depth from depth texture
                float depth = tex2D(_CameraDepthTexture, i.uv).r;
                // linear depth between camera and far clipping plane
                depth = Linear01Depth(depth);
                // depth as distance from camera in units
                depth = depth * _ProjectionParams.z;

                // Get the source color
                fixed4 source = tex2D(_MainTex, i.uv);
                // skip wave and return source color if we're at the skybox
                if (depth >= _ProjectionParams.z)
                    return source;

                // wave color
                float4 waveColor = float4(0,0,0,0);

                // Calculate the wave
                float waveFront = step(depth, _WaveDistance);
                float waveTrail = smoothstep(_WaveDistance - _WaveTrail, _WaveDistance, depth);
                float wave = waveFront * waveTrail;


                // Blend the wave colors
                waveColor = lerp(_TrailColor, _LeadColor, waveTrail) + horizBars(i.uv) * _HBarColor;

                float4 waveColor_temp = waveColor;

                
                // Mix wave into source color
                fixed4 col = lerp(source, waveColor, wave);

                return col;
            }
            ENDCG
        }
    }
}
