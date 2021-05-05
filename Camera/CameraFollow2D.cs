using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField]
    private float m_trackSpeed;

	public GameObject objectToTrack;

	void FixedUpdate ()
	{
		transform.parent.position = Vector3.Lerp(transform.parent.position, new Vector3(objectToTrack.transform.position.x, objectToTrack.transform.position.y, transform.position.z), m_trackSpeed * Time.deltaTime);
	}

    public void TrackOther(GameObject a_other)
    {
        objectToTrack = a_other;
    }
}
