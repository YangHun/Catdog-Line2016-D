using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSpawningPool : MonoBehaviour {

    private static FieldSpawningPool _pool;
    public static FieldSpawningPool I
    {
        get { return _pool; }
    }

	private const int NUM = 6;
	private const float RESPAWNTIME = 1.5f;

	private SpawnPoint _point;

	[SerializeField]
	private Flower[] CurrentFlower = new Flower[5]; //max field size

	[SerializeField]
	private UnityEngine.Object Enemy;
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


	[SerializeField]
	private float _timer = 0.0f;
	private int cursor;

	public void Init(){
	
		//Init() is called when everytime Game Starts
		Transform m = FieldManager.I.Maze.transform;

		if (m != null) {
			//for enomy spawning
			cursor = 0;
			for (int i = 0; i < m.childCount; i++) {
				if (m.GetChild (i).gameObject.activeSelf == true)
					cursor = i;
			}
			_point = m.GetChild (cursor).GetComponentsInChildren<SpawnPoint> () [0];

			if (_point == null)
				Application.Quit ();

			//start spawning index
			int s = (Random.Range (1, NUM) + Random.Range (1, NUM) + Random.Range (1, NUM)) % NUM;
			cursor = s;

			//pollen spawn
		} 
	}

	public void Spawn(){
		if (_point.SpawnType == SpawnPoint.Type.Enemy) {
			_spawnEnemy ();
		} else
			Debug.Log (_point.SpawnType);
	}

	public void SpawnFlower(SpawnPoint sp, int index){
		if(sp.SpawnType == SpawnPoint.Type.Flower){
			_spawnFlower (sp, index);
		}
	}

	private void _spawnFlower(SpawnPoint _sp, int index){

		int num = 8;

		List<Flower.Type> l = new List<Flower.Type> ();

		for (int i = 0; i < num; i++) {
			if (GameManager.Data.FieldFlowerData [index, i] == false) {
				l.Add (_sp.SpawnFlowers [i]);
			}
			else {
				_sp.FlowerColleted (i);
			}
		}

		num = l.Count;
		Debug.Log ("num : " + num);

		if (num > 0) {
			//pick a random flower which player doesn't collect yet
			Flower.Type r = l [Random.Range (0, num - 1)];


			//flower spawn (once at start game)

			//TODO : 여러번 시작할때랑 저장되는 데이터?
			if ((CurrentFlower [index] == null)) {

				GameObject f = (GameObject)Instantiate (Flowers [FlowerDict [r]], _sp.SpawnPosition [FlowerDict [r]], Quaternion.identity);
				//	GameObject f = (GameObject)Instantiate (Flowers [0], _sp.SpawnPosition [0], Quaternion.identity);
				f.transform.SetParent (_sp.gameObject.transform);

				//UnityEngine.Object _p = Resources.Load ("Prefabs/Pollen/"+_fieldnum.ToString+"-"+ (FlowerDict [r]).ToString);
				GameObject _p = Resources.Load ("Prefabs/Pollens/1-1", typeof(GameObject)) as GameObject;
				GameObject p = (GameObject)Instantiate (_p, Vector3.zero, Quaternion.identity);
				p.transform.SetParent (f.gameObject.transform.parent);

				CurrentFlower [index] = f.GetComponent<Flower> ();
				CurrentFlower [index].type = r;
			}
		}
			
	}

	public void FlowerPick (Flower f){
		int index;

		for (index = 0; index < 5; index++) {
			if (f == CurrentFlower [index])
				break;
		}

		FieldManager.I.ObtainFlower (index, FlowerDict [f.type]);
	}

	private void _spawnEnemy(){

		if (_timer <= 0.0f) {

			for (int i = 0; i < _point.SpawnPosition.Count; i++) {
				if ((i % NUM) == (cursor % NUM)) {

					GameObject g = (GameObject) Instantiate (Enemy, _point.SpawnPosition [i], Quaternion.identity);
					Vector3 _pos = g.transform.position;
					_pos.z = -5.0f;
					g.transform.position = _pos;

					g.transform.SetParent (this.transform.FindChild ("Enemies"));
				}
			}

			cursor++;
			_timer = RESPAWNTIME;
		} 

		else {
			_timer -= Time.deltaTime;
		}
	}

	public void Kill(){
		_killEnemies();
	}

	private void _killEnemies(){

		Destroy (this.gameObject.transform.GetChild (0).gameObject);
		GameObject g = new GameObject ("Enemies");
		g.transform.SetParent (transform);
	}
}
