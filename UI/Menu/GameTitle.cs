using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTitle : MonoBehaviour {

    public float menuZoomSpeedMultiplier = 1f;
    public float minTitleScale = 0.5f;

	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3(1f, 1f, 0f) * (Mathf.Abs(Mathf.Sin(Time.time * menuZoomSpeedMultiplier)) + minTitleScale);
	}
}
