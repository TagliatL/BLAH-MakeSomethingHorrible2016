using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	[HideInInspector]
	public GameObject player1;
	[HideInInspector]
	public GameObject player2;
	[HideInInspector]
	public GameObject player3;
	[HideInInspector]
	public GameObject player4;

	public GameObject[] arenasTwoPlayers;
	public GameObject[] arenasThreePlayers;
	public GameObject[] arenasFourPlayers;

	public GameObject UITransition2;
	public Text UIScore11;
	public Text UIScore12;
	public GameObject UITransition3;
	public Text UIScore21;
	public Text UIScore22;
	public Text UIScore23;
	public GameObject UITransition4;
	public Text UIScore31;
	public Text UIScore32;
	public Text UIScore33;
	public Text UIScore34;

	public GameObject WON;


	public GameObject UIMenu;

	public int scorePlayer1;
	public int scorePlayer2;
	public int scorePlayer3;
	public int scorePlayer4;

	public int nbPlayers;

	public int state = 0;

	GameObject _arena;
	GameObject _savedArena;
	int randomArena;
	bool roundEnded;

	void Start () {
	
	}
	
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("fight");
		}

		if (state == 0) {
			if (Input.GetKeyDown (KeyCode.A)) {
				//solo
			} else if (Input.GetKeyDown (KeyCode.E)) {
				UITransition2.SetActive (true);
				Invoke ("InstantiateMap2", 0.5f);
				Invoke ("HideUIMenu", 0.5f);
				state++;
			} else if (Input.GetKeyDown (KeyCode.T)) {
				UITransition3.SetActive (true);
				Invoke ("InstantiateMap3", 0.5f);
				Invoke ("HideUIMenu", 0.5f);
				state++;
			} else if (Input.GetKeyDown (KeyCode.U)) {
				UITransition4.SetActive (true);
				Invoke ("InstantiateMap4", 0.5f);
				Invoke ("HideUIMenu", 0.5f);
				state++;
			}
		}

		if (state == 1 && !roundEnded) {
			if (GameObject.FindGameObjectsWithTag ("Player").Length == 1) {
				if (GameObject.FindGameObjectWithTag ("Player") == player1) {
					scorePlayer1++;
				} else if (GameObject.FindGameObjectWithTag ("Player") == player2) {
					scorePlayer2++;
				} else if (GameObject.FindGameObjectWithTag ("Player") == player3) {
					scorePlayer3++;
				} else if (GameObject.FindGameObjectWithTag ("Player") == player4) {
					scorePlayer4++;
				}
				roundEnded = true;
				Invoke ("RoundEnd",1f);
				if (scorePlayer1 == 3 || scorePlayer2 == 3 || scorePlayer3 == 3 || scorePlayer4 == 3) {
					Invoke ("Winner", 1.5f);
				} else {
					Invoke ("Reset", 1.8f);
				}
			}
		}

		if (state == 2 && Input.anyKeyDown) {
			SceneManager.LoadScene ("fight");
		}
	}

	void Winner() {
		Destroy (_arena);
		Destroy (GameObject.FindGameObjectWithTag ("Player").GetComponent<SphereCollider> ());
		GameObject.FindGameObjectWithTag ("Player").transform.parent = null;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody> ().useGravity = false;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody> ().velocity = Vector3.zero;
		GameObject.FindGameObjectWithTag ("Player").transform.localScale = new Vector3 (6, 6, 6);
		GameObject.FindGameObjectWithTag ("Player").transform.position = new Vector3 (-5, 0, 0);
		Invoke ("ShowWon", 0.5f);
		state++;
	}
	void ShowWon() {
		WON.SetActive (true);

	}

	void RoundEnd() {
		if (nbPlayers == 2) {
			UIScore11.text = scorePlayer1.ToString();
			UIScore12.text = scorePlayer2.ToString ();
			UITransition2.GetComponent<Animation> ().Play ("transition2Joueurs");
		} else if (nbPlayers == 3) {
			UIScore21.text = scorePlayer1.ToString();
			UIScore22.text = scorePlayer2.ToString ();
			UIScore23.text = scorePlayer3.ToString ();
			UITransition3.GetComponent<Animation> ().Play ("transition3Joueurs");
		} else if (nbPlayers == 4) {
			UIScore31.text = scorePlayer1.ToString();
			UIScore32.text = scorePlayer2.ToString ();
			UIScore33.text = scorePlayer3.ToString ();
			UIScore34.text = scorePlayer4.ToString ();
			UITransition4.GetComponent<Animation> ().Play ("transition4Joueurs");
		} 
		Destroy (GameObject.FindGameObjectWithTag ("Player").GetComponent<SphereCollider> ());
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

	void Reset() {
		Destroy (_arena);
		roundEnded = false;
		_arena = Instantiate(_savedArena, Vector3.zero, Quaternion.identity) as GameObject;
	}

	void HideUIMenu() {
		UIMenu.SetActive (false);
	}

	void InstantiateMap2() {
		randomArena = Random.Range (0, arenasTwoPlayers.Length);
		_arena = Instantiate (arenasTwoPlayers [randomArena], Vector3.zero, Quaternion.identity) as GameObject;
		_savedArena = arenasTwoPlayers [randomArena];
		nbPlayers = 2;
	}

	void InstantiateMap3() {
		randomArena = Random.Range (0, arenasThreePlayers.Length);
		_arena = Instantiate (arenasThreePlayers [randomArena], Vector3.zero, Quaternion.identity) as GameObject;
		_savedArena = arenasThreePlayers [randomArena];
		nbPlayers = 3;
	}

	void InstantiateMap4() {
		randomArena = Random.Range (0, arenasFourPlayers.Length);
		_arena = Instantiate (arenasFourPlayers [randomArena], Vector3.zero, Quaternion.identity) as GameObject;
		_savedArena = arenasFourPlayers [randomArena];
		nbPlayers = 4;
	}

}
