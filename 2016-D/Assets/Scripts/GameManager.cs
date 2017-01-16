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

    public enum GameFlow { Load, Menu, Story, Tutorial, Tutorial_Menu, Tutorial_Catcher, Game, Save, Catcher, Null };
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
            { "Catcher", 1 },
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

		_flow_prev = GameFlow.Load;
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
		case GameFlow.Tutorial_Menu:
			Debug.Log ("Tutorial_Menu!");
			//TODO: Tutorial
			FlowTutorialMenuState();
			break;
		case GameFlow.Tutorial_Catcher:
			Debug.Log ("Tutorial_Catcher!");
			//TODO: Tutorial
			FlowTutorialCatcherState();
			break;

        case GameFlow.Menu:
            Debug.Log("Menu!");
            FlowMenuState(_GetMenuButton);
            break;
		case GameFlow.Catcher:
			Debug.Log ("Catcher!");

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
			UiManager.I.CanvasOff(UiManager.UICanvas.Catcher);
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
			UiManager.I.CanvasOff(UiManager.UICanvas.Catcher);
			UiManager.I.CanvasOff(UiManager.UICanvas.Tutorial_Menu);
			UiManager.I.CanvasOff(UiManager.UICanvas.Tutorial_Catcher);

			GameObject.Find ("Map").SetActive (false);
			GameObject.Find ("Tutorial").SetActive (true);

			TutorialManager.I.Init();
			UiManager.I.CanvasOn (UiManager.UICanvas.Tutorial);
		}

		//UiManager.I.CanvasOn (UiManager.UICanvas.Menu);
		//_flow_next = GameFlow.Menu;
	}	

	public void TrnsTutorialToTMenu(){

		UiManager.I.SetHandle (Vector3.zero, Vector3.zero, UiManager.UICanvas.Tutorial);
		UiManager.I.ChangeCanvas (UiManager.UICanvas.Tutorial, UiManager.UICanvas.Tutorial_Menu);
		_flow_next = GameFlow.Tutorial_Menu;
	}


	void FlowTutorialMenuState(){
		if (isFirstFrame) {
			UiManager.I.CanvasOff(UiManager.UICanvas.Menu);
			UiManager.I.CanvasOff(UiManager.UICanvas.Game);
			UiManager.I.CanvasOff(UiManager.UICanvas.Story);
			UiManager.I.CanvasOff(UiManager.UICanvas.Catcher);
			UiManager.I.CanvasOff (UiManager.UICanvas.Tutorial);
			UiManager.I.CanvasOff (UiManager.UICanvas.Tutorial_Catcher);

			UiManager.I.CanvasOn (UiManager.UICanvas.Tutorial_Menu);
		}
	}

	public void TrnsTMenuToTCatcher(){

		UiManager.I.ChangeCanvas (UiManager.UICanvas.Tutorial_Menu, UiManager.UICanvas.Tutorial_Catcher);
		_flow_next = GameFlow.Tutorial_Catcher;

	}

	public void TrnsAnyToMenu(){

		UiManager.I.CanvasOff(UiManager.UICanvas.Tutorial_Catcher);
		UiManager.I.CanvasOff(UiManager.UICanvas.Tutorial_Menu);
		UiManager.I.CanvasOff(UiManager.UICanvas.Catcher);

		UiManager.I.CanvasOn (UiManager.UICanvas.Menu);

		_flow_next = GameFlow.Menu;

	}

	void FlowTutorialCatcherState(){
		if (isFirstFrame) {
			UiManager.I.CanvasOff(UiManager.UICanvas.Menu);
			UiManager.I.CanvasOff(UiManager.UICanvas.Game);
			UiManager.I.CanvasOff(UiManager.UICanvas.Story);
			UiManager.I.CanvasOff(UiManager.UICanvas.Catcher);
			UiManager.I.CanvasOff (UiManager.UICanvas.Tutorial);
			UiManager.I.CanvasOn (UiManager.UICanvas.Tutorial_Menu);

			UiManager.I.CanvasOn (UiManager.UICanvas.Tutorial_Catcher);
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
			Debug.Log ("Menu-start game");
                //SceneManager.LoadScene(1);
			_flow_next = GameFlow.Game;
			if (TutorialManager.I.gameObject.activeSelf) {
				TutorialManager.I.gameObject.SetActive (false);
				FieldManager.I.Maze.SetActive (true);
				Destroy (GameObject.Find ("Particle System"));
			}
            break;
        case 1: //TODO:Catcher Stage UI
			//TrnsMenuToCatcher();
        	break;
        }
        
    }

    private int _GetMenuButton = -1;
    public void GetMenuButton(Button b)
    {
        _GetMenuButton = MenuButtonDict[b.name];

    }

	public void TrnsMenuToCatcher(){
		UiManager.I.ChangeCanvas (UiManager.UICanvas.Menu, UiManager.UICanvas.Catcher);
		_flow_next = GameFlow.Catcher;
	}

	void FlowCatcherState() {
		if (isFirstFrame) {
			UiManager.I.UpdateCatcherPollenText (_data.TotalPlln);
		}



	}	

	public void EventCatcherPurchase(int value){

		//TODO: purchase

		if (_data != null) {

			if (value <= _data.TotalPlln) {

				_data.Update ((-1) * value, PlayerData.UpdateType.LocalPlln);
				StartCoroutine ("_save");
				UiManager.I.UpdateCatcherPollenText (_data.TotalPlln);
				Debug.Log ("Purchase");

			} else {
				Debug.Log ("Not Enough Money");
			}
		} 
		else {
			UiManager.I.UpdateCatcherPollenText (0);
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
				UiManager.I.UpdateResultValue (LocalPollen);
			}
        }
    }



    void FlowSaveState()
    {
		if (isFirstFrame) {

			_data.Update (1, PlayerData.UpdateType.LocalPCount);
			StartCoroutine("_save");
			_GetMenuButton = -1;
			_flow_next = GameFlow.Menu;
		}
    }
		
    IEnumerator _save()
    {
       _data.Update(_data.LocalPCount, _data.LocalPlln, LocalFlowers);

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
