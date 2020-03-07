using UnityEngine;
using System.Collections;

public class HornScript : MonoBehaviour {

	public GameObject player;
	public float speed;
	public string key;
	void Start () {
	
	}
	
	void Update () {
		if (player == null) {
			Destroy (gameObject);
		} else {
			transform.Rotate(new Vector3 (0, 0, speed));
			transform.position = player.transform.position;

			if (Input.GetKeyDown (key) && player.GetComponent<PlayerBehavior> ().canDash) {
				player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				player.GetComponent<Rigidbody> ().AddExplosionForce (25, transform.GetChild (2).position, 0, 0f, ForceMode.VelocityChange);
				//player.GetComponent<Rigidbody> ().AddForce(Vector3.up + Vector3.Angle(player.transform.position, transform.GetChild (2).position)*10, ForceMode.VelocityChange);
				player.GetComponent<Rigidbody> ().useGravity = true;

				player.GetComponent<PlayerBehavior> ().state = 1;
				player.GetComponent<PlayerBehavior> ().canDash = false;
				player.transform.GetChild (1).gameObject.SetActive (true);
				Invoke ("CancelDash", 0.8f);
			}
		}
	}
	void CancelDash() {
		player.GetComponent<PlayerBehavior> ().CancelDash ();
	}
}
