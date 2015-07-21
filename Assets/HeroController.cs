using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

	GameObject player;
	float minJumpForce = 150;
	float maxJumpForce = 300;
	float currentJumpForce = 50;

	public LayerMask groundLayer;

	// Use this for initialization
	void Start () {
		player = this.gameObject;
		currentJumpForce = 0;
	}

	void Update ()
	{
		bool grounded = Physics2D.OverlapCircle (player.transform.position, 0.2f, groundLayer);
		if (Input.GetKey ("right"))
		{
			Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
			rigidbody.AddForce(new Vector2(10,0));
		}
		else if (Input.GetKey ("left"))
		{
			Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
			rigidbody.AddForce(new Vector2(-10,0));
		}

		if (grounded) {
			if (Input.GetKey ("space")) {
				currentJumpForce += 5;

				if (currentJumpForce > maxJumpForce)
					currentJumpForce = maxJumpForce;

				Debug.Log ("charge " + currentJumpForce);
			}

			if (Input.GetKeyUp ("space")) {
				Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D> ();
				rigidbody.AddForce (new Vector2 (0, currentJumpForce));
				Debug.Log ("saut charger a " + currentJumpForce);
				currentJumpForce = minJumpForce;
			}
		}
	}
}
