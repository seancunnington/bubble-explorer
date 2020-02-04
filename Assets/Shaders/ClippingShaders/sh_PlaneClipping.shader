Shader "Custom/sh_PlaneClipping"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Smoothness ("Smoothness", float) = 0
        [HDR] _Emission ("Emission", Color) = (0,0,0)
    }
    SubShader
    {
        // This material is transparent and is rendered at the same time as other opaque geometry
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}

        // Render faces regardless if they point towards the camera or away from it
        Cull Off

        CGPROGRAM

        #pragma surface surf Standard vertex:vert alpha:fade nolightmap
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        half3 _Emission;
        half _Smoothness;

        struct Input {
            float2 uv_MainTex;
            float4 screenPos;
            float3 worldPos;
            float eyeDepth;
        };

        sampler2D_float _CameraDepthTexture;
        float4 _CameraDepthTexture_TexelSize;
        float _InvFade;

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            COMPUTE_EYEDEPTH(o.eyeDepth);
        }

        void surf (Input i, inout SurfaceOutputStandard o)
        { 
            // Regular color stuff
            fixed4 col = tex2D(_MainTex, i.uv_MainTex) * _Color;
            o.Albedo = col.rgb;
            o.Smoothness = _Smoothness;

            // Finding the Depth Texture
            float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));
            float sceneZ = LinearEyeDepth(rawZ);
            float partZ = i.eyeDepth;

            float fade = 1.0;
            if ( rawZ > 0.0 )   // Make sure the depth texture exists
                fade = saturate(_InvFade * (sceneZ - partZ));

            o.Alpha = col.a * fade;

        }
        ENDCG
    }
    FallBack "Standard"
}
