//\===========================================================================================
//\ File: GameUIController.cs
//\ Author: Morgan James
//\ Brief: Enables and disables certain UI objects depending on the game state (functions are called from buttons).
//\===========================================================================================

using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
	[SerializeField]
	private GameObject playerLock;
	[SerializeField]
	private GameObject playerTimer;

	[Header("Pause")]
	[SerializeField]
	private GameObject[] m_ObjectsToHideOnPause;
	[SerializeField]
	private GameObject[] m_ObjectsToUnHideOnPause;

	[Header("Shop")]
	[SerializeField]
	private GameObject[] m_ObjectsToHideOnShop;
	[SerializeField]
	private GameObject[] m_ObjectsToUnHideOnShop;

	[Header("Menu")]
	[SerializeField]
	private GameObject[] m_ObjectsToHideOnMenu;
	[SerializeField]
	private GameObject[] m_ObjectsToUnHideOnMenu;

	//When the game is paused.
	public void Pause()
	{
		
		foreach(GameObject a_gameObject in m_ObjectsToHideOnPause)
		{
			a_gameObject.SetActive(false);
		}

		foreach (GameObject a_gameObject in m_ObjectsToUnHideOnPause)
		{
			a_gameObject.SetActive(true);
		}
    }

	//On resume.
	public void Resume()
	{
		foreach (GameObject a_gameObject in m_ObjectsToUnHideOnPause)
		{
			a_gameObject.SetActive(false);
		}

		foreach (GameObject a_gameObject in m_ObjectsToHideOnPause)
		{
			a_gameObject.SetActive(true);
		}

		//Reset timer and lock fade.
		playerTimer.GetComponent<Image>().CrossFadeAlpha(0f, 0, true);
		playerLock.GetComponent<Image>().CrossFadeAlpha(0f, 0, true);
    }

	//Whilst in the shop.
	public void Shop()
	{
		foreach (GameObject a_gameObject in m_ObjectsToHideOnShop)
		{
			a_gameObject.SetActive(false);
		}

		foreach (GameObject a_gameObject in m_ObjectsToUnHideOnShop)
		{
			a_gameObject.SetActive(true);
		}
	}

	//When exiting the shop.
	public void CloseShop()
	{
		foreach (GameObject a_gameObject in m_ObjectsToUnHideOnShop)
		{
			a_gameObject.SetActive(false);
		}

		foreach (GameObject a_gameObject in m_ObjectsToHideOnShop)
		{
			a_gameObject.SetActive(true);
		}

		//Reset timer and lock fade.
		playerTimer.GetComponent<Image>().CrossFadeAlpha(0f, 0, true);
		playerLock.GetComponent<Image>().CrossFadeAlpha(0f, 0, true);
	}

	//On the return to the main menu.
	public void MainMenu()
	{
		foreach (GameObject a_gameObject in m_ObjectsToHideOnMenu)
		{
			a_gameObject.SetActive(false);
		}

		foreach (GameObject a_gameObject in m_ObjectsToUnHideOnMenu)
		{
			a_gameObject.SetActive(true);
		}

		//Reset timer and lock fade.
		playerTimer.GetComponent<Image>().CrossFadeAlpha(0f, 0, true);
		playerLock.GetComponent<Image>().CrossFadeAlpha(0f, 0, true);
	}
}
