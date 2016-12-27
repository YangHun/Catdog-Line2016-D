using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    UnityEngine.Object effect;

	// Use this for initialization
	void Start () {
		
	}
	
    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerInventory>().GainMoney();
            PlayEffect();

            Destroy(this.gameObject);
        }
    }

    void PlayEffect()
    {

        Debug.Log("Play Effect!");
        //GameObject obj = Instantiate(effect, transform.position, Quaternion.identity) as GameObject;
    
    }

}
