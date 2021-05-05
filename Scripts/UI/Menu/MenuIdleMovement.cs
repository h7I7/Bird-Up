using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIdleMovement : MonoBehaviour {

    public float moveSpeedMultplier = 1f;
    [SerializeField]
    public Vector3 movementVector;

	// Update is called once per frame
	void Update () {
        transform.position += movementVector * moveSpeedMultplier * Time.deltaTime;
	}
}
