using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour {

	[SerializeField]
	private float speed;


	private Vector3 StartVector = new Vector3(0.0f, -1.0f, 0.0f);


	// Use this for initialization
	void Start () {

		StartCoroutine ("FadeIn");
		//transform.LookAt (Vector3.zero);
		LookAtStartPoint();

	}
	
	// Update is called once per frame
	void Update () {

		Move ();

		if (isTriggerEnter ()) {
			StartCoroutine ("FadeOut");
		} 

		else if (isTriggerStay ()) {

			if (speed >= Time.deltaTime) {
				speed -= Time.deltaTime;
			}
		}

	}

	void Move(){
		Vector3 dir = Vector3.Normalize (transform.position);
		transform.position += (dir * speed * Time.deltaTime * (-1.0f));
	}


	void LookAtStartPoint(){

		Vector3 angle = Vector3.zero;
	
		angle.z = Vector3.Angle (StartVector, transform.position);

		if (transform.position.x < 0)
			angle.z *= (-1.0f);

		transform.rotation = Quaternion.Euler (angle);

//		Debug.Log (this.gameObject.name + " " + angle);

	}

	private static float RADIUS = 8.0f;

	private bool isTriggerEnter(){

		if (Vector3.Distance (transform.position, Vector3.zero) <= RADIUS + Time.deltaTime && 
			Vector3.Distance (transform.position, Vector3.zero) >= RADIUS - Time.deltaTime) {
			return true;
		}

		return false;
	}

	private bool isTriggerStay(){
		if (Vector3.Distance (transform.position, Vector3.zero) < RADIUS) {
			return true;
		} 

		return false;
	}
		
	[SerializeField]
	private const float FADETIME = 1.0f;

	IEnumerator FadeOut()
	{

		for (float i = FADETIME; i >= 0; i -= Time.deltaTime)
		{
			Renderer r = GetComponent<SpriteRenderer>();
			Color c = r.material.color;
			c.a = i / FADETIME;
			r.material.color = c;
			yield return null;
		}

		if (GetComponent<SpriteRenderer> ().material.color.a <= Time.deltaTime)
			Destroy(this.gameObject);
	}

	IEnumerator FadeIn()
	{

		if (GetComponent<SpriteRenderer> ().material.color.a >= Time.deltaTime)
			GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);

		for (float i = 0; i < FADETIME; i += Time.deltaTime)
		{
			Renderer r = GetComponent<SpriteRenderer>();
			Color c = r.material.color;
			c.a = i / FADETIME;
			r.material.color = c;
			yield return null;
		}

	}
}
