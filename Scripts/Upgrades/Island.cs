//\===========================================================================================
//\ File: Island.cs
//\ Author: Morgan James
//\ Brief: Changes the island texture in accordance with the shop and unlocks.
//\===========================================================================================

using System.Collections;
using UnityEngine;

public class Island : MonoBehaviour
{
	private int m_currentIslandIndex;

	[Header("Island'")]
	[SerializeField]
	private Texture[] m_Island;

	[Header("Current Island")]
	[SerializeField]
	private Material m_CurrentIsland;

	IEnumerator Start()
	{
		//Wait for the save manager instance to load.
		while (SaveManager.Instance == null)
		{
			yield return null;
		}

		//Set the island.
		UpdateIsland(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedIslandTexture));
		
		//Set the new index number to check against.
		m_currentIslandIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedIslandTexture);
	}

	void Update()
	{
		//Checks if a new island texture is selected.
		if (m_currentIslandIndex != SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedIslandTexture))
		{
			//Sets the island texture to the newly selected one.
			UpdateIsland(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedIslandTexture));

			//Set the new index number to check against.
			m_currentIslandIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedIslandTexture);
		}
	}

	public void UpdateIsland(int a_iIslandIndex)
	{
		if (m_Island.Length > 0 && a_iIslandIndex < m_Island.Length)
		{
			//Changes island texture.
			m_CurrentIsland.SetTexture("_MainTex", m_Island[a_iIslandIndex]);
		}
	}
}
