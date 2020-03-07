using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour {

	public float lifetime;
	float timer;

	void Start () {
		timer = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += 0.1f;
		if (timer >= lifetime)
			Destroy (gameObject);
	}
}
