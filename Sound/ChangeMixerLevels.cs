//\===========================================================================================
//\ File: ChangeMixerLevels.cs
//\ Author: Morgan James
//\ Brief: Allows for the changing of exposed audio mixer variables.
//\===========================================================================================

using UnityEngine;
using UnityEngine.Audio;

public class ChangeMixerLevels : MonoBehaviour
{
	[SerializeField]
	private AudioMixer m_Mixer;//The mixer that contains the variables we want to change.

	[SerializeField]
	private string m_parameterName;//The parameter we want to change.

	//Changes the sound level of a parameter based on it's input.
	public void SetMusicLevel(float a_musicLevel)
	{
		m_Mixer.SetFloat(m_parameterName, a_musicLevel);//Sets the level of the chosen parameter.
	}
}