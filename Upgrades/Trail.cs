//\===========================================================================================
//\ File: Trail.cs
//\ Author: Morgan James
//\ Brief: Changes the players trail in accordance with the shop and unlocks.
//\===========================================================================================

using System.Collections;
using UnityEngine;

public class Trail : MonoBehaviour
{
	private int m_CurrentTrailIndex;

	[Header("Trail'")]
	[SerializeField]
	private Color[] m_Trail;

	[Header("Trail Renderer")]
	[SerializeField]
	private TrailRenderer m_TrailRenderer;

	IEnumerator Start()
	{
		//Wait for the save manager instance to load
		while (SaveManager.Instance == null)
		{
			yield return null;
		}

		//Set the Trail.
		UpdateTrail(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedTrail));

		//Set the new index number to check against.
		m_CurrentTrailIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedTrail);
	}

	void Update()
	{
		//Checks if a new trail is selected.
		if (m_CurrentTrailIndex != SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedTrail))
		{
			//Sets the trail to the newly selected one.
			UpdateTrail(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedTrail));

			//Set the new index number to check against.
			m_CurrentTrailIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedTrail);
		}
	}

	public void UpdateTrail(int a_iTrailIndex)
	{
		if (m_Trail.Length > 0 && a_iTrailIndex < m_Trail.Length)
		{
			//Changes trail.
			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(m_Trail[a_iTrailIndex], 0.0f), new GradientColorKey(m_Trail[a_iTrailIndex], 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 0.5f) }
				);
			m_TrailRenderer.colorGradient = gradient;
		}
	}
}
