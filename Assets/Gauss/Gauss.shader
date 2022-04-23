Shader "Hidden/Blurry"
{

	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Sigma("Sigma", Range(1.0, 5.0)) = 1.5
		_BlurRadius("BlurRadius", Range(1.0, 10.0)) = 3.0
	}

		CGINCLUDE
#include "UnityCG.cginc"
			struct v2f_blur
		{
			float4 pos : SV_POSITION; //顶点位置
			float2 uv  : TEXCOORD0;	  //纹理坐标
		};


		//用到的变量
		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		//模糊半径
		float _BlurRadius;

		// σ
		float _Sigma;
		//vertex shader
		v2f_blur vert_blur(appdata_img v)
		{
			v2f_blur o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord.xy;
			return o;
		}

		//fragment shader
		fixed4 frag_blur(v2f_blur i) : SV_Target
		{
			fixed4 color = fixed4(0,0,0,0);

		// 采样的9个像素点的偏移量
		float2 offsets[9] =
		{
			float2(-1, 1), float2(0, 1), float2(1, 1),
			float2(-1, 0), float2(0, 0), float2(1, 0),
			float2(-1, -1), float2(0, -1), float2(1, -1),
		};

		float pi = 3.1415;	// 圆周率
		float e = 2.7182;	// 数学常数

		float sum = 0;
		float weight[9];
		// 卷积
		for (int j = 0; j < 9; j++)
		{
			float l = length(_BlurRadius * _MainTex_TexelSize * offsets[j]);	// 求距离
			float g = (1.0 / (2.0 * pi * pow(_Sigma, 2.0))) * pow(e, (-(l * l) / (2.0 * pow(_Sigma, 2.0))));	// 高斯函数值
			weight[j] = g;
			sum += g;
		}

		for (int j = 0; j < 9; j++)
			weight[j] /= sum;

		for (int j = 0; j < 9; j++)
		{
			color += tex2D(_MainTex, i.uv + _BlurRadius * _MainTex_TexelSize * offsets[j]) * weight[j];
		}
		return color;
		}

			ENDCG

			SubShader
		{
			Pass
			{
				ZTest Always
				Cull Off
				ZWrite Off
				Fog{ Mode Off }

				CGPROGRAM
				#pragma vertex vert_blur
				#pragma fragment frag_blur
				ENDCG
			}
		}

}