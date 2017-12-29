Shader "Gene_Shades/HopR_Screen_Effect"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_IsBnW("IsBlackandWhite", Int) = 1
	}

	SubShader
	{
		Tags { "LightMode" = "ForwardBase" }
		Cull Off
		LOD 100
		 
		Pass
		{
			CGPROGRAM
	
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			
		uniform float4 _LightColor0;
		uniform int _IsBnW;

			struct appdata
			{
				half4 vertex : POSITION;
				half4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				half4 pos : SV_POSITION;
				half4 tex : TEXCOORD0;
			};

			uniform sampler2D _MainTex;

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.tex = v.texcoord;
				return o;
			}
			
	

			fixed4 frag (v2f i) : SV_Target 
			{ 
				fixed4 tex = tex2D(_MainTex, i.tex.xy);
				fixed3 color = fixed3(0, 0, 0);

				if (_IsBnW == 0)
					color = tex.rgb;
				else
					color = dot(tex.rgb, float3(0.3, 0.59, 0.11));//fixed4(0,0,0,0);

				return fixed4(color, 1.0);
			}
			ENDCG
		}
	}
}
