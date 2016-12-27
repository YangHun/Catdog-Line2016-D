using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

    PointManager manager;

  	// Use this for initialization
	void Start () {

        manager = transform.GetComponentInParent<PointManager>();

    }

    void OnTriggerEnter2D (Collider2D col)
    {

        Debug.Log("Enter!");

    }

    void OnTriggerStay2D (Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            manager.ActiveTimer();
        }
    }

}
