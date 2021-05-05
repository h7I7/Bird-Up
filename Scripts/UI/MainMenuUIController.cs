//\===========================================================================================
//\ File: MainMenuUIController.cs
//\ Author: Morgan James
//\ Brief: Enables and disables certain UI objects depending on the game state (functions are called from buttons).
//\===========================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
	[Header("Play")]
	[SerializeField]
	private GameObject[] m_ObjectsToHideOnPlay;
	[SerializeField]
	private GameObject[] m_ObjectsToUnHideOnPlay;

	[Header("Share")]
	[SerializeField]
	private GameObject[] m_ObjectsToHideOnShare;
	[SerializeField]
	private GameObject[] m_ObjectsToUnHideOnShare;

	//A quit function that we used to use.
	public void QuitGame()
	{
		#if !UNITY_EDITOR
			Application.Quit();
		#endif
		
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}

	//When the game starts.
	public void PlayGame()
	{
		foreach (GameObject a_gameObject in m_ObjectsToHideOnPlay)
		{
			a_gameObject.SetActive(false);
		}

		foreach (GameObject a_gameObject in m_ObjectsToUnHideOnPlay)
		{
			a_gameObject.SetActive(true);
		}
	}
	
	//When the user wants to share a screen shot.
	public void Share()
	{
		foreach (GameObject a_gameObject in m_ObjectsToHideOnShare)
		{
			a_gameObject.SetActive(false);
		}

		foreach (GameObject a_gameObject in m_ObjectsToUnHideOnShare)
		{
			a_gameObject.SetActive(true);
		}
	}

	//When the screenshot has been taken.
	public void StopShare()
	{
		foreach (GameObject a_gameObject in m_ObjectsToUnHideOnShare)
		{
			a_gameObject.SetActive(false);
		}

		foreach (GameObject a_gameObject in m_ObjectsToHideOnShare)
		{
			a_gameObject.SetActive(true);
		}
	}

}
