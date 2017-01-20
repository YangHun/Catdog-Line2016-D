using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour {

    private static FieldManager _manager;
    public static FieldManager I
    {
        get
        {
            return _manager;
        }
    }

    [SerializeField]
    private GameObject _maze = null;
	public GameObject Maze {
		get { return _maze; }
	}

	private bool _gamestart = false;
	private bool _gameend = false;
	public bool EndGame {
		get { return _gameend; }
	}

    private enum GameEnd { TimeOver, KickedOut, DreamOut, Null }
    private GameEnd EndType = GameEnd.Null;

	private Player _player;

	[SerializeField]
	private bool[] _spawnfield;

	void Start () {

        if( _manager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _manager = this;
        }
    }

    public void Init() //Call When Game Starts
	{
		//TODO: Init field (for after 1st play)
        
		// Player transform.position & local data
		if ( _player == null )
			_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();

		_player.transform.position = Vector3.zero;
		Camera.main.transform.position = new Vector3 (0.0f, 0.0f, -10.0f);
		_player.transform.rotation = Quaternion.Euler (Vector3.zero);

		// Map Generate
		if (_maze.tag != "Map")
			return;
		

		_spawnfield = GameManager.Data.FieldData;

		for (int i = 0; i < _spawnfield.Length; i++) {
			if (_spawnfield [i]) {
				_maze.transform.FindChild ("field" + i).gameObject.SetActive (true);			
			}
		}


		GameManager.Data.Reset (PlayerData.ResetType.All);
		UiManager.I.UpdatePollenText (GameManager.Data.LocalPlln);



        // Flowers Spawn & Pollens Spawn
        
        Field[] _fields = _maze.gameObject.GetComponentsInChildren<Field>(false);

		for (int i = 0; i < _fields.Length; i++) {
			if (_fields [i] == true) {

				FieldSpawningPool.I.SpawnFlower (_fields [i].spawnPoint, i);

			}
		}

        //TODO: Do sth After Player obtains Flower


        // Enemies respawn
        FieldSpawningPool.I.Init();



		//Game start!
		_gameend = false;
		EndType = GameEnd.Null;
		_gamestart = true;

    }

    void Update () {

        //TODO : game end conditions

        if (isGameEnd())
		{
			_gamestart = false;
			_gameend = true;

			FieldSpawningPool.I.Kill ();
			StopTimer ();
			_player.EndGame();
        }

		if (_gamestart) {

			
			FieldSpawningPool.I.Spawn ();
			ActiveTimer();
		}
           

	}
    
	private const float PLAYTIME = 30.0f;

	[SerializeField]
	private float _timer = PLAYTIME;
	public float Timer
	{
		get { return _timer; }
	}
		
	void ActiveTimer(){
		_timer -= Time.deltaTime;
		UiManager.I.UpdateTimerText(Timer);
	}

	void StopTimer(){

		if (_timer < 0.0f)
			_timer = 0.0f;
		UiManager.I.UpdateTimerText(Timer);

		_resetTimer ();
	}

	void _resetTimer()
	{
		_timer = PLAYTIME;
	}


    public void ObtainPollen(int value)
    {
        Debug.Log(value);
		GameManager.Data.Update(value, PlayerData.UpdateType.LocalPlln);
		Debug.Log(GameManager.Data.LocalPlln);
		Debug.Log(GameManager.Data.TotalPlln);
		UiManager.I.UpdatePollenText(GameManager.Data.LocalPlln);
    }

    public void ObtainFlower(int index, int value)
    {
		GameManager.Data.ObtainFlower(index, value);
		GameManager.Data.Write ();
    }

    private bool isGameEnd()
    {
        // time over : GameManager.PLAYTIME
        // kicked out : collision of the enomy
        // dream out : penalty X

        if (_timer <= 0.0f)
        {
            EndType = GameEnd.TimeOver;
            
            return true;
        }
        else if (EndType == GameEnd.DreamOut)
        {
      		
            return true;
        }
		else if (_player != null)
        {
			if (_player.isPlayerKilled == true) {
				EndType = GameEnd.KickedOut;
				return true;
			} else
				return false;
        }

        return false;          

    }
}
