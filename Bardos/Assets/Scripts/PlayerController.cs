using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float MovementSpeed = 100f;
	public float JumpHeight = 1.0f;
	public Rigidbody2D RB;

	private int JumpCount = 0;

	// Update is called once per frame
	void Update () 
	{
		if ( JumpCount == 0 && Input.GetButtonDown("Jump")) 
		{
			RB.AddForce(Vector3.up * JumpHeight);
			JumpCount = 1;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag (Tags.Ground)) 
		{
			JumpCount = 0;
		}
	}
}
