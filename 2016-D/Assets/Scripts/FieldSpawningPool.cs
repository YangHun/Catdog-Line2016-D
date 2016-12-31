using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSpawningPool : MonoBehaviour {

    private FieldSpawningPool _pool;
    public FieldSpawningPool I
    {
        get { return _pool; }
    }

	// Use this for initialization
	void Start () {
		if (_pool != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _pool = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
