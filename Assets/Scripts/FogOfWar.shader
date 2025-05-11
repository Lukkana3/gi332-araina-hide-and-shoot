Shader "Custom/FogOfWar"
{
    Properties
    {
        _Color ("Fog Color", Color) = (0, 0, 0, 1)
        _Center ("Light Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Light Radius", Float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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

            fixed4 _Color;
            float4 _Center;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.uv, _Center.xy);
                float alpha = smoothstep(_Radius, _Radius * 1.5, dist);
                return fixed4(_Color.rgb, alpha);
            }
            ENDCG
        }
    }
}
