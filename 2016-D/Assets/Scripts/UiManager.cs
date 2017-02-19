using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    static Canvas Menu;
	static Canvas Game;
	static Canvas Story;
	static Canvas Tutorial;
	static Canvas Tutorial_Menu;
	static Canvas Tutorial_Catcher;
	static Canvas Catcher;
	static Canvas Note;
	static Canvas Brain;
	static Canvas Brain_Unlock;

	private string[] BrainFieldName = new string[4]{
		"Field_Name_1",
		"Field_Name_2",
		"Field_Name_3",
		"Field_Name_4"
	};

    private static UiManager _manager = null;
    public static UiManager I
    {
        get
        {
            return _manager;
        }
    }

	public enum UICanvas { Menu, Game, Story, Tutorial, Tutorial_Menu, Tutorial_Catcher, Catcher, Note, Brain, Brain_Unlock};
    private Dictionary<UICanvas, Canvas> CanvasDict;

    private float r;

    void Start()
    {

        //Singleton
        if (_manager != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _manager = this;
			DontDestroyOnLoad (this.gameObject);
        }

        //Canvas prefab init
        Menu = GameObject.Find("Menu UI").GetComponent<Canvas>();
        Game = GameObject.Find("Game UI").GetComponent<Canvas>();
		Story = GameObject.Find("Story UI").GetComponent<Canvas>();
		Tutorial = GameObject.Find("Tutorial UI").GetComponent<Canvas>();
		Tutorial_Menu = GameObject.Find("Tutorial_Menu UI").GetComponent<Canvas>();
		Tutorial_Catcher = GameObject.Find("Tutorial_Catcher UI").GetComponent<Canvas>();
		Catcher = GameObject.Find("Catcher UI").GetComponent<Canvas>();
		Note = GameObject.Find("Note UI").GetComponent<Canvas>();
		Brain = GameObject.Find("Brain UI").GetComponent<Canvas>();
		Brain_Unlock = GameObject.Find("Brain Unlock UI").GetComponent<Canvas>();

        CanvasDict = new Dictionary<UICanvas, Canvas>()
        {
            { UICanvas.Menu, Menu },
            { UICanvas.Game, Game },
			{ UICanvas.Story, Story },
			{ UICanvas.Catcher, Catcher },
			{ UICanvas.Tutorial, Tutorial },
			{ UICanvas.Tutorial_Menu, Tutorial_Menu },
			{ UICanvas.Tutorial_Catcher, Tutorial_Catcher },
			{ UICanvas.Note, Note },
			{ UICanvas.Brain, Brain },
			{ UICanvas.Brain_Unlock, Brain_Unlock }
        };


        r = Game.transform.FindChild("Handle").FindChild("Point").GetComponent<Image>().sprite.texture.width/2.0f;
	}

	public void ButtonMenuEvent (Button b)
    {
        GameManager.I.GetMenuButton(b);
    }

	public void ButtonTCatcherEvent ()
	{
		GameManager.I.TrnsTMenuToTCatcher ();
	}

	public void ButtonCatcherEvent ()
	{
		GameManager.I.TrnsMenuToCatcher ();

	}

	public void ButtonCatcherPurchaseEvent (Image b)
	{
		ShopFlower f = b.gameObject.GetComponent<ShopFlower> ();
		if (GameManager.Data != null && f != null) {

			if (f.Price <= GameManager.Data.TotalPlln) {

				Debug.Log((-1) * f.Price + " pollen");
				GameManager.Data.Update ((-1) * f.Price, PlayerData.UpdateType.Pollen);
				GameManager.Data.MakeFlower (f.Index);
				UiManager.I.UpdateCatcherPollenText (GameManager.Data.TotalPlln);
				GameManager.Data.Write();
				Debug.Log ("Purchase");

			} else {
				Debug.Log ("Not Enough Money");
			}
		} 
		else {
			UpdateCatcherPollenText (0);
		}

	}

	public void ButtonNoteEvent ()
	{
		GameManager.I.TrnsMenuToNote ();
	}

	public void ButtonBrainEvent ()
	{
		GameManager.I.TrnsMenuToBrain();
		UpdateBrainInactivedButton ();


	}

	int currentUnlockIndex = -1;

	public void ButtonBrainFieldEvent(Button b){
		CanvasOn (UICanvas.Brain_Unlock);
		//TODO:Change Unlock UI number
		int index = int.Parse(b.name) - 1;

		Brain_Unlock.transform.FindChild ("Name").GetComponent<Text> ().text = BrainFieldName [index];

		currentUnlockIndex = index;

        UpdateBrainUnlockFlowerText();

    }

	public void ButtonBrainUnlockYesEvent(){

		int[] f = new int[8];

		for (int i = 0; i < GameManager.Data.ShopFlower.Length; i++) {
			f[i] = GameManager.Data.ShopFlower[i] - GameManager.Data.UsedFlower[i];
		}

		int index = currentUnlockIndex;

		bool cannotbuyit = false;

		for (int i = 0; i < f.Length; i++) {
			if (f [i] < GameManager.Data.Brain [index,i]) {
				cannotbuyit = true;
				break;
			}
		}

		if (!cannotbuyit) {
			for (int i = 0; i < f.Length; i++) {
				GameManager.Data.UseFlower (i, GameManager.Data.Brain [index, i]);
			}
			GameManager.Data.RecoverBrain (currentUnlockIndex);

			GameManager.Data.Write ();

			currentUnlockIndex = -1;
			UpdateBrainFlowerText ();

			CanvasOff (UICanvas.Brain_Unlock);
			UpdateBrainInactivedButton ();
		}
		else {
			Debug.Log ("Not Enough Flower");
			currentUnlockIndex = - 1;
			CanvasOff (UICanvas.Brain_Unlock);
		}

	}

	public void ButtonBrainUnlockNoEvent(){
		CanvasOff (UICanvas.Brain_Unlock);
	}

	public void ButtonSettingEvent ()
	{

	}
		
	public void ButtonHomeEvent(){
		GameManager.I.TrnsAnyToMenu ();
	}

	public void ChangeTutorialText(){
		Tutorial.transform.FindChild("Catcher's").GetComponentInChildren<Text>().text = "들어와!";
	}

    public void ChangeCanvas (UICanvas pre, UICanvas next)
    {
        Canvas _pre = CanvasDict[pre];
        Canvas _next = CanvasDict[next];

        _pre.gameObject.SetActive(false);
        _next.gameObject.SetActive(true);

        Debug.Log(_pre);
        Debug.Log(_next);
    }

    public void CanvasOn (UICanvas cvs)
    {
        Canvas _canvas = CanvasDict[cvs];
        _canvas.gameObject.SetActive(true);
    }

    public void CanvasOff(UICanvas cvs)
    {
        Canvas _canvas = CanvasDict[cvs];
        _canvas.gameObject.SetActive(false);
    }

	public void ChangeStorySentence(string t, string p){

		Story.transform.FindChild (p).GetComponent<Text> ().text = t;	
	}


    public void UpdateTimerText(float t)
    {
        Game.transform.FindChild("Time").GetComponent<Text>().text = t.ToString("N2");
    }

    public void UpdatePollenText(int t)
    {
        Game.transform.FindChild("Pollen").GetComponent<Text>().text = t.ToString();
    }

    public void UpdateTPollenText(int t)
    {
        Tutorial.transform.FindChild("Pollen").GetComponent<Text>().text = t.ToString();
    }

    public void UpdateCatcherPollenText(int t)
	{
		Catcher.transform.FindChild("Pollen").GetComponent<Text>().text = t.ToString();
	}

	public void UpdateNotePollenText(int t)
	{
		Note.transform.FindChild("Pollen").GetComponent<Text>().text = t.ToString();
	}

	public void UpdateNoteAddTimeText(float t){

		string s = "Time 30 + " + t.ToString("N1") + " sec.";
		Note.transform.FindChild("Time").GetComponent<Text>().text = s;
	}

	public void UpdateNoteFlowerText(){
		Transform t = Note.transform.FindChild ("Scroll View").GetChild (0).GetChild (0); //Content
		Transform s = Note.transform.FindChild ("Sumary"); //Content
		for (int i = 0; i < t.childCount; i++) {
			t.GetChild (i).FindChild ("Num").GetComponent<Text> ().text = GameManager.Data.ShopFlower [i].ToString();
			s.GetChild (i).FindChild ("Num").GetComponent<Text> ().text = GameManager.Data.ShopFlower [i].ToString();
		}
	}

	private void UpdateBrainInactivedButton(){
		for (int i = 0; i < GameManager.Data.RecoveredField.Length; i++) {

			if (GameManager.Data.RecoveredField [i]) {
				Brain.transform.FindChild ("Brain").FindChild ((i+1).ToString ()).GetComponent<Button> ().interactable = false;
			}
		}

	}

	public void UpdateBrainFlowerText(){
		Transform t = Brain.transform.FindChild ("Sumary");
		for (int i = 0; i < t.childCount; i++) {
			int value = GameManager.Data.ShopFlower [i] - GameManager.Data.UsedFlower[i];
			t.GetChild (i).FindChild ("Num").GetComponent<Text> ().text = value.ToString();
		}

		int p = 0;

		for (int i = 0; i < GameManager.Data.RecoveredField.Length; i++) {
			if (GameManager.Data.RecoveredField [i])
				p++;
		}

		int percentage = (int)(p * 100.0 / GameManager.Data.RecoveredField.Length);
		Brain.transform.FindChild ("recovered").GetComponent<Text> ().text = percentage.ToString () + "%";
	}

    public void UpdateBrainUnlockFlowerText()
    {
        Transform t = Brain_Unlock.transform.FindChild("Sumary");
        for (int i = 0; i < t.childCount; i++)
        {
            int v = GameManager.Data.Brain[currentUnlockIndex, i];
            t.GetChild(i).FindChild("Num").GetComponent<Text>().text = v.ToString();
        }

    }

	public void UpdateMenuBackground(){

		Menu.transform.FindChild ("Image").gameObject.SetActive (true);
	}

    private int _result;

    public void UpdateResultValue(int t)
    {
        _result = t;
    }

    public void UpdateResultText()
    {
        if (_result != 0)
        {
            string _text = "+" + _result.ToString();
            Menu.transform.FindChild("Result").GetComponent<Text>().text = _text;
        }
        else
        {
            Menu.transform.FindChild("Result").GetComponent<Text>().text = "";

        }
    }

    public void LocalFirstPlayMenu(bool b)
    {
        Menu.transform.FindChild("Result").gameObject.SetActive(!b);
    }

    private Vector3 _point = Vector3.zero;
    private Vector3 _dir = Vector3.zero;

	public void SetHandle (Vector3 point, Vector3 dir, UICanvas cvs)
    {
        _point = point;
        _dir = dir;
		_SetHandle (cvs);
    }

	private void _SetHandle(UICanvas cvs)
    {
		Transform __handle = null;
		if(cvs == UICanvas.Game)
        	__handle = Game.transform.FindChild("Handle");
		else if (cvs == UICanvas.Tutorial)
			__handle = Tutorial.transform.FindChild("Handle");			

        if (_point != Vector3.zero)
        {
				__handle.gameObject.SetActive(true);


            RectTransform __point = __handle.FindChild("Point").GetComponent<RectTransform>();
            RectTransform __dir = __handle.FindChild("Dir").GetComponent<RectTransform>();

            __point.position = _point;
            __dir.position = (__point.position + _dir * r);
        }
        else
        {
            __handle.gameObject.SetActive(false);
        }
    }


    void Update()
	{
    }
}
