Shader "Sprites/SpriteGhostShader"
{
	Properties
	{
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("MaskTexture", 2D) = "white" {}
        [PerRendererData]_Speed ("Speed", Range(0, 10)) = 2
        [PerRendererData]_Scale ("Scale", Range(0.001, 10)) = 2
        [PerRendererData]_BlendAmount ("Blend Amount", Range(0, 1)) = 0
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
		[PerRendererData] _Displacement ("ChromaticDisplacement", Float) = 0
		[PerRendererData] _Tears ("Tears", Int) = 10
		[PerRendererData] _TearsDistance ("TearsDistance", Range(-0.1,0.1)) = 0
	}
	SubShader
	{
		Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "CanUseSpriteAtlas"="True"
        }
		LOD 100

        Cull Off
        Lighting Off
        ZWrite Off

		Pass
		{
            Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
                float4 color : COLOR;
			};

			sampler2D _MainTex;
            sampler2D _MaskTex;
			float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            sampler2D _NoiseTex;
            float _Speed;
            float _Scale;
            float4 _OutlineColor;
            float _BlendAmount;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

                float i1 = _Speed * _Time.x;
                float i2 = 2 * _Speed * _Time.x;
                float i3 = _Speed * _Time.y;
                float noise = tex2D(_NoiseTex, fixed2(i1, i1)).r;
                float noise2 = tex2D(_NoiseTex, fixed2(i2, i1)).r;
                float noise3 = tex2D(_NoiseTex, fixed2(i3, i2)).r;

                if (col.a == 0) {
                    fixed4 pixelUp = tex2D(_MainTex, i.uv + fixed2(0, 1) * _MainTex_TexelSize.xy);
                    fixed4 pixelDown = tex2D(_MainTex, i.uv + fixed2(0, -1) * _MainTex_TexelSize.xy);
                    fixed4 pixelLeft = tex2D(_MainTex, i.uv + fixed2(1, 0) * _MainTex_TexelSize.xy);
                    fixed4 pixelRight = tex2D(_MainTex, i.uv + fixed2(-1, 0) * _MainTex_TexelSize.xy);

                    if ((pixelUp + pixelDown + pixelLeft + pixelRight).a != 0) {
                        col = _OutlineColor;
                    }
                }

                float2 uv = i.uv;
                uv.y += _Time.x * _Speed + noise2 * 0.3 - 0.15;
                fixed4 maskCol = tex2D(_MaskTex, uv * _Scale + (1 * noise));
                float origA = col.a * maskCol.a;

                col = lerp(col, i.color, _BlendAmount - 0.3 + 0.6 * noise3);

                col.a *= origA;
                col.rgb *= origA;
				return col;
			}
			ENDCG
		}
	}

}
