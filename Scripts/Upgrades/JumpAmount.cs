//\===========================================================================================
//\ File: JumpAmount.cs
//\ Author: Morgan James
//\ Brief: Changes the background in accordance with the shop and unlocks.
//\===========================================================================================

using System.Collections;
using UnityEngine;

public class JumpAmount : MonoBehaviour
{
	private int m_CurrentJumpAmountIndex;

	[Header("JumpAmount'")]
	[SerializeField]
	private int[] m_JumpAmount;

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

		//Set the jump Amount.
		UpdateJumpAmount(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedAmountOfJumps));

		//Set the new index number to check against.
		m_CurrentJumpAmountIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedAmountOfJumps);
	}

	void Update()
	{
		//Checks if a new jump amount is selected.
		if (m_CurrentJumpAmountIndex != SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedAmountOfJumps))
		{
			//Sets the jump amount to the newly selected one.
			UpdateJumpAmount(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedAmountOfJumps));

			//Set the new index number to check against.
			m_CurrentJumpAmountIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedAmountOfJumps);
		}
	}

	public void UpdateJumpAmount(int a_iJumpAmountIndex)
	{
		if (m_JumpAmount.Length > 0 && a_iJumpAmountIndex < m_JumpAmount.Length)
		{
			//Changes JumpAmount.
			m_Player.GetComponent<PlayerController>().m_gravityJumps = m_JumpAmount[a_iJumpAmountIndex];
		}
	}
}
