using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSpawningPool : MonoBehaviour {

    private static FieldSpawningPool _pool;
    public static FieldSpawningPool I
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

	public void Init(){
	
		//TODO: Active Pollens

	}
}
