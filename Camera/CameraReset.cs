using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour {

	public void ResetPosition()
    {
        transform.position = new Vector3(0f, 0f, 0f);
    }
}
