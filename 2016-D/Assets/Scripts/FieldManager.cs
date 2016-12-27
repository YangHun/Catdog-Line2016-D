using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour {

    [SerializeField]
    private UnityEngine.Object Maze;
        public GameObject _maze;

	// Use this for initialization
	void Start () {

        if (Maze == null)
            return;

        if (_maze != null)
            return;
        else
            _maze = Instantiate(Maze, Vector3.zero, Quaternion.identity) as GameObject;

    }
	
	// Update is called once per frame
	void Update () {
		
	}


}
