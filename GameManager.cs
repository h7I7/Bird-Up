using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // A variable to store the timescale before pausing so we can resume at the same timescale later
    private float timeScaleTemp = -1f;

	public void PauseGame()
    {
        timeScaleTemp = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        if (timeScaleTemp != -1f)
            Time.timeScale = timeScaleTemp;
        else
            Time.timeScale = 1f;
    }
}
