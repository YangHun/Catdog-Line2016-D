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

    public enum GameFlow { Load, Menu, Story, Tutorial, Game, Save, Unlock, Null };
	private GameFlow _flow_prev;
	private GameFlow _flow_next;

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
			DontDestroyOnLoad (this.gameObject.transform.parent.gameObject);
        }

        if (_data != null){
            return;
        }
        else
        {
            _data = new PlayerData();
        }

        LocalFlowers = new int[_data.FlowerIndex.Count];

		_flow_prev = GameFlow.Tutorial;
        _flow_next = GameFlow.Null;

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
            break;
		case GameFlow.Story:
			Debug.Log ("Story!");
			FlowStoryState ();
			break;
		case GameFlow.Tutorial:
			Debug.Log ("Tutorial!");
			//TODO: Tutorial
			FlowTutorialState();
			break;

        case GameFlow.Menu:
            Debug.Log("Menu!");
            FlowMenuState(_GetMenuButton);
            break;
		case GameFlow.Unlock:
			Debug.Log ("Unlock!");

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

		
	StoryTeller _teller;

	void FlowStoryState(){

		if (isFirstFrame) {
			_teller = GameObject.Find ("Story UI").GetComponent<StoryTeller> ();
			UiManager.I.CanvasOff (UiManager.UICanvas.Game);
			UiManager.I.CanvasOff (UiManager.UICanvas.Menu);
			UiManager.I.CanvasOff (UiManager.UICanvas.Tutorial);
			UiManager.I.CanvasOff(UiManager.UICanvas.Unlock);
			UiManager.I.CanvasOn (UiManager.UICanvas.Story);
			_teller.StartStory ();

		}	


		if (_teller.isStoryEnd) {
			UiManager.I.CanvasOff (UiManager.UICanvas.Story);
            _data.StoryModeOff();
            _data.TutorialModeOn();
            _data.Write();
            _flow_next = GameFlow.Tutorial;
		}
	}

	void FlowTutorialState(){
		if (isFirstFrame) {
			UiManager.I.CanvasOff(UiManager.UICanvas.Menu);
			UiManager.I.CanvasOff(UiManager.UICanvas.Game);
			UiManager.I.CanvasOff(UiManager.UICanvas.Story);
			UiManager.I.CanvasOff(UiManager.UICanvas.Unlock);

			GameObject.Find ("Map").SetActive (false);
			GameObject.Find ("Tutorial").SetActive (true);

			TutorialManager.I.Init();
			UiManager.I.CanvasOn (UiManager.UICanvas.Tutorial);
		}

		//UiManager.I.CanvasOn (UiManager.UICanvas.Menu);
		//_flow_next = GameFlow.Menu;
	}	

	public void TrnsTutorialToMenu(){

		UiManager.I.SetHandle (Vector3.zero, Vector3.zero, UiManager.UICanvas.Tutorial);
		UiManager.I.ChangeCanvas (UiManager.UICanvas.Tutorial, UiManager.UICanvas.Menu);
		_flow_next = GameFlow.Menu;
		LocalPlayCnt = 0;
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
			Debug.Log ("Menu-start game");
                //SceneManager.LoadScene(1);
			_flow_next = GameFlow.Game;
			if (TutorialManager.I.gameObject.activeSelf) {
				TutorialManager.I.gameObject.SetActive (false);
				FieldManager.I.Maze.SetActive (true);
				Destroy (GameObject.Find ("Particle System"));
			}
            break;
        case 1: //TODO:Unlock Stage UI
			TrnsMenuToUnlock();
        	break;
        }
        
    }

    private int _GetMenuButton = -1;
    public void GetMenuButton(Button b)
    {
        _GetMenuButton = MenuButtonDict[b.name];

    }

	void TrnsMenuToUnlock(){
		UiManager.I.ChangeCanvas (UiManager.UICanvas.Menu, UiManager.UICanvas.Unlock);
		_flow_next = GameFlow.Unlock;
	}

	void FlowUnlockState() {
		if (isFirstFrame) {
		}
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

          //  resetTimer();
            FieldManager.I.Init();
            
            LocalPlayCnt++;
            LocalPollen = 0; //init

        }
        else
        {
			if (FieldManager.I.EndGame == true) {
				UiManager.I.SetHandle (Vector3.zero, Vector3.zero, UiManager.UICanvas.Game);
				_flow_next = GameFlow.Save;
				UiManager.I.UpdateResultValue (GameManager.Data.LocalPlln);
			}
        }
    }



    void FlowSaveState()
    {
		if (isFirstFrame) {

			StartCoroutine("_save");
			LocalPlayCnt++;   
			_GetMenuButton = -1;
			_flow_next = GameFlow.Menu;
		}
    }
		
    IEnumerator _save()
    {
        _data.Update(LocalPlayCnt, LocalPollen, LocalFlowers);

		yield return null;
		_data.Write ();
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

        if (_data.StoryMode)
        {
            if (_data.TutorialMode)
            {
                _flow_next = GameFlow.Tutorial;
            }

            else
            {
                _flow_next = GameFlow.Story;
            }

        }
        else 
        {
            _flow_next = GameFlow.Menu;
        }
    }
}
