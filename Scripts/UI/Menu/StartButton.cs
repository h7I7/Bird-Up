using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

    public float shakeAmount;
    private Vector3 startingTransform;
    public float colorChangeSpeed = 1f;

    private CanvasRenderer childText;

	// Use this for initialization
	void Start () {
        startingTransform = transform.localPosition;

        if (transform.childCount > 0)
            childText = transform.GetChild(0).GetComponent<CanvasRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(startingTransform.x + Random.Range(0f, shakeAmount), startingTransform.y + Random.Range(0f, shakeAmount), startingTransform.z);
        childText.SetColor(HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * colorChangeSpeed, 1f), 1f, 1f)));
	}
}
