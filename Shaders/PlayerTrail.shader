//\===========================================================================================
//\ File: PlayerTrail.shader
//\ Author: Morgan James
//\ Brief: Stops the trail overlapping itself in weird ways.
//\===========================================================================================

Shader "Custom/PlayerTrail"
{
	Category
	{
		Tags{}
		Blend SrcAlpha One
		ZWrite On
		ZTest Less

		SubShader
		{
			Pass
			{

			}
		}
	}
}