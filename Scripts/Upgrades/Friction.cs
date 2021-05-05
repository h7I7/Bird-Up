//\===========================================================================================
//\ File: Friction.cs
//\ Author: Morgan James
//\ Brief: Changes the players friction in accordance with the shop and unlocks.
//\===========================================================================================

using System.Collections;
using UnityEngine;

public class Friction : MonoBehaviour
{
	private int m_CurrentFrictionIndex;

	[Header("Friction'")]
	[SerializeField]
	public float[] m_Friction;

	[Header("Player")]
	[SerializeField]
	private GameObject m_Player;

	IEnumerator Start()
	{
		//Wait for the save manager instance to load
		while (SaveManager.Instance == null)
		{
			yield return null;
		}

		//Set the Friction.
		UpdateFriction(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedFriction));

		//Set the new index number to check against.
		m_CurrentFrictionIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedFriction);
	}

	void Update()
	{
		//Checks if a new friction is selected.
		if (m_CurrentFrictionIndex != SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedFriction))
		{
			//Sets the friction to the newly selected one.
			UpdateFriction(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedFriction));

			//Set the new index number to check against.
			m_CurrentFrictionIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedFriction);
		}
	}

	public void UpdateFriction(int a_iFrictionIndex)
	{								 
		if (m_Friction.Length > 0 && a_iFrictionIndex < m_Friction.Length)
		{
			//Changes friction.
			m_Player.GetComponent<PlayerController>().floorFriction = m_Friction[a_iFrictionIndex];
		}
	}
}
