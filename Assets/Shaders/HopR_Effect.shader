// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Gene_Shades/HopR_Effect"
{
	Properties
	{
		_Color("Lit Color", Color) = (1,1,1,1)
		_UnlitColor("Unlit Color", Color) = (0.5,0.5,0.5,0.5)
		_DiffuseThreshold("Lighting Threshold", Range(-1.1,1)) = 0.1
		_Diffusion("Diffusion", Range(0,0.99)) = 0.0
		_SpecColor("Specular Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Range(0.5, 1)) = 1
		_SpecDiffusion("Specular Diffusion", Range(0, 0.99)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		 
		Pass
		{
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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				fixed3 normalDir : TEXCOORD0;
				fixed4 lightDir : TEXCOORD1;
				fixed3 viewDir : TEXCOORD2;
				fixed3 lightFinal : TEXCOORD3;
				float4 fragPos: TEXCOORD4;
				float2 tex: TEXCOORD5;
			};

			uniform fixed4 _Color;
			uniform float4 _LightColor0;

			uniform float3 _Points[7];
			uniform fixed4 _UnlitColor;
			uniform fixed _DiffuseThreshold;
			uniform fixed _Diffusion;
			uniform fixed4 _SpecColor;
			uniform fixed _Shininess;
			uniform half _SpecDiffusion;



			uniform float3 palette[8];
			uniform int paletteSize;
			const float lightnessSteps = 15.0;

			uniform float indexMatrix16x16[64];
			const int num = 9;


			float lightnessStep(float l)
			{
				/* Quantize the lightness to one of `lightnessSteps` values */
				return floor((0.5 + l * lightnessSteps)) / lightnessSteps;
			}

			float indexValue(float3 pos)
			{
				float x = (fmod(pos.x, 4.0));
				float y = (fmod(pos.y, 4.0));

				float grabby = 0.0;

				for (int i = 0; i < 64; i++)
				{
					if (i >= int(x + (y * 4.0)))
					{
						grabby = indexMatrix16x16[i];
						break;
					}
				}

				return grabby / 64.0;
			}

			float ditherMap(float color, float3 pos)
			{
				float closestColor = (color <= 0.5) ? 0.0 : 1.0;
				float secondClosestColor = 1.0 - closestColor;
				float d = indexValue(pos);
				float distance = abs(closestColor - color);
				return (distance < d) ? closestColor : secondClosestColor;
			}

			float3 hslToRgb(float3 c)
			{
				float3 rgb = clamp(abs(fmod(c.x*6.0 + float3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
				return c.z + c.y * (rgb - 0.5)*(1.0 - abs(2.0*c.z - 1.0));
			}

			float3 rgbToHsl(float3 c)
			{
				float h = 0.0;
				float s = 0.0;
				float l = 0.0;

				float r = c.r;
				float g = c.g;
				float b = c.b;

				float cMin = min(r, min(g, b));
				float cMax = max(r, max(g, b));

				l = (cMax + cMin) / 2.0;
				if (cMax > cMin) {
					float cDelta = cMax - cMin;

					//s = l < .05 ? cDelta / ( cMax + cMin ) : cDelta / ( 2.0 - ( cMax + cMin ) ); Original
					s = l < .0 ? cDelta / (cMax + cMin) : cDelta /
						(2.0 - (cMax + cMin));

					if (r == cMax) {
						h = (g - b) / cDelta;
					}
					else if (g == cMax) {
						h = 2.0 + (b - r) / cDelta;
					}
					else {
						h = 4.0 + (r - g) / cDelta;
					}

					if (h < 0.0) {
						h += 6.0;
					}
					h = h / 6.0;
				}
				return float3(h, s, l);
			}

			float hueDistance(float h1, float h2)
			{
				float diff = abs((h1 - h2));
				return min(abs((1.0 - diff)), diff);
			}

			float3 dither(float3 color, float3 pos)
			{
				float3 hsl = rgbToHsl(color);

				float3 ret[2];
				float3 closest = float3(0, 0, 0);
				float3 secondClosest = float3(0, 0, 0);
				float3 temp = float3(0, 0, 0);

				for (int i = 0; i < 8; ++i)
				{
					temp = palette[i];
					float tempDistance = hueDistance(temp.x, hsl.x);

					if (tempDistance < hueDistance(closest.x, hsl.x))
					{
						secondClosest = closest;
						closest = temp;
					}
					else
					{
						if (tempDistance < hueDistance(secondClosest.x, hsl.x))
						{
							secondClosest = temp;
						}
					}
				}

				ret[0] = closest;
				ret[1] = secondClosest;

				float d = indexValue(pos);
				float hueDiff = hueDistance(hsl.x, ret[0].x) / hueDistance(ret[1].x, ret[0].x);

				float l1 = lightnessStep(max((hsl.z - 0.125), 0.0));
				float l2 = lightnessStep(min((hsl.z + 0.124), 1.0));
				float lightnessDiff = (hsl.z - l1) / (l2 - l1);

				float3 resultColor = (hueDiff < d) ? ret[0] : ret[1];
				resultColor.z = (lightnessDiff < d) ? l1 : l2;
				return hslToRgb(resultColor);
			}


			v2f vert (appdata v)
			{
				v2f o;
				o.tex = v.uv;

			
				//Normal Direction
				o.normalDir = normalize(mul(half4(v.vertex.xyz, 0.0), unity_WorldToObject).xyz);

				//Unity transform Position
				o.pos = UnityObjectToClipPos(v.vertex);

				//world Position
				half4 posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.fragPos = posWorld;
				//view Direction
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
				//light Direction
				half3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz;
				o.lightDir = fixed4(
					normalize(lerp(_WorldSpaceLightPos0.xyz, fragmentToLightSource, _WorldSpaceLightPos0.w)),
					lerp(1.0, 1.0 / length(fragmentToLightSource), _WorldSpaceLightPos0.w)
				);

				//dotProduct
				fixed nDotL = saturate(dot(o.normalDir, o.lightDir.xyz));

				//Diffuse Color
				fixed diffuseCutoff = saturate((max(_DiffuseThreshold, nDotL) - _DiffuseThreshold)
					* pow((2 - _Diffusion), 10));

				fixed specularCutoff = saturate((max(_Shininess, dot(reflect(-o.lightDir, o.normalDir),
					o.viewDir)) - _Shininess)* pow((2 - _SpecDiffusion), 10));

				fixed3 ambientLight = (1 - diffuseCutoff) * _UnlitColor.xyz;
				fixed3 diffuseReflection = (1 - specularCutoff) * _Color.xyz * diffuseCutoff;
				fixed3 specularReflection = _SpecColor.xyz * specularCutoff;

				o.lightFinal = saturate(ambientLight + diffuseReflection + specularReflection);

				return o;
			}
			
	

			fixed4 frag (v2f i) : SV_Target 
			{ 

				//Lighting
				 
			//float3 lightDither = dither(i.lightDir, float3(i.fragPos.xyz));

			float dither2x = ditherMap(i.lightFinal.x, float3(i.pos.xy, 1.0));
			float dither2y = ditherMap(i.lightFinal.y, float3(i.pos.xy, 1.0));
			float dither2z = ditherMap(i.lightFinal.z, float3(i.pos.xy, 1.0));

			float3 dither2 = float3(dither2x, dither2y, dither2z);

			return float4(dither2, 1.0);
			}
			ENDCG
		}
	}
}
