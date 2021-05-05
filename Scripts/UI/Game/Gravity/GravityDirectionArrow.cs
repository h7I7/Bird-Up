using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GravityDirectionArrow : MonoBehaviour {

    private Image m_img;

	// Use this for initialization
	void Awake () {
        m_img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        m_img.transform.rotation = Quaternion.LookRotation(Vector3.forward, Physics2D.gravity);
	}
}
