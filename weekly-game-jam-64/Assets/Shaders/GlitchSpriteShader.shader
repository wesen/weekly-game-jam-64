Shader "Sprites/GlitchSpriteShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Displacement ("ChromaticDisplacement", Float) = 0
		_Tears ("Tears", Int) = 10
		_TearsDistance ("TearsDistance", Range(-0.1,0.1)) = 0
	}
	SubShader
	{
	    Tags 
	    {
	      "Queue" = "Transparent"
	      "RenderType" = "Transparent"
	      "PreviewType" = "Planet"
	      "CanUseSpriteAtlas" = "True"
	    }
	    
		// No culling or depth
		Cull Off
		ZWrite Off
		Lighting Off
		ZTest Always
		Blend One OneMinusSrcAlpha

		Pass
		{
            Name "Glitch"
	    
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			float rand3(float3 co)
            {
                return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
            }
            
			float rand2(float2 co)
            {
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }
            
			float rand(float co)
            {
                return frac(sin(co * 12.9898) * 43758.5453);
            }

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color: COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float4 worldPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.color = v.color;
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			
			float _Displacement;
			int _Tears;
			float _TearsDistance;

			fixed4 frag (v2f i) : SV_Target
			{
                float2 uv = i.uv;
                float _ix = floor(i.uv.y * _Tears);
                uv.x += rand(_ix) * _TearsDistance;
                
				fixed4 col = tex2D(_MainTex, uv);
				
				uv += float2(_Displacement, 0);
				fixed4 ocol = tex2D(_MainTex, uv);
				col.a = max(ocol.a, col.a);
				col.g = ocol.g;
				
				uv += float2(_Displacement, 0);
				ocol = tex2D(_MainTex, uv);
				col.a = max(ocol.a, col.a);
				col.b = ocol.b;
				
				col.rgb *= col.a;
				col *= i.color;
				return col;
			}
			ENDCG
		}
	}
}
