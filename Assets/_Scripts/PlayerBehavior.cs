using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	public GameObject horn;
	public float speed;
	[HideInInspector]
	public GameObject _horn;
	public GameObject deathParticle;
	public GameObject dashReady;
	public GameObject clash;
	public bool canDash;
	public Color color;
	public string key;
	public int number;
	public int state;

	GameObject _death;
	GameObject _readyDash;
	GameObject _impact;
	GameObject _clash;
	Vector3 velocitySaved;
	Vector3 normal = Vector3.zero;
	Vector3 reflectResult;

	void Start () {
		_horn = Instantiate (horn, transform.position, Quaternion.identity) as GameObject;
		_horn.GetComponent<HornScript> ().player = gameObject;
		_horn.GetComponent<HornScript> ().speed = speed;
		_horn.GetComponent<HornScript> ().key = key;

		if (number == 1) {
			GameObject.FindGameObjectWithTag ("Manager").GetComponent<GameManager> ().player1 = gameObject;
		}
		if (number == 2) {
			GameObject.FindGameObjectWithTag ("Manager").GetComponent<GameManager> ().player2= gameObject;
		}
		if (number == 3) {
			GameObject.FindGameObjectWithTag ("Manager").GetComponent<GameManager> ().player3= gameObject;
		}
		if (number == 4) {
			GameObject.FindGameObjectWithTag ("Manager").GetComponent<GameManager> ().player4= gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		velocitySaved = GetComponent<Rigidbody> ().velocity;
		if (transform.GetChild (1).gameObject.activeInHierarchy) {
			transform.GetChild (1).GetComponent<TrailRenderer> ().startWidth -= 0.025f;
			transform.GetChild (1).GetComponent<TrailRenderer> ().endWidth -= 0.025f;
		}
	}

	void Shake(){
		if ((velocitySaved.x > 0 && velocitySaved.y > 0 && velocitySaved.x > velocitySaved.y) 
			|| (velocitySaved.x < 0 && velocitySaved.y < 0 && velocitySaved.x < velocitySaved.y) 
			|| (velocitySaved.x > 0 && velocitySaved.y < 0 && -velocitySaved.x < velocitySaved.y) 
			|| (velocitySaved.x < 0 && velocitySaved.y > 0 && velocitySaved.x < -velocitySaved.y)) {
			GameObject.FindGameObjectWithTag ("MainCamera").transform.parent.GetComponent<Screenshake> ().ShakeHorizontal (0.5f, 1, velocitySaved.magnitude/10);

		} else {
			GameObject.FindGameObjectWithTag ("MainCamera").transform.parent.GetComponent<Screenshake> ().ShakeVertical (0.5f, 1, velocitySaved.magnitude/10);
		}
	}

	void OnCollisionEnter(Collision other) {
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
	
		if (other.gameObject.tag == "Bounce") {
			reflectResult = Vector3.Reflect (velocitySaved*1.2f, other.contacts [0].normal);
			Shake ();
			Invoke ("BackToSpeed", 0.1f);
		}else if (other.gameObject.tag == "LittleBounce") {
			reflectResult = Vector3.Reflect (velocitySaved*0.4f, other.contacts [0].normal);
			Shake ();
			Invoke ("BackToSpeed", 0.1f);
		}else if (other.gameObject.tag == "Stop") {
			reflectResult = Vector3.Reflect (Vector3.zero, other.contacts [0].normal);
			GetComponent<Rigidbody> ().rotation = Quaternion.Euler (Vector3.zero);
			GetComponent<Rigidbody> ().useGravity = false;
			Invoke ("BackToSpeed", 0.1f);
		}

		if (other.gameObject.tag == "Player") {
			if (other.gameObject.GetComponent<PlayerBehavior> ().state > GetComponent<PlayerBehavior> ().state) {
				Die ();
			} else if (other.gameObject.GetComponent<PlayerBehavior> ().state < GetComponent<PlayerBehavior> ().state) {
				GetComponent<Rigidbody> ().velocity = Vector3.Reflect (velocitySaved*0.7f, other.contacts [0].normal);
				Shake ();
			} else if (other.gameObject.GetComponent<PlayerBehavior> ().state == GetComponent<PlayerBehavior> ().state) {
				GetComponent<Rigidbody> ().velocity = Vector3.Reflect (velocitySaved*0.8f, other.contacts [0].normal);
				_clash = Instantiate (clash, Vector3.Lerp(transform.position, other.transform.position,0.5f), Quaternion.identity)as GameObject;
				_clash.transform.GetChild(0).GetComponent<ParticleSystem> ().startColor = color;
				Shake ();
			}
		}
	}

	public void CancelDash() {
		transform.GetChild (1).gameObject.SetActive (false);
		state = 0;
		transform.GetChild (1).GetComponent<TrailRenderer> ().startWidth = 1f;
		transform.GetChild (1).GetComponent<TrailRenderer> ().endWidth = 1f;
		Invoke ("CoolDown", 0.2f);
	}

	void CoolDown() {
		canDash = true;
		_readyDash =  Instantiate (dashReady, transform.position, Quaternion.identity) as GameObject;
		_readyDash.transform.parent = transform;
		_readyDash.GetComponent<ParticleSystem> ().startColor = color;
	}

	void BackToSpeed() {
		GetComponent<Rigidbody> ().velocity = reflectResult;
		//GetComponent<Rigidbody> ().velocity = velocitySaved;
	}


	void Die() {
		_death = Instantiate (deathParticle, transform.position, Quaternion.identity) as GameObject;
		_death.transform.GetChild(0).GetComponent<ParticleSystem> ().startColor = color;
		_death.transform.GetChild(1).GetComponent<ParticleSystem> ().startColor = color;
		Destroy (gameObject);
	}
}
