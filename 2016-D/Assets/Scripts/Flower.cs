using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

	private const float FADETIME = 0.7f;

	public enum Type {
		Poppy, //양귀비
		Aster, //아스터
		White_egret, //해오라비난초
		Snowdrop, //스노우드롭
		Anemone, //아네모네
		Baby_s, //안개꽃
		Valley, //은방울꽃
		Kalmia  //칼미아
	}

	public Type type;
		
	private bool startfade = false;

	private void OnTriggerEnter2D(Collider2D col)
	{

		if (col.gameObject.tag == "Player" && !startfade) {
			StartCoroutine ("FadeOut");
			startfade = true;
		}
	}

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

		if (GetComponent<SpriteRenderer> ().material.color.a <= Time.deltaTime * 2f)
			Destroy(this.gameObject);
	}

}
