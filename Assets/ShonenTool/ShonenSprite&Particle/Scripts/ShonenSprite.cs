using UnityEngine;
using System.Collections;

public class ShonenSprite : MonoBehaviour {

	[Header("Shonen Sprite")]
	[Space(10)]
	[Header("Impact Sprite Parameters")]
	public int impactID;
	public Sprite[] impactSprites;
	public Color colorOfSprite;
	Transform spritePosition;
	[Space(10)]
	[Header("Background Color")]
	public Sprite backgroundSprite;
	public Color colorOfBackground;
	[Space(10)]
	[Header("Other Settings")]
	[Range(0.01f, 0.1f)]
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
	int spriteIndex;
	GameObject instantiatedFX;
	bool cooldDownRunning;
	float timerCoolDown;
	Transform savedTransform;

	void Start () {
		camTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
		spriteIndex = 0;
		timer = 0;
		cooldDownRunning = false;
	}

	void Update () {
		if(cooldDownRunning) {
			timerCoolDown+=0.1f;
			if(timerCoolDown > coolDown) {
				cooldDownRunning= false;
			}
		}
	}

	public void Impact() {
		savedTransform = transform;
		//savedTransform = savedTransform;
		if(!cooldDownRunning) {
			//create the background
			NewBackground();

			//create the impact
			NewImpactSprite();

			//launch the coroutine of the impact
			StartCoroutine("ShonenImpact");
		}
	}

	void NewBackground() {
		background = new GameObject();
		background.AddComponent<SpriteRenderer>();
		background.transform.parent = camTransform;
		background.transform.localPosition = new Vector3(0,0,2);
		//add the sprite and the color
		background.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
		background.GetComponent<SpriteRenderer>().color = colorOfBackground;
		background.GetComponent<SpriteRenderer>().sortingOrder = 1000;
	}

	void NewImpactSprite() {
		impact = new GameObject();
		impact.AddComponent<SpriteRenderer>();
		impact.transform.position = transform.position;
		//impact.transform.position = camTransform.GetComponent<Camera>().WorldToViewportPoint(impact.transform.position);

		//add the sprite and the color
		impact.GetComponent<SpriteRenderer>().sprite = impactSprites[0];
		impact.GetComponent<SpriteRenderer>().color = colorOfSprite;
		impact.GetComponent<SpriteRenderer>().sortingOrder = 2000;
	}

	IEnumerator ShonenImpact() {
		while (true) {
			yield return new WaitForSeconds(frameTime);
			timer +=frameTime;
			if(timer > frameTime*impactSprites.Length) {
				Destroy(background);
				Destroy(impact);
				timer = 0;
				spriteIndex = 0;
				if(endFX != null) {
					instantiatedFX = Instantiate(endFX, savedTransform.position, Quaternion.identity) as GameObject;
					instantiatedFX.GetComponentInChildren<ParticleSystem>().startColor = colorOfSprite;
				}
				cooldDownRunning = true;
				timerCoolDown = 0;
				StopCoroutine("ShonenImpact");
			}
			if(spriteIndex < impactSprites.Length) {
				impact.GetComponent<SpriteRenderer>().sprite = impactSprites[spriteIndex];
			}
			spriteIndex ++;
		}
	}
}
