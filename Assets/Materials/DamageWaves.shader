Shader "Custom/DamageWaves"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _Amplitude ("Amplitude", float) = 30
        _Frequence ("Frequence", float) = 30
        _Attenuation ("Attenuation", float) = 1
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
            float _Amplitude;
            float _Frequence;
            float _Attenuation;

            fixed4 frag (v2f IN) : SV_Target
            {
                float2 offset = float2(sin(IN.vertex.y/_Frequence + _Time[3]) /_Amplitude, 0) * _Attenuation;
                
                fixed4 col = tex2D(_MainTex, IN.uv + offset);
                return col;
            }
            ENDCG
        }
    }
}
