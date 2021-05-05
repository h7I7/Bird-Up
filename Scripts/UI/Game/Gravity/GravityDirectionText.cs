using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GravityDirectionText : MonoBehaviour {

    private Text m_txt;
    private float gravityConst;

	// Use this for initialization
	void Awake () {
        m_txt = GetComponent<Text>();

        gravityConst = 1f / 9.8f;
    }

	// Update is called once per frame
	void Update () {
        m_txt.text = (System.Math.Round(Physics2D.gravity.magnitude * gravityConst, 2)).ToString() + "x";
	}
}
