using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Console : MonoBehaviour {

	public Text inGameConsoleText = null;

	private bool chargeJump = false;
	private float currentJumpForce = 0;
	private float velocityX = 0;
	private float velocityY = 0;
	private bool grounded = true;
	private float groundDistance = 0;
	private int direction = 0;
	private bool soundSpotted = false;
	private bool viewSpotted = false;

	void Update () {
		if (inGameConsoleText != null) {
			inGameConsoleText.text =
				"ChargeJump : "+chargeJump.ToString() + "\n" +
				"ChargeJumpForce : " + currentJumpForce.ToString () + "\n" +
				"Velocity x : " + velocityX.ToString () + "\n" +
				"Velocity y : " + velocityY.ToString () + "\n" +
				"Grounded : " + grounded.ToString () + "\n" +
				"Ground Distance : " + groundDistance.ToString () + "\n" +
				"\n" +
				"Sound Spotted : " + soundSpotted.ToString() + "\n" +
				"View Spotted : " + viewSpotted.ToString();

		}
	}

	public bool ChargeJump {
		get {
			return chargeJump;
		}
		set {
			chargeJump = value;
		}
	}

	public float CurrentJumpForce {
		get {
			return currentJumpForce;
		}
		set {
			currentJumpForce = value;
		}
	}

	public float VelocityX {
		get {
			return velocityX;
		}
		set {
			velocityX = value;
		}
	}

	public float VelocityY {
		get {
			return velocityY;
		}
		set {
			velocityY = value;
		}
	}

	public bool Grounded {
		get {
			return grounded;
		}
		set {
			grounded = value;
		}
	}

	public float GroundDistance {
		get {
			return groundDistance;
		}
		set {
			groundDistance = value;
		}
	}

	public int Direction {
		get {
			return direction;
		}
		set {
			direction = value;
		}
	}

	public bool SoundSpotted {
		get {
			return soundSpotted;
		}
		set {
			soundSpotted = value;
		}
	}

	public bool ViewSpotted {
		get {
			return viewSpotted;
		}
		set {
			viewSpotted = value;
		}
	}
}
