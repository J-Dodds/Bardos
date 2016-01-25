﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float MovementSpeed = 500f;
	public float JumpHeight = 1.0f;
	public Rigidbody2D RB;

	private int JumpCount = 0;
	private int AttackCount = 0;
	private Vector2 DirectionFacing;
	private bool IsTouchingGround;
	private bool IsAttacking;

	// Update is called once per frame
	void Update () 
	{
		float MovementLeftRight = Input.GetAxis ("Horizontal") * MovementSpeed * Time.deltaTime;
		DirectionFacing = new Vector2 (MovementLeftRight, 0);

		if (JumpCount == 0 && Input.GetButtonDown("Jump")) 
		{
			RB.AddForce(Vector2 * JumpHeight);
			JumpCount = 1;
			IsTouchingGround = false;
		}

		if (AttackCount == 0 && IsTouchingGround == true && Input.GetMouseButtonDown(0)) 
		{
			RB.AddForce (transform.forward * MovementSpeed);     
		}
	}

	void FixedUpdate()
	{
		RB.AddForce (DirectionFacing);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag (Tags.Ground)) 
		{
			JumpCount = 0;
			IsTouchingGround = true;
		}
	}
}
