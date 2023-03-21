using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = true;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private BoxCollider2D m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] ParticleSystem dustParticles;


	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public float groundedMemoryTime;
	float groundedMemory = 0;
	public float jumpMemoryTime;
	float jumpMemory = 0;


	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		groundedMemory -= Time.fixedDeltaTime;
		jumpMemory -= Time.fixedDeltaTime;

		if(Physics2D.IsTouchingLayers(m_GroundCheck, m_WhatIsGround))
		{ 
			m_Grounded = true;
			groundedMemory = groundedMemoryTime;

			if (!wasGrounded)
            {
				dustParticles.Play();
				AudioManager.Play(AudioClipName.Land);
            }
		}

	}


	public bool Move(float move, bool jump)
	{
		if(jump)
        {
			jumpMemory = jumpMemoryTime;
        }
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if ((groundedMemory > 0) && (jumpMemory > 0))
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			groundedMemory = 0;
			jumpMemory = 0;
			dustParticles.Play();
			AudioManager.Play(AudioClipName.Jump);
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
			return true;
		}
		return false;
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		dustParticles.Play();
		AudioManager.Play(AudioClipName.Turn);
	}

}
