using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeroController : MonoBehaviour {

	public LayerMask groundLayer;
	public Text inGameConsole;
	public GameObject arrow;
	public GameObject sword;

	private GameObject player = null;
	private Rigidbody2D rigidbody = null;
	private GameObject dir = null;

	private Console console = null;


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
		console = Camera.main.GetComponent<Console> ();
		player = this.gameObject;
		rigidbody = player.GetComponent<Rigidbody2D> (); 
		currentJumpForce = minJumpForce;
		dir = Instantiate (arrow, player.transform.position, Quaternion.identity) as GameObject;
	}

	void Update ()
	{
		// WIP
		dir.transform.position = player.transform.position;
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = player.transform.position.z;
		dir.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((mousePosition.y - player.transform.position.y), (mousePosition.x - player.transform.position.x))*Mathf.Rad2Deg);
		Vector3 distanceFromObject = mousePosition - player.transform.position;
		dir.transform.localScale = new Vector3 (distanceFromObject.magnitude, dir.transform.localScale.y, dir.transform.localScale.z);

		if (Input.GetMouseButtonUp(0))
		{
			GameObject swordThrow = Instantiate (sword, player.transform.position, Quaternion.Inverse(dir.transform.rotation)) as GameObject;
			Physics2D.IgnoreCollision (swordThrow.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
			swordThrow.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (distanceFromObject.x*30, distanceFromObject.y*30));
		}
		// End WIP



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
		updateConsoleData ();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Bot"))
        {
			Camera.main.GetComponent<Console>().SoundSpotted = true;
        }
    }

	void OnTriggerExit2D()
	{
		Camera.main.GetComponent<Console>().SoundSpotted = false;
	}

	private void updateConsoleData()
	{
		console.ChargeJump = chargeJump;
		console.CurrentJumpForce = currentJumpForce;
		console.VelocityX = rigidbody.velocity.x;
		console.VelocityY = rigidbody.velocity.y;
		console.Grounded = grounded;
		console.GroundDistance = groundDistance;
		console.Direction = (int)player.transform.eulerAngles.y;
	}

	private void refreshDataAnimation ()
	{
		GetComponent<Animator> ().SetFloat ("VelocityNegX", rigidbody.velocity.x);
		GetComponent<Animator> ().SetFloat ("VelocityX", Mathf.Abs(rigidbody.velocity.x));
		GetComponent<Animator> ().SetFloat ("VelocityY", rigidbody.velocity.y);
		GetComponent<Animator> ().SetBool ("Grounded", grounded);
		GetComponent<Animator> ().SetBool ("ChargeJump", chargeJump);
		GetComponent<Animator> ().SetInteger("Direction", (int)player.transform.eulerAngles.y);
		GetComponent<Animator> ().SetFloat("GroundDistance", groundDistance);
	}
}
