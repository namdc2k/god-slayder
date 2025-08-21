Shader "Pancake/Mobile/Unlit/Cutout Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Cutoff", float) = 0.5
        _OutlineColor ("Outline Color", Color) = (0.2,0.2,0.2,1)
        _Outline ("Outline Width", float) = 0.02
        _EnableFlicker ("Enable Flicker", Float) = 0
        _FlickerColor ("Flicker Color", Color) = (1,0,0,1)
        _FlickerDuration ("Flicker Duration", float) = 0.1
    }
    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest" "RenderType"="TransparentCutout"
        }
        LOD 100

        // First pass: Render outline
        Pass
        {
            Name "OUTLINE"
            Tags
            {
                "LightMode"="Always"
            }
            Cull Front // Render back-faces for the outline
            ZWrite Off
            Offset 15, 15

            CGPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            uniform float _Outline;
            uniform float4 _OutlineColor;

            v2f vertOutline(appdata v)
            {
                // Expand vertices along their normal
                v2f o;
                float3 normalDirection = normalize(v.normal) * _Outline;
                o.pos = UnityObjectToClipPos(v.vertex + float4(normalDirection, 0));
                return o;
            }

            fixed4 fragOutline(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // Second pass: Main texture with cutout
        Pass
        {
            Name "CUTOUT"
            Tags
            {
                "LightMode"="Always"
            }
            Cull Off
            ZWrite On
            AlphaTest Greater 0.5

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;
            float4 _FlickerColor;
            float _FlickerDuration;
            float _EnableFlicker;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.uv = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = mul((float3x3)unity_WorldToObject, v.normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - _Cutoff);

                if (_EnableFlicker > 0)
                {
                    // Calculate flicker effect
                    float flicker = sin(_Time.y * (3.14159 / _FlickerDuration)) * 0.5 + 0.5;
                    col.rgb = lerp(col.rgb, _FlickerColor.rgb, flicker);
                }

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
