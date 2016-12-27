using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    private int money;
    public float lifeTime;

    UIManager ui;

	// Use this for initialization
	void Start () {
        lifeTime = 30.0f;
        ui = GameObject.Find("UI Manager").GetComponent<UIManager>();
	}
	
	// Update is called once per frame
	void Update () {

        CheckTimeOut();
	}

    void CheckTimeOut()
    {
        if (lifeTime > 0)
            lifeTime -= Time.deltaTime;
        ui.SetLifeUI(lifeTime);
    }
    
    public void GainMoney()
    {
        money++;
        ui.SetBlossomUI(money);
    }
}
