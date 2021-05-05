using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeOut : MonoBehaviour {

    public float fadeSpeedMultiplier = 1f;
	private CanvasGroup fadeGroup = null;

    private void Start()
    {
        fadeGroup = GetComponent<CanvasGroup>();
        fadeGroup.alpha = 1f;
        StartCoroutine(Fade());
    }

	// Update is called once per frame
	private IEnumerator Fade () {

        WaitForSeconds delay = new WaitForSeconds(0.01f * fadeSpeedMultiplier);

        while (fadeGroup.alpha > 0)
        {
            fadeGroup.alpha -= 0.01f;
            yield return delay;
        }
	}
}
