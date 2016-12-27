using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    Text blossom;
    [SerializeField]
    Text lifetime;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void SetBlossomUI (int t)
    {
        blossom.text = t.ToString();
    }
   
    public void SetLifeUI (float t)
    {
        lifetime.text = t.ToString("N2");
    }

}
