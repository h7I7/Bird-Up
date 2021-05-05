//\===========================================================================================
//\ File: Separate.cs
//\ Author: Morgan James
//\ Brief: Attempts to move the entity's from the inside of platforms.
//\===========================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separate : MonoBehaviour
{
	private float timeRemaining = 0.25f;//How long the entity will be colliding with the platforms for.

	void Start()
	{
		//Turns trigger off to try to move the entity away from rigid bodies.
		GetComponent<CircleCollider2D>().isTrigger = false;

		//Move the entity to give it a nudge as when you move them it sometimes works better.
		transform.Translate(Vector3.right * 1f);//Move it the right a bit.
		transform.Translate(Vector3.up * 1f);//Move it up a bit.
	}

	void Update()
	{
		timeRemaining -= Time.deltaTime * 0.5f;//Decrease the timer.
		if (timeRemaining < 0.0f)//If the timer reaches 0.
		{
			GetComponent<CircleCollider2D>().isTrigger = true;//Turn the trigger back on to stop platform collisions.

			//Stop the entity moving.
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			GetComponent<Rigidbody2D>().angularVelocity = 0.0f;

			//Reset the rotation as it may have rotated.
			transform.rotation = new Quaternion(0, 0, 0, 0);

			//Destroy this script as we don't need it anymore.
			Destroy(GetComponent<Separate>());
		}
	}
}
