//\===========================================================================================
//\ File: LineRenderFade.cs
//\ Author: Morgan James
//\ Brief: Fades the color of the arc that shows you where you are going.
//\===========================================================================================

using UnityEngine;

public class LineRenderFade : MonoBehaviour
{
	[SerializeField]
	private LineRenderer m_lineRenderer;//The line render this script affects.

	private void Start()
	{
		//A simple 2 color gradient with a fixed alpha of 1.0f.
		Gradient gradient = new Gradient();

		//Set the keys of the gradient.
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
			);
		//Apply the gradient.
		m_lineRenderer.colorGradient = gradient;
	}
}
