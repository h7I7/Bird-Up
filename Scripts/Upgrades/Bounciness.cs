//\===========================================================================================
//\ File: Bounciness.cs
//\ Author: Morgan James
//\ Brief: Changes the bounciness of the player in accordance with the shop and unlocks.
//\===========================================================================================

using System.Collections;
using UnityEngine;

public class Bounciness : MonoBehaviour
{
	private int m_CurrentBouncinessIndex;

	[Header("Bounciness'")]
	[SerializeField]
	private float[] m_Bounciness;

	[Header("Physics 2D Material")]
	[SerializeField]
	private PhysicsMaterial2D m_PlayerPhysiscsMaterial;

	IEnumerator Start()
	{
		//Wait for the save manager instance to load.
		while (SaveManager.Instance == null)
		{
			yield return null;
		}

		//Set the Bounciness.
		UpdateBounciness(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBouncyness));
		
		//Set the new index number to check against.
		m_CurrentBouncinessIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBouncyness);
	}

	void Update()
	{
		//Checks if a new bounciness is selected.
		if (m_CurrentBouncinessIndex != SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBouncyness))
		{
			//Sets the bounciness to the newly selected one.
			UpdateBounciness(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBouncyness));
			
			//Set the new index number to check against.
			m_CurrentBouncinessIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedBouncyness);
		}
	}

	public void UpdateBounciness(int a_iBouncinessIndex)
	{
		if (m_Bounciness.Length > 0 && a_iBouncinessIndex < m_Bounciness.Length)
		{
			//Changes Bounciness.
			m_PlayerPhysiscsMaterial.bounciness = m_Bounciness[a_iBouncinessIndex];
		}
	}
}
