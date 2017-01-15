using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

	void OnTriggerStay2D (Collider2D col){

		if (col.gameObject.tag == "Player" ) {

			ParticleSystem p = gameObject.GetComponentInChildren<ParticleSystem>();

			if(p.isPlaying){
				TutorialManager.I.EndTutorialGame ();
				transform.DetachChildren ();
				Destroy (this.gameObject);
			}
		}
	}
}
