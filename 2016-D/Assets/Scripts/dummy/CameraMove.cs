using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    
    Transform player;

    [SerializeField]
    float delay;

    Vector3 pre;
    Vector3 next;

    float t;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        next = player.position;
        t = 0;
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = player.transform.position - new Vector3(0.0f, 0.0f, 10.0f);
        
       
	}

    void lerp()
    {

        pre = next;
        next = player.transform.position;


    }
}
