//\===========================================================================================
//\ File: PlayerColour.cs
//\ Author: Morgan James
//\ Brief: Changes the players color in accordance with the shop and unlocks.
//\===========================================================================================

using System.Collections;
using UnityEngine;

public class PlayerColour : MonoBehaviour
{
	private int m_CurrentPlayerColourIndex;

	[Header("PlayerColour'")]
	[SerializeField]
	private Color[] m_PlayerColour;

	[Header("Player")]
	[SerializeField]
	private SpriteRenderer m_PlayerSpriteRenderer;

	IEnumerator Start()
	{
		//Wait for the save manager instance to load
		while (SaveManager.Instance == null)
		{
			yield return null;
		}

		//Set the PlayerColour.
		UpdatePlayerColour(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerColour));

		//Set the new index number to check against.
		m_CurrentPlayerColourIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerColour);
	}

	void Update()
	{
		//Checks if a new player color is selected.
		if (m_CurrentPlayerColourIndex != SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerColour))
		{
			//Sets the player color to the newly selected one.
			UpdatePlayerColour(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerColour));

			//Set the new index number to check against.
			m_CurrentPlayerColourIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerColour);
		}
	}

	public void UpdatePlayerColour(int a_iPlayerColourIndex)
	{
		if (m_PlayerColour.Length > 0 && a_iPlayerColourIndex < m_PlayerColour.Length)
		{
			//Changes PlayerColour.
			m_PlayerSpriteRenderer.color = m_PlayerColour[a_iPlayerColourIndex];
		}
	}
}
