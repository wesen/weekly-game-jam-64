Shader "Sprites/SpriteGhostShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_HoloTexture ("Hologram Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags {
	      "Queue" = "Transparent"
	      "RenderType" = "Transparent"
	      "CanUseSpriteAtlas" = "True"
		}
		LOD 100
		
		Cull Off
		ZWrite Off
		Lighting Off
		ZTest Always
//        ZWrite Off

		
//		UsePass "Sprites/GlitchSpriteShader/Glitch"
		
		Pass 
		{
		    Name "White"
		    
		    CGPROGRAM
		    #pragma vertex vert
		    #pragma fragment frag
		    
			#include "UnityCG.cginc"
		    
		    struct appdata { 
		        float4 vertex: POSITION;
		    };
		    struct v2f { 
		        float4 vertex : SV_POSITION;
		    };
		    v2f vert(appdata v) {
		      v2f o;
		      o.vertex = UnityObjectToClipPos(v.vertex);
		      return o;
		    }
		    fixed4 frag(v2f i) : SV_Target {
		      return fixed4(1, 0, 0, 1);
		    }
		    ENDCG
		}
		
		Pass
		{
	        Name "Hologram"
	        
	        BlendOp Add, Min
            Blend SrcAlpha SrcAlpha
	        
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color: COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _HoloTexture;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 maskCol = tex2D(_HoloTexture, i.uv);
				// apply fog
//				UNITY_APPLY_FOG(i.fogCoord, col);
//                col *= i.color;
//				col.rgb *= col.a;
//				col.rgb *= maskCol;

//				maskCol.a = 0.5;
//				maskCol = float4(1, 1, 1, 1);
                maskCol.rgb *= maskCol.a;
				return maskCol;
			}
			ENDCG
		}
	}
}
