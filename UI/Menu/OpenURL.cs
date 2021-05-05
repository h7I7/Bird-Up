//\===========================================================================================
//\ File: OpenURL.cs
//\ Author: Morgan James
//\ Brief: A function to open URL's in the native browser.
//\===========================================================================================

using UnityEngine;

public class OpenURL : MonoBehaviour
{
	[SerializeField]
	private string m_URL;//The URL that will open on function execution

	//A function to open the desired URL (activate with button event for best results).
	public void OpenWebpage()
	{
		Application.OpenURL(m_URL);
	}
	
}
