using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImage : MonoBehaviour {

    public float birdYSpeed = 10f;
    [Range(0f, 5f)]
    public float birdYMovePercent = 1f;
    public float birdXSpeed = 5f;

    public Texture birdImage1 = null;
    public Texture birdImage2 = null;
    private bool birdSwap = true;

    public float animationSpeed = 1f;
    private float timer = 0f;

    private float screenLeeway = 0f;

    void Awake()
    {
        screenLeeway = ((birdImage1.width + birdImage2.width) * 0.5f) * 1.25f;

        timer = Time.time - (1f / animationSpeed);
    }

	// Update is called once per frame
	void Update () {

        transform.position += Vector3.up * Mathf.Sin(Time.time * birdYSpeed) * birdYMovePercent;
        transform.position += Vector3.right * birdXSpeed;

        if (transform.position.x > Screen.width + screenLeeway)
        {
            transform.position += Vector3.right * -(Screen.width + screenLeeway * 1.5f);
        }

        if (birdImage1 != null && birdImage2 != null)
        {
            if (Time.time >= timer)
            {
                if (birdSwap)
                {
                    GetComponent<RawImage>().texture = birdImage1;
                }
                else
                {
                    GetComponent<RawImage>().texture = birdImage2;
                }
                birdSwap = !birdSwap;
                timer = Time.time + (1f / animationSpeed);
            }
        }
	}
}
