//\===========================================================================================
//\ File: Gravity.cs
//\ Author: Morgan James
//\ Brief: Changes the gravity in accordance with the shop and unlocks.
//\===========================================================================================

using System.Collections;
using UnityEngine;

public class Gravity : MonoBehaviour
{
	//Singleton pattern.
	private static Gravity m_instance;
	public static Gravity Instance
	{
		get { return m_instance; }
	}

	private int m_CurrentGravityIndex;

	[Header("Gravity Modifiers")]
	[SerializeField]
	private Vector2[] m_Gravity;

	private void Awake()
	{
		//Setting the instance
		if (m_instance == null)
			m_instance = this;
		else
			Destroy(gameObject);
	}

	private IEnumerator Start()
	{
		//Wait for the save manager instance to load.
		while (SaveManager.Instance == null)
		{
			yield return null;
		}

		//Set the gravity.
		UpdateGravity(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedGravityModifyer));

		//Set the new index number to check against.
		m_CurrentGravityIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedGravityModifyer);
	}

	public void UpdateGravity(int a_iGravityIndex = -1)
	{
		if (a_iGravityIndex == -1)
			a_iGravityIndex = m_CurrentGravityIndex;

		if (m_Gravity.Length > 0 && a_iGravityIndex < m_Gravity.Length)
		{
			//Changes gravity.
			Physics2D.gravity = m_Gravity[a_iGravityIndex];
		}

		m_CurrentGravityIndex = a_iGravityIndex;
	}
}
