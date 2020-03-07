using UnityEngine;
using System.Collections;

public class ShonenParticle : MonoBehaviour {

	[Header("Shonen Particle")]
	[Space(10)]
	[Header("Impact Particle Parameters")]
	public int impactID;
	public GameObject impactParticles;
	public Color colorOfParticles;
	Transform spritePosition;
	[Space(10)]
	[Header("Background Color")]
	public Sprite backgroundSprite;
	public Color colorOfBackground;
	[Space(10)]
	[Header("Other Settings")]
	[Range(0.01f, 2.0f)]
	[Tooltip("How many seconds will each frame show up")]
	public float frameTime;
	public GameObject endFX;
	[Tooltip("Time before the next impact")]
	public float coolDown;

	//variables created in runtime
	Transform camTransform;
	GameObject background;
	GameObject impact;
	float timer;
	GameObject instantiatedFX;
	bool coolDownRunning;
	float timerCoolDown;
	Transform savedTransform;

	void Start () {
		timer = 0;
		coolDownRunning = false;
	}

	void Update () {
		if (GameObject.FindGameObjectWithTag ("CamImpact")) {
			camTransform = GameObject.FindGameObjectWithTag("CamImpact").transform;
		}
		if(coolDownRunning) {
			timerCoolDown+=0.1f;
			if(timerCoolDown > coolDown) {
				coolDownRunning= false;
			}
		}
	}

	public void Impact() {
		savedTransform = transform;
		//savedTransform = savedTransform;
		if(!coolDownRunning) {
			//create the background
			NewBackground();

			//create the impact
			NewImpactParticles();

			//launch the coroutine of the impact
			StartCoroutine("ShonenImpact");
			timerCoolDown = 0;
			coolDownRunning = true;
		}
	}

	void NewBackground() {
		background = new GameObject();
		background.AddComponent<SpriteRenderer>();
		background.transform.parent = camTransform;
		background.transform.localPosition = new Vector3(0,0,0.2f);
		//add the sprite and the color
		background.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
		background.GetComponent<SpriteRenderer>().color = colorOfBackground;
		background.GetComponent<SpriteRenderer>().sortingOrder = 1000;
	}

	void NewImpactParticles() {
		impact = Instantiate (impactParticles, transform.position, Quaternion.identity) as GameObject;
		impact.transform.position = transform.position;
		//impact.transform.position = camTransform.GetComponent<Camera>().WorldToViewportPoint(impact.transform.position);

		//add the sprite and the color
		impact.GetComponentInChildren<ParticleSystem>().startColor = colorOfParticles;
	}

	IEnumerator ShonenImpact() {
		while (true) {
			yield return new WaitForSeconds(frameTime);
			timer +=frameTime;
			if(timer > frameTime) {
				Destroy(background);
				Destroy(impact);
				timer = 0;
				if(endFX != null) {
					instantiatedFX = Instantiate(endFX, savedTransform.position, Quaternion.identity) as GameObject;
					//instantiatedFX.GetComponentInChildren<ParticleSystem>().startColor = colorOfParticles;
				}
				StopCoroutine("ShonenImpact");
			}
		}
	}
}
