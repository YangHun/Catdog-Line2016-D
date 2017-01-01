using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fileIO : MonoBehaviour {

    PlayerData data;


	// Use this for initialization
	void Start () {

        data = new PlayerData();
        data.Write();
        
        data.Read();
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
