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
    private UnityEngine.Object map;
    public GameObject _maze;

    private enum GameEnd { TimeOver, KickedOut, DreamOut, Null }
    private GameEnd EndType = GameEnd.Null;

    private PlayerData _localData;
        
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

       

        if (_localData == null)
            _localData = new PlayerData();

    }

    public void Init() //Call When Game Starts
    {
        //TODO: Init field (for after 1st play)
        // Map Generate

        if (map == null)
            return;

        if (_maze != null)
            return;
        else
            _maze = Instantiate(map, Vector3.zero, Quaternion.identity) as GameObject;


        // Player transform.position
        // Pollens Respawn
        // Enomys Respawn



    }

    // Update is called once per frame
    void Update () {

        //TODO : game end conditions

        if (isGameEnd())
        {
            GameManager.I.TrnsGameToSave();
        }
           

	}
    
    public void ObtainPollen(int value)
    {
        Debug.Log(_localData);
        _localData.Update(value, PlayerData.UpdateType.Pollen);
        Debug.Log(_localData.LocalPlln);
        UiManager.I.UpdatePollenText(_localData.LocalPlln);
    }

    public void ObtainFlower(int index, int value)
    {
        _localData.Update(index, value);
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
