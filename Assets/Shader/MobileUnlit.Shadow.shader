Shader "Pancake/Mobile/Unlit/Shadow"
{

    Properties
    {
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _PlaneHeight ("planeHeight", Float) = 0
        _ShadowDirection ("Shadow Direction", Vector) = (1, 1, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }

        // shadow color
        Pass
        {

            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil
            {
                Ref 0
                Comp Equal
                Pass IncrWrap
                ZFail Keep
            }

            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            uniform float4 _ShadowColor;
            uniform float _PlaneHeight;
            uniform float4 _ShadowDirection;

            struct vsOut
            {
                float4 pos : SV_POSITION;
            };

            vsOut vert(appdata_base v)
            {
                vsOut o;

                float4 vPosWorld = mul(unity_ObjectToWorld, v.vertex);
                //float4 lightDirection = -normalize(_WorldSpaceLightPos0); // for has lighting source
                float4 lightDirection = -normalize(_ShadowDirection);

                float opposite = vPosWorld.y - _PlaneHeight;
                float cosTheta = -lightDirection.y;
                float hypotenuse = opposite / cosTheta;
                float3 vPos = vPosWorld.xyz + lightDirection.xyz * hypotenuse;

                o.pos = mul(UNITY_MATRIX_VP, float4(vPos.x, _PlaneHeight, vPos.z, 1));

                return o;
            }


            fixed4 frag(vsOut i) : COLOR
            {
                return _ShadowColor;
            }
            ENDCG

        }
    }
}