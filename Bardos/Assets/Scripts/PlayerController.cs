using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;

public class PlayerController : MonoBehaviour 
{
	public float MovementSpeed = 500f;
	public float JumpHeight = 1.0f;
	public float WallSlideSpeed = 250;
	public Rigidbody2D RB;

	private int JumpCount = 0;
	private Vector2 DirectionFacing;
	private bool IsTouchingGround;
	private bool IsAttacking = false;
	private bool IsWallSliding = false;

	//Coroutine that pauses value for X amount of seconds, then changes it
	private IEnumerator AttackTimer()
	{
		IsAttacking = true;
		yield return new WaitForSeconds (0.5f);
		IsAttacking = false;
	}

	// Update is called once per frame
	void Update () 
	{
		//Player movement on horizontal axis based on public float and Time.deltaTime
		float MovementLeftRight = Input.GetAxis ("Horizontal") * MovementSpeed * Time.deltaTime;
		DirectionFacing = new Vector2 (MovementLeftRight, 0);

		//If JumpCount is 0 and jump button pressed, player jumps, increases jump count and sets IsTouchingGround bool to false
		if (JumpCount == 0 && Input.GetButtonDown("Jump")) 
		{
			UnityEngine.Debug.Log ("Jumping");
			RB.AddForce(Vector2.up * JumpHeight);
			JumpCount = 1;
			IsTouchingGround = false;
			//Play jump animation
			//Play jump sound
		}

		//If player hasnt attacked and is touching the ground on mouse button click, player dashes forwards, and AttackTimerRuns 
		if (IsAttacking == false && IsTouchingGround == true && Input.GetMouseButtonDown(0)) 
		{
			UnityEngine.Debug.Log ("Punching");
			RB.velocity = new Vector2 (MovementLeftRight, 0);
			StartCoroutine(AttackTimer());
			//Play punch animation
			//Play punch sound?
		}

		//If player hasnt attacked and is not touching the ground on mouse click, player slams down until they touch the ground
		if (IsAttacking == false && IsTouchingGround == false && Input.GetMouseButtonDown (0)) 
		{
			UnityEngine.Debug.Log ("Ground Pounding");

			RB.AddForce(Vector2.down);
			StartCoroutine(AttackTimer ());
			//Play ground pound animation
			//Play ground pound sound?
		}

		if (IsWallSliding == true && IsTouchingGround == false && Input.GetButtonDown("Jump")) 
		{
			UnityEngine.Debug.Log ("Wall Jumping");

			if(RB.velocity.x <= -1)
			{
				RB.AddForce(Vector2.up * JumpHeight);
				RB.velocity = new Vector2 (-MovementLeftRight, 0);
				IsWallSliding = false;
			}

			if(RB.velocity.x >= 1)
			{
				RB.AddForce(Vector2.up * JumpHeight);
				RB.velocity = new Vector2 (MovementLeftRight, 0);
				IsWallSliding = false;
			}
		}
	}

	void FixedUpdate()
	{
		RB.AddForce (DirectionFacing);
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		//On ground touch resets JumpCount and and IsTouchingGround
		if (other.gameObject.CompareTag (Tags.Ground)) 
		{
			JumpCount = 0;
			IsTouchingGround = true;
			//Play landing sound		}
		}

		if (other.gameObject.CompareTag (Tags.Enemy) && IsAttacking == true) 
		{
			Destroy (other.gameObject);
			//Play Enemy death animation
			//Play on hit sound
			//Play Enemy death sound
		}

		if (other.gameObject.CompareTag (Tags.Wall) && IsTouchingGround == false) 
		{
			UnityEngine.Debug.Log ("Wall Sliding");
			IsWallSliding = true;
			JumpCount = 0;

			if (RB.velocity.y < -WallSlideSpeed)
			{
				RB.velocity = new Vector2 (0, ((MovementSpeed * Time.deltaTime) / 1.5f));
			}
		}
	}
}
