using UnityEngine;
using System.Collections;

public class Vision : MonoBehaviour {

	public GameObject player;

	private int angleOfView = 50;
	private float distVision = 6;
	private bool spotted = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		spotted = false;
		if (Vector2.Angle (-transform.right, player.transform.position - transform.position) < angleOfView)
		{
			RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.right, distVision);
			if ((hits.Length > 1) && hits[1].collider.tag == "Player")
			{
				spotted = true;
			}
		}
		Camera.main.GetComponent<Console> ().ViewSpotted = spotted;
	}
}
