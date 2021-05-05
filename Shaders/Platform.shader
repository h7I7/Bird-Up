//\===========================================================================================
//\ File: Platform.shader
//\ Author: Morgan James
//\ Brief: Outlines the object and uses a screen space texture as its material.
//\===========================================================================================

Shader "Custom/Platform"
{
	Properties
	{
		[Header(Outline)]
		_OutlineVal("Outline value", Range(1., 2.)) = 1.
		_OutlineCol("Outline color", color) = (1., 1., 1., 1.)

		[Header(Shadow)]
		_ShadowDir("Drop shadow direction and amount", vector) = (-1, -1, 0, 0)

		[Header(Texture)]
		_MainTex("Texture", 2D) = "white" {}
		_Color("Tint Color", color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }

		Pass
		{
			Name "OUTLINE"


			CGPROGRAM//Allows talk between two languages: shader lab and nvidia C for graphics.

			//\===========================================================================================
			//\ Function Defines - defines the name for the vertex and fragment functions
			//\===========================================================================================

			#pragma vertex vert//Define for the building function.

			#pragma fragment frag//Define for coloring function.

			//\===========================================================================================
			//\ Includes
			//\===========================================================================================

			#include "UnityCG.cginc"//Built in shader functions.

			//\===========================================================================================
			//\ Structures - Can get data like - vertices's, normal, color, uv.
			//\===========================================================================================

			struct appdata//How the vertex function receives info.
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//\===========================================================================================
			//\ Imports - Re-import property from shader lab to nvidia cg
			//\===========================================================================================

			float _OutlineVal;
			float4 _OutlineCol;

			float4 _ShadowDir;

			//\===========================================================================================
			//\ Vertex Function - Builds the object
			//\===========================================================================================

			v2f vert(appdata IN)
			{
				IN.vertex.xyz *= _OutlineVal;
				v2f OUT;

				OUT.pos = UnityObjectToClipPos(IN.vertex) + _ShadowDir;
				OUT.uv = IN.uv;

				return OUT;
			}

			//\===========================================================================================
			//\ Fragment Function - Color it in
			//\===========================================================================================

			fixed4 frag(v2f IN) : SV_Target
			{
				return _OutlineCol;
			}

		ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 vert(appdata_base v) : SV_POSITION
			{
				return UnityObjectToClipPos(v.vertex);
			}

			sampler2D _MainTex;
			float4 _Color;

			fixed4 frag(float4 i : VPOS) : SV_Target
			{
				// Screen space texture
				return tex2D(_MainTex, i.xy / _ScreenParams.xy) * _Color;
			}

		ENDCG
		}
	}
}