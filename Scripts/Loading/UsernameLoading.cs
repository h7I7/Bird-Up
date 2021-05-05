using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsernameLoading : MonoBehaviour {

    public Text txt = null;
    public InputField inp = null;
    public Button btn = null;

	// Use this for initialization
	IEnumerator Start () {

        while (SaveManager.Instance == null)
            yield return null;

        if (SaveManager.Instance.state.Name != null)
        {
            txt.text = "Welcome back " + SaveManager.Instance.state.Name + "!";
            inp.gameObject.SetActive(false);
            btn.gameObject.SetActive(false);
        }
	}
	
	public void SetName()
    {
        if (inp.text == null)
            return;

        SaveManager.Instance.state.Name = inp.text;
        txt.text = "Hello " + SaveManager.Instance.state.Name + "!";

        inp.gameObject.SetActive(false);
        btn.gameObject.SetActive(false);
    }

}
