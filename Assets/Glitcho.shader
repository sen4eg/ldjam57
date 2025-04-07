Shader "Custom/GlitchEffect"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _GlitchIntensity ("Glitch Intensity", Range(0,1)) = 0.5
        _Speed ("Speed", Range(0,10)) = 1.0
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float _GlitchIntensity;
            uniform float _Speed;

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                // Horizontal offset
                float yOffset = rand(float2(uv.y, floor(_Time.y * _Speed))) * _GlitchIntensity * 0.05;
                uv.x += yOffset;

                // Occasional color shift
                float glitch = rand(float2(floor(_Time.y * _Speed), 0.0));
                float shift = step(0.95, glitch) * _GlitchIntensity * 0.05;

                float r = tex2D(_MainTex, uv + float2(shift, 0)).r;
                float g = tex2D(_MainTex, uv).g;
                float b = tex2D(_MainTex, uv - float2(shift, 0)).b;

                return float4(r, g, b, 1);
            }
            ENDCG
        }
    }
}