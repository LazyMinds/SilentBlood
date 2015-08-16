using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	private bool hasHit = false;
	private Vector3 v3;
	private Quaternion quat;
	
	void FixedUpdate()
	{
		if (!hasHit) {
			Vector3 dir = GetComponent<Rigidbody2D> ().velocity;
			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

			v3 = transform.position;
			quat = transform.rotation;
		}
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		transform.position = v3;
		transform.rotation = quat;

		if (col.gameObject.tag == "Enemies") {
			Physics2D.IgnoreCollision (GetComponent<Collider2D> (), col.gameObject.GetComponent<Collider2D> ());
			transform.SetParent (col.gameObject.transform);
		}
		GetComponent<Rigidbody2D>().isKinematic = true;
		hasHit = true;
		Destroy (gameObject, 1);
	}
}
