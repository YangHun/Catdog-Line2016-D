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
    
    public enum GameFlow { Load, Menu, Game, Save, Null };
    private GameFlow _flow_prev = GameFlow.Menu;
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

	void Start () {

        //Singleton Init
        if (_manager != null) {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _manager = this;
        }
        

    }
	
	void Update () {

        switch ( _flow_prev )
        {
            case GameFlow.Load:
                Debug.Log("Load!");
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

        if (isFirstFrame)
        {
            isFirstFrame = !isFirstFrame;
        }

        if (_flow_next != GameFlow.Null)
        {
            _flow_prev = _flow_next;
            _flow_next = GameFlow.Null;
            isFirstFrame = true;
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
            }

        }
            
        switch (n)
        {
            case -1: //Default
                break;

            case 0: //Game Start
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
        }
        else
        {
            Timer -= Time.deltaTime;
        }

        UiManager.I.UpdateTimerText(Timer);

        if (Timer <= 0) //Time Out!
        {
            Timer = 0.0f;
            UiManager.I.UpdateTimerText(Timer);
            _flow_next = GameFlow.Save;
        }
    }

    private float Timer = PLAYTIME;
    void resetTimer()
    {
        Timer = PLAYTIME;
    }

    void FlowSaveState()
    {
        if (isFirstFrame)
        {

            //TODO: Save

        }

        LocalPlayCnt++;
       
        
        _flow_next = GameFlow.Menu;

    }
    
}
