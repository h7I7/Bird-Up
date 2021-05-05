//\===========================================================================================
//\ File: Face.cs
//\ Author: Morgan James
//\ Brief: Changes the players face in accordance with the shop and unlocks.
//\===========================================================================================

using System.Collections;
using UnityEngine;

public class Face : MonoBehaviour
{
	private int currentFaceIndex;
	
	[Header("Player Faces")]
	[SerializeField]
	private Sprite[] m_PlayerFaces;

	[Header("Current Face")]
	[SerializeField]
	private SpriteRenderer m_Face;

	IEnumerator Start()
	{
		//Wait for the save manager instance to load.
		while (SaveManager.Instance == null)
		{
			yield return null;
		}

		//Set the players face.
		UpdateFace(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerTexture));

		//Set the new index number to check against.
		currentFaceIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerTexture);
	}

	void Update()
	{
		//Checks if a new face is selected.
		if (currentFaceIndex != SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerTexture))
		{
			//Sets the face to the newly selected one.
			UpdateFace(SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerTexture));

			//Set the new index number to check against.
			currentFaceIndex = SaveManager.Instance.CheckBitPos(SaveManager.Instance.state.selectedPlayerTexture);
		}
	}

	public void UpdateFace(int a_iFaceIndex)
	{
		if (m_PlayerFaces.Length > 0 && a_iFaceIndex < m_PlayerFaces.Length)
		{
			//Changes the players face texture.
			m_Face.sprite = m_PlayerFaces[a_iFaceIndex];
		}
	}
}

