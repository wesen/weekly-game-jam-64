Shader "Custom/BloomShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	
    CGINCLUDE
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

        sampler2D _MainTex, _SourceTex;
        float4 _MainTex_TexelSize;
        float4 _Filter;
        
        half3 sample(float2 uv) 
        {
            return tex2D(_MainTex, uv).rgb;
        }
        
        half3 prefilter(half3 c)
        {
            float brightness = max(c.r, max(c.g, c.b));
            float soft = brightness - _Filter.y;
            soft = clamp(soft, 0, _Filter.z);
            soft = soft * soft * _Filter.w;
            
            float contribution = max(soft, brightness - _Filter.x);
            contribution /= max(brightness, 0.000001);
            return c * contribution;
        }
        
        half3 sampleBox(float2 uv, float delta) 
        {
            float4 o = _MainTex_TexelSize.xyxy * float2(-delta, delta).xxyy;
            half3 s = sample(uv + o.xy) + sample(uv + o.zy)
              + sample(uv + o.xw) + sample(uv + o.zw);
            return s / 4;
        }
        
        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }
			
    ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		
		Pass
		{
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
			
			fixed4 frag (v2f i) : SV_Target
			{
                fixed4 col = fixed4(prefilter(sampleBox(i.uv, 1)), 1);
				return col;
			}
			ENDCG
		}
		
		Pass
		{
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
			
			fixed4 frag (v2f i) : SV_Target
			{
                fixed4 col = fixed4(sampleBox(i.uv, 1), 1);
				return col;
			}
			ENDCG
		}
		
		Pass
		{
            Blend One One
		
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
			fixed4 frag (v2f i) : SV_Target
			{
                fixed4 col = fixed4(sampleBox(i.uv, .5), 1);
				return col;
			}
			ENDCG
		}
		
		Pass
		{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
			fixed4 frag (v2f i) : SV_Target
			{
			    fixed4 c = tex2D(_SourceTex, i.uv);
			    c.rgb += sampleBox(i.uv, 0.5);
				return c;
			}
            ENDCG
        }
        
		Pass // DebugPass
		{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
			fixed4 frag (v2f i) : SV_Target
			{
			    fixed4 c = fixed4(sampleBox(i.uv, 0.5), 1);
				return c;
			}
            ENDCG
        }
	}
}
