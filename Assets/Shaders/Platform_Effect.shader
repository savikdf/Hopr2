// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Gene_Shades/Platform_Effect"
{
	Properties
	{
		_Color("Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
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

			struct appdata
			{
				half4 vertex : POSITION;
			};

			struct v2f
			{
				half4 pos : SV_POSITION;
			};

			uniform float4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target 
			{ 
				return _Color;
			}
			ENDCG
		}
	}
}
