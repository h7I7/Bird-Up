//\===========================================================================================
//\ File: Highscore.cs
//\ Author: Morgan James
//\ Brief: Sets text objects to be equal to the Highscore.
//\===========================================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Highscore : MonoBehaviour {

	IEnumerator Start()
	{
		//If there is no save manager exit.
		while (SaveManager.Instance == null)
		{
			yield return null;
		}

		//Set the highscore from the save file.
		GetComponent<Text>().text = "HIGHSCORE: " + SaveManager.Instance.state.HighScore.ToString();
	}

	public void UpdateScore()
	{
		//Set the highscore from the save file.
		GetComponent<Text>().text = "HIGHSCORE: " + SaveManager.Instance.state.HighScore.ToString();
	}
}
