using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class PickupController : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem particle;
	[SerializeField]
	private float m_amountToCharge;

	private bool m_particlesPlayed = false;
    public bool particlesPlayed { get { return m_particlesPlayed; } }

    [SerializeField]
    private bool m_hideSpriteOnCollision = false;

	public void PlayerCollision()
    {
        if (m_hideSpriteOnCollision)
		{
			GetComponent<SpriteRenderer>().enabled = false;

		}

		PlayerController.instance.ChargeJump(m_amountToCharge);

		particle.Play();
		GetComponent<AudioSource>().Play();

		m_particlesPlayed = true;
    }
}
