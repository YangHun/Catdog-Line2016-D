using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    static Canvas Menu;
	static Canvas Game;
	static Canvas Story;

    private static UiManager _manager = null;
    public static UiManager I
    {
        get
        {
            return _manager;
        }
    }

    public enum UICanvas { Menu, Game, Story };
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

        CanvasDict = new Dictionary<UICanvas, Canvas>()
        {
            { UICanvas.Menu, Menu },
            { UICanvas.Game, Game },
			{ UICanvas.Story, Story }
        };


        r = Game.transform.FindChild("Handle").FindChild("Point").GetComponent<Image>().sprite.texture.width/2.0f;
	}

    public void MenuButtonEvent (Button b)
    {
        GameManager.I.GetMenuButton(b);
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

    private int _result;

    public void UpdateResultValue(int t)
    {
        _result = t;
    }

    public void UpdateResultText()
    {
        string _text = "+" + _result.ToString();
        Menu.transform.FindChild("Result").GetComponent<Text>().text = _text;
    }

    public void LocalFirstPlayMenu(bool b)
    {
        Menu.transform.FindChild("Result").gameObject.SetActive(!b);
    }

    private Vector3 _point = Vector3.zero;
    private Vector3 _dir = Vector3.zero;

    public void SetHandle (Vector3 point, Vector3 dir)
    {
        _point = point;
        _dir = dir;
    }

    private void _SetHandle()
    {
        Transform __handle = Game.transform.FindChild("Handle");

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

        _SetHandle();

    }
}
