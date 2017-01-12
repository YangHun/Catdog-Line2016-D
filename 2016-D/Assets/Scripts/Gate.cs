using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D col){

		if (col.gameObject.tag == "Player") {
			TutorialManager.I.EndTutorialGame ();
			transform.DetachChildren ();
			Destroy (this.gameObject);
		}
	}
}
