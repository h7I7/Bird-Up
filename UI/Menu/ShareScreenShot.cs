//\===========================================================================================
//\ File: ShareScreenShot.cs
//\ Author: Morgan James
//\ Brief: Sets the UI to screenshot mode, takes a screenshot then opens it in the native sharing platform.
//\===========================================================================================

using System.Collections;
using UnityEngine;
using EasyMobile;//The asset name space that i used to run the native code.

public class ShareScreenShot : MonoBehaviour
{
	[SerializeField]	
	private string m_Message;//The message that will be attached the screen grab if applicable (email/twitter).
	[SerializeField]
	private string m_Title;//The title of the shared screen capture(email title).
	
	public void Share()
	{
		//Sets the UI to the screen shot setup.
		GetComponent<MainMenuUIController>().Share();

		//Start the screen capture coroutine.
		StartCoroutine(CROneStepSharing());
	}

	IEnumerator CROneStepSharing()
	{
		//Waits until the frame has loaded
		yield return new WaitForEndOfFrame();

		//Takes and saves a screen shot then shares it using the native sharing platform of the device.
		Sharing.ShareScreenshot(m_Title, m_Message);

		//Sets the UI to the menu setup.
		GetComponent<MainMenuUIController>().StopShare();
	}
}
