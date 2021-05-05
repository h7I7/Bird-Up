//\===========================================================================================
//\ File: Background.cs
//\ Author: Morgan James
//\ Brief: Changes the background in accordance with the shop and unlocks.
//\===========================================================================================

using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
	private int m_CurrentBackgroundIndex;

	[Header("Background'")]
	[SerializeField]
	private Sprite[] m_Background;

	[Header("Current Background")]
	[SerializeField]
	private GameObject m_CurrentBackground;

	IEnumerator Start()
	{
		//Wait for the save manager instance to load.
		while (SaveManager.Instance == null)
		{
			yield return null;
		}

		//Set the Background.
		UpdateBackground(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBackgroundTexture));

		//Set the new index number to check against.
		m_CurrentBackgroundIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBackgroundTexture);
	}

	void Update()
	{
		//Checks if a new background is selected.
		if (m_CurrentBackgroundIndex != SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBackgroundTexture))
		{
			//Sets the background to the newly selected one.
			UpdateBackground(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBackgroundTexture));

			//Set the new index number to check against.
			m_CurrentBackgroundIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBackgroundTexture);
		}
	}

	public void UpdateBackground(int a_iBackgroundIndex)
	{
		if (m_Background.Length > 0 && a_iBackgroundIndex < m_Background.Length)
		{
			//Changes Background.
			m_CurrentBackground.GetComponent<SpriteRenderer>().sprite = m_Background[a_iBackgroundIndex];
		}
	}
}
