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
    private GameObject _maze;

    private enum GameEnd { TimeOver, KickedOut, DreamOut, Null }
    private GameEnd EndType = GameEnd.Null;

	private Player _player;

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
        
		// Map Generate
		if (_maze.tag != "Field")
			return;
		
		bool[] arr = GameManager.Data.FieldsData;


		for (int i = 0; i < arr.Length; i++) {
			if (arr [i]) {
				_maze.transform.FindChild ("field" + i).gameObject.SetActive (true);			
			}
		}

		// Player transform.position & local data
		if ( _player == null )
			_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();

		_player.transform.position = Vector3.zero;
		_player.transform.rotation = Quaternion.Euler (Vector3.zero);

		GameManager.Data.Reset (PlayerData.ResetType.All);
		UiManager.I.UpdatePollenText (GameManager.Data.LocalPlln);

		// Pollens Respawn



		// Flowers Random Spawn
		Field[] _fields = _maze.gameObject.GetComponentsInChildren<Field>(false);

		Debug.Log (_fields.Length);

		for (int i = 0; i < _fields.Length; i++) {
			if (_fields [i] == true) {
				_fields [i].GenerateFlower ();
			}
		}


		// Enomies Respawn


    }

    void Update () {

        //TODO : game end conditions

        if (isGameEnd())
		{
			GameManager.I.TrnsGameToSave ();
			UiManager.I.UpdateResultValue (GameManager.Data.LocalPlln);
			GameManager.I.resetTimer ();
        }
           

	}
    
    public void ObtainPollen(int value)
    {
        Debug.Log(value);
		GameManager.Data.Update(value, PlayerData.UpdateType.LocalPlln);
		Debug.Log(GameManager.Data.LocalPlln);
		UiManager.I.UpdatePollenText(GameManager.Data.LocalPlln);
    }

    public void ObtainFlower(int index, int value)
    {
		GameManager.Data.Update(index, value);
    }

    private bool isGameEnd()
    {
        // time over : GameManager.PLAYTIME
        // kicked out : collision of the enomy
        // dream out : penalty X

        if (GameManager.I.Timer <= 0)
        {
            EndType = GameEnd.TimeOver;
            
            return true;
        }
        else if (EndType == GameEnd.DreamOut)
        {
            
            return true;
        }
        else if (EndType == GameEnd.KickedOut)
        {
            
            return true;
        }

        return false;          

    }
}
