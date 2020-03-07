using UnityEngine;
using System.Collections;

public class Die : MonoBehaviour {

	public int timeBeforeDeath;
	int timer;
	// Update is called once per frame
	void Update () {
		timer++;
		if (timer >= timeBeforeDeath) {
			Destroy (gameObject);
		}
	}
}
