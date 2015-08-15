using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeroController : MonoBehaviour {

	public LayerMask groundLayer;
	public Text inGameConsole;

	private GameObject player;

	private float runForceByUpdate = 10;

	private float chargeJumpByUpdate = 10;
	private float minJumpForce = 300;
	private float maxJumpForce = 600;
	private float currentJumpForce = 50;
	
	private float groundDistance = 0;
	private float groundTolerance = 0.1f;

	private float maxVelocity = 5;
	private float maxVelocityForChargeJump = 0.5f;

	private bool grounded = true;
	private bool chargeJump = false;
	

	void Start ()
	{
		player = this.gameObject;
		currentJumpForce = minJumpForce;
	}

	void Update ()
	{
		// Camera follow
		Camera.main.transform.position = new Vector3 (player.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);


		Collider2D collider2d = player.GetComponent<Collider2D> ();
		Vector3 playerBottomLeft = new Vector3 (collider2d.transform.position.x - collider2d.bounds.size.x / 2, collider2d.transform.position.y - (collider2d.bounds.size.y / 2) + collider2d.offset.y, collider2d.transform.position.z);
		Vector3 playerBottomMid = new Vector3 (collider2d.transform.position.x, collider2d.transform.position.y - (collider2d.bounds.size.y / 2) + collider2d.offset.y, collider2d.transform.position.z);
		Vector3 playerBottomRight = new Vector3 (collider2d.transform.position.x + collider2d.bounds.size.x / 2, collider2d.transform.position.y - (collider2d.bounds.size.y / 2) + collider2d.offset.y, collider2d.transform.position.z);

		RaycastHit2D hit1 = Physics2D.Raycast(playerBottomLeft, -Vector2.up, Mathf.Infinity, groundLayer);
		RaycastHit2D hit2 = Physics2D.Raycast(playerBottomMid, -Vector2.up, Mathf.Infinity, groundLayer);
		RaycastHit2D hit3 = Physics2D.Raycast(playerBottomRight, -Vector2.up, Mathf.Infinity, groundLayer);
		/*Color color1 = hit1 ? Color.blue : Color.red;
		Debug.DrawRay(playerBottomLeft, -Vector2.up, color1);*/
		float groundDistanceTemp1 = Mathf.Abs(hit1.point.y - playerBottomLeft.y);
		float groundDistanceTemp2 = Mathf.Abs(hit2.point.y - playerBottomRight.y);
		float groundDistanceTemp3 = Mathf.Abs(hit3.point.y - playerBottomRight.y);


		grounded = ((groundDistanceTemp1 < groundTolerance) || (groundDistanceTemp2 < groundTolerance) || (groundDistanceTemp3 < groundTolerance));
		if (grounded)
		{
			if (groundDistanceTemp1 <= groundDistanceTemp2 && groundDistanceTemp1 <= groundDistanceTemp3)
				groundDistance = groundDistanceTemp1;
			else if (groundDistanceTemp2 <= groundDistanceTemp1 && groundDistanceTemp2 <= groundDistanceTemp3)
				groundDistance = groundDistanceTemp2;
			else
				groundDistance = groundDistanceTemp3;
		}

		// Jump & Charged Jump
		if (grounded) {
			if (Input.GetKey ("space")) {
				if (Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.x) < maxVelocityForChargeJump)
				{
					chargeJump = true;
					currentJumpForce += chargeJumpByUpdate;
				}
				else
				{
					chargeJump = false;
					currentJumpForce = minJumpForce;
				}
				
				if (currentJumpForce > maxJumpForce)
					currentJumpForce = maxJumpForce;
			}
			
			if (Input.GetKeyUp ("space")) {
				chargeJump = false;
				Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D> ();
				rigidbody.AddForce (new Vector2 (0, currentJumpForce));
				currentJumpForce = minJumpForce;
			}
		}

		if (Input.GetKey ("right"))
		{
			player.transform.rotation = Quaternion.Euler(0, -180, 0);
			Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
			rigidbody.AddForce(new Vector2(runForceByUpdate,0));
			if (rigidbody.velocity.x > maxVelocity)
				rigidbody.velocity = new Vector2(maxVelocity, rigidbody.velocity.y);
		}
		else if (Input.GetKey ("left"))
		{
			player.transform.rotation = Quaternion.Euler(0, 0, 0);
			Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
			rigidbody.AddForce(new Vector2(-runForceByUpdate,0));
			if (rigidbody.velocity.x < -maxVelocity)
				rigidbody.velocity = new Vector2(-maxVelocity, rigidbody.velocity.y);
		}

		refreshDataAnimation ();    
		showDebugIngameConsole ();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Bot"))
        {
            Debug.Log("Player Sound Detected");
        }
    }

	private void showDebugIngameConsole()
	{
		inGameConsole.text =
			"ChargeJump : "+chargeJump.ToString() + "\n" +
			"ChargeJumpForce : " + currentJumpForce.ToString () + "\n" +
			"Velocity x : " + GetComponent<Rigidbody2D> ().velocity.x.ToString () + "\n" +
			"Velocity y : " + GetComponent<Rigidbody2D> ().velocity.y.ToString () + "\n" +
			"Grounded : " + grounded.ToString () + "\n" +
			"Ground Distance : " + groundDistance.ToString () + "\n" +
			"Direction : " + player.transform.eulerAngles.y;
	}

	private void refreshDataAnimation ()
	{
		GetComponent<Animator> ().SetFloat ("VelocityNegX", GetComponent<Rigidbody2D> ().velocity.x);
		GetComponent<Animator> ().SetFloat ("VelocityX", Mathf.Abs(GetComponent<Rigidbody2D> ().velocity.x));
		GetComponent<Animator> ().SetFloat ("VelocityY", GetComponent<Rigidbody2D> ().velocity.y);
		GetComponent<Animator> ().SetBool ("Grounded", grounded);
		GetComponent<Animator> ().SetBool ("ChargeJump", chargeJump);
		GetComponent<Animator> ().SetInteger("Direction", (int)player.transform.eulerAngles.y);
		GetComponent<Animator> ().SetFloat("GroundDistance", groundDistance);
	}
}
