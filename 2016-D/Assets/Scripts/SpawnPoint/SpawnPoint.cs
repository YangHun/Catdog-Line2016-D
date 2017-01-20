using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoint : MonoBehaviour {

	public Vector3 Position = Vector3.zero;

	public enum Type { Flower, Enemy, Null };

	[SerializeField]
	private Type _spawn = Type.Null;
	public Type SpawnType{
		get { return _spawn; }
	}

	[SerializeField]
	private List<Flower.Type> _flowers = new List<Flower.Type>();
	public List<Flower.Type> SpawnFlowers{
		get { return _flowers; }
	}

	[SerializeField]
	private List<Vector3> _spawnpos = new List<Vector3>();
	public List<Vector3> SpawnPosition{
		get { return _spawnpos; }
	}

	[SerializeField]
	private List<bool> _collected = new List<bool>();
	public List<bool> IsFlowerCollected {
		get { return _collected; }
	}

	public void FlowerColleted (int index){
		_collected [index] = true;
	}

	void OnDrawGizmos(){
		Color c;

		if (_spawn == Type.Enemy) {

			c = Color.red;
			for (int i = 0; i < SpawnPosition.Count; i++) {
				Gizmos.DrawSphere (SpawnPosition[i], 1.0f);
			}
		} 
		else {
			for (int i = 0; i < SpawnPosition.Count; i++) {
				Flower.Type t = SpawnFlowers [i];
				switch (t) {
				case Flower.Type.Anemone:
					c = Color.red;
					break;
				case Flower.Type.Aster:
					c = Color.yellow;
					break;
				case Flower.Type.Baby_s:
					c = Color.green;
					break;
				case Flower.Type.Kalmia:
					c = Color.cyan;
					break;
				case Flower.Type.Poppy:
					c = Color.blue;
					break;
				case Flower.Type.Snowdrop:
					c = Color.gray;
					break;
				case Flower.Type.Valley:
					c = Color.magenta;
					break;
				case Flower.Type.White_egret:
					c = Color.white;
					break;
				}
				
				Gizmos.DrawSphere (SpawnPosition [i], 0.3f);
			}
		}
	}
		
	void Start () {
		
	}
	
	void Update () {
		
	}
}