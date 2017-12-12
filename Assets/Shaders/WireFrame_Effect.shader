// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Gene_Shades/WireFrame_Effect"
{
	Properties
	{
		_Color("File Color", Color) = (0,0,0,1)
		_Color1("Wire Color", Color) = (1,1,1,1)
		_Boundry("Wire Boundry", Range(0, 0.5)) = 0.455
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" }
		ZWrite ON
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"


			uniform float4 _Color;
			uniform float4 _Color1;
			uniform float _Boundry;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};
			
			v2f vert (appdata v)
			{ 
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 c;
			float coord = step(_Boundry, abs(i.uv.x - 0.5)) || step(_Boundry, abs(i.uv.y - 0.5)); //1 if i.uv.x is inside boundry edges

			c = lerp(_Color, _Color1, coord*_Color1.a);
			return c;

			}
			ENDCG
		}
	}
}
