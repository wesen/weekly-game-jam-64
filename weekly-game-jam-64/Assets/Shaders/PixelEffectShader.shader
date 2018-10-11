Shader "Hidden/PixelEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Pixels ("Pixels", Vector) = (10, 10, 0, 0)
		
		_LCDTex ("LCD Texture", 2D) = "white" {}
		_LCDPixels ("LCD Pixels", Vector) = (3, 3, 0, 0)
		
		_LCDAmount ("LCD Amount", Range(0,1)) = 0.5
		
		_Br ("Brightness", Range(0,1)) = 0
		_Contrast ("Contrast", Range(0, 2)) = 0
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
			sampler2D _LCDTex;
			float4 _Pixels;
			float4 _LCDPixels;
			float _LCDAmount;
			float _Br;
			float _Contrast;

			fixed4 frag (v2f i) : SV_Target
			{
			    float2 uv_lcd = i.uv * _Pixels.xy / _LCDPixels.xy;
			    fixed4 lcd_col = tex2D(_LCDTex, uv_lcd);
			    
			    float2 uv = round(i.uv * _Pixels.xy + 0.5) / _Pixels.xy;
				fixed4 col = tex2D(_MainTex, uv);
				
//				col.rgb = lerp(col.rgb, col.rgb * lcd_col.rgb, _LCDAmount);
                col.rgb *= lcd_col.rgb;
                
                col.rgb += _Br;
                col.rgb = col.rgb - _Contrast * (col.rgb - 1.0) * col.rgb * (col.rgb - 0.5); 
				
				return col;
			}
			ENDCG
		}
	}
}

