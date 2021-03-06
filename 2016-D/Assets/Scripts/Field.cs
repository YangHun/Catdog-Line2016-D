﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {

	[SerializeField]
	private int _fieldnum;

	[SerializeField]
	private SpawnPoint _sp;
	public SpawnPoint spawnPoint {
		get { return _sp; }
	}

	[SerializeField]
	private UnityEngine.Object[] Flowers;

	private Dictionary <Flower.Type, int> FlowerDict = new Dictionary<Flower.Type, int>() {
		
		{ Flower.Type.Poppy, 0 }, //양귀비
		{ Flower.Type.Aster, 1 }, //아스터
		{ Flower.Type.White_egret, 2 }, //해오라비난초
		{ Flower.Type.Snowdrop, 3 }, //스노우드롭
		{ Flower.Type.Anemone, 4 }, //아네모네
		{ Flower.Type.Baby_s, 5 }, //안개꽃
		{ Flower.Type.Valley, 6 }, //은방울꽃
		{ Flower.Type.Kalmia, 7 },  //칼미아
	};

	private Flower CurrentFlower;
	private GameObject CurrentPollens;

	// Use this for initialization
	void Start () {

		if (Flowers == null)
			return;

		if (_sp == null)
			_sp = GetComponentInChildren<SpawnPoint> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateFlower(){

		if (CurrentFlower != null) {
			flowerpick (CurrentFlower);
		}
		_generate ();
	}

	private void _generate(){

		int num = _sp.IsFlowerCollected.Count;

		List<Flower.Type> l = new List<Flower.Type> ();

		for (int i = 0; i < num; i++) {
			if (_sp.IsFlowerCollected[i] == false) {
				l.Add (_sp.SpawnFlowers [i]);
			}
		}

		num = l.Count;

		 //pick a random flower which player doesn't collect yet
		Flower.Type r = l[Random.Range (0, num-1)];

//		GameObject f= (GameObject)Instantiate (Flowers [FlowerDict [r]], _sp.SpawnPosition [FlowerDict [r] ], Quaternion.identity);
		GameObject f= (GameObject)Instantiate (Flowers [0], _sp.SpawnPosition [0], Quaternion.identity);
		f.transform.SetParent (this.gameObject.transform.FindChild("SpawnPoint"));

		CurrentFlower = f.GetComponent<Flower>();
		CurrentFlower.type = r;

		//UnityEngine.Object _p = Resources.Load ("Prefabs/Pollen/"+_fieldnum.ToString+"-"+ (FlowerDict [r]).ToString);
		GameObject _p = Resources.Load ("Prefabs/Pollens/1-1", typeof(GameObject)) as GameObject;
		GameObject p = (GameObject)Instantiate (_p, Vector3.zero, Quaternion.identity);
		p.transform.SetParent (f.transform);

	}

	private void flowerpick(Flower f){ //Destroy existing object

		if (f != null) {

			if (f.transform.childCount > 0) {
				f.gameObject.transform.DetachChildren(); //remain pollens
			}
			Destroy (f.gameObject);

		}
	}
}
