using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private const float PLAYTIME = 30.0f;

    //Singleton
    private static GameManager _manager = null;
    public static GameManager I
    {
        get
        {
            return _manager;
        }
    }

    private bool isFirstFrame = true;

    private int LocalPlayCnt = 0;
    private int LocalPollen = 0;
    private int[] LocalFlowers;

    public enum GameFlow { Load, Menu, Game, Save, Null };
    private GameFlow _flow_prev = GameFlow.Load;
    private GameFlow _flow_next = GameFlow.Null;

    public GameFlow CurrentState
    {
        get
        {
            return _flow_prev;
        }
    }

    private Dictionary<string, int> MenuButtonDict =
        new Dictionary<string, int>()
        {
            { "Start", 0 },
            { "Unlock", 1 },
            { "None", -1 }
        };

    private static PlayerData _data;
    public static PlayerData Data
    {
        get
        {
            return _data;
        }
    }

    void Start()
    {

        //Singleton Init
        if (_manager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _manager = this;
        }

        if (_data != null){
            return;
        }
        else
        {
            _data = new PlayerData();
        }

        LocalFlowers = new int[_data.FlowerIndex.Count];

    }
	
	void Update () {

		if (_flow_next != GameFlow.Null)
		{
			_flow_prev = _flow_next;
			Debug.Log ("Flow_Next : " + _flow_next);
			_flow_next = GameFlow.Null;
			isFirstFrame = true;
		}

        switch ( _flow_prev )
        {
            case GameFlow.Load:
                Debug.Log("Load!");
                FlowLoadState();
                //TODO: Load
                break;
            case GameFlow.Menu:
                Debug.Log("Menu!");
                FlowMenuState(_GetMenuButton);
                break;
            case GameFlow.Game:
                Debug.Log("Game!");
                FlowGameState();
                break;
            case GameFlow.Save:
                Debug.Log("Save!");
                FlowSaveState();
                break;
        }
        

    }

    void LateUpdate()
    {

		if (isFirstFrame) {
			isFirstFrame = !isFirstFrame;
		}
    }

    void FlowMenuState(int n)
    {
        if (isFirstFrame)
        {
            //Change UI
            if (LocalPlayCnt == 0)
            {
                UiManager.I.CanvasOff(UiManager.UICanvas.Game);
            }
            else
            {
                UiManager.I.CanvasOn(UiManager.UICanvas.Menu);
                UiManager.I.LocalFirstPlayMenu(false);
                UiManager.I.UpdateResultText();
            }

        }
            
        switch (n)
        {
            case -1: //Default
			Debug.Log("menu-default?");
				break;

            case 0: //Game Start
			Debug.Log("Menu-start game");
                //SceneManager.LoadScene(1);
                _flow_next = GameFlow.Game;
                break;
            case 1: //TODO:Unlock Stage UI
                Debug.Log("Unlock Stage");
                break;
        }
        
    }
  
    private int _GetMenuButton = -1;
    public void GetMenuButton(Button b)
    {
        _GetMenuButton = MenuButtonDict[b.name];

    }

    void FlowGameState()
    {
        if (isFirstFrame)
        {
            //Menu Click Init
            _GetMenuButton = -1;

            //Change UI
            if (LocalPlayCnt == 0)
                UiManager.I.ChangeCanvas(UiManager.UICanvas.Menu, UiManager.UICanvas.Game);
            else
                UiManager.I.CanvasOff(UiManager.UICanvas.Menu);

            resetTimer();
            FieldManager.I.Init();
            
            LocalPlayCnt++;
            LocalPollen = 0; //init

        }
        else
        {
            _timer -= Time.deltaTime;
        }

        UiManager.I.UpdateTimerText(Timer);

    }

    public void TrnsGameToSave() { //transaction
        _timer = 0.0f;
        UiManager.I.UpdateTimerText(Timer);
        _flow_next = GameFlow.Save;
    }

	[SerializeField]
    private float _timer = PLAYTIME;
    public float Timer
    {
        get { return _timer; }
    }

    public void resetTimer()
    {
        _timer = PLAYTIME;
    }

    void FlowSaveState()
    {
		if (isFirstFrame) {

			StartCoroutine("_save");
			LocalPlayCnt++;   
			TrnsSaveToMenu ();
		}
    }
		
    IEnumerator _save()
    {
        _data.Update(LocalPlayCnt, LocalPollen, LocalFlowers);

		yield return null;
		_data.Write ();
	}

	public void TrnsSaveToMenu() { //transaction
		_GetMenuButton = -1;
		_flow_next = GameFlow.Menu;
	}

    void FlowLoadState()
    {
        if (isFirstFrame)
        {
            StartCoroutine("_load");
        }
    }

    IEnumerator _load()
    {
        yield return _data.Read();

        LocalPlayCnt = 0; //init

        _flow_next = GameFlow.Menu;
    }
}
