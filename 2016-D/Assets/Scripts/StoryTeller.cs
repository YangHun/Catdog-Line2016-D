using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;


public class StoryTeller : MonoBehaviour {

	private string _filename = "Dialogue";

	XmlNodeList nodes;
	XmlNodeList sentences;

	void Start () {
		Xme_Load (_filename);
	}

	void Update () {

		if (_start) {

			StoryTelling ();

			ActiveTimer ();
		}
	}
	void LateUpdate(){
		if (next_s != -2) {
		
			prev_s = next_s;
			next_s = -2;
			_timer += SPEAKTIME;
		}
	}

	[SerializeField]
	private float _timer = 0;
	[SerializeField]
	private float _wait = WAITTIME;
	private const float SPEAKTIME = 5.0f;
	private const float WAITTIME = 10.0f;

	[SerializeField]
	int cursor = -1;

	private string n;
	private string _speaker;
	private string _text;

	[SerializeField]
	private int prev_s = -2;
	[SerializeField]
	private int next_s = -2;
	private bool _start = false;

	public void StartStory(){
		_start = true;
	}
	private void StopStory(){
		_start = false;
	}
	private void EndStory(){
		_start = false;
		_timer = 0;
	}

    public bool EndGame = false;

	public bool isStoryEnd{
		get { return !_start; }
	}

	public void StoryTelling(){

		switch (prev_s) {

		case -1:
			if (_timer <= 0) {
				Debug.Log ("start!");
				cursor = 0;
				_timer = 0;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 0;
			}
			break;

		case 0:
			
			if (_timer <= 0) {
				cursor = 1;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 1;
			}
			break;
		case 1:
			if (_timer <= 0) {
				cursor = 2;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 2;
			}
			break;
		case 2: 
			if (_timer <= 0) {
				cursor = 3;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				_timer = SPEAKTIME;
				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 3;
			}
			break;
		case 3: //Question
			if (Input.GetMouseButtonDown (0)) {
				cursor = 4;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;
				_timer = 0;
				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 4;
			}

			else if (_timer <= 0) {
				cursor = 7;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 7;
			}
			break;
		
		case 4:
			if (_timer <= 0) {
				cursor = 5;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 5;
			}
			break;
		case 5:
			if (_timer <= 0) {
				cursor = 6;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 6;
			}
			break;

		case 6: //Speak_End
			if (_timer <= 0) {
				EndStory ();
            }
			break;

		case 7:
			if (_timer <= 0) {
				cursor = 8;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 8;
			}
			break;
		case 8:
			if (_timer <= 0) {
				cursor = 9;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;
				_timer = SPEAKTIME;
				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 9;
			}
			break;

		case 9: //Question
			if (Input.GetMouseButtonDown (0)) {
				cursor = 4;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;
				_timer = 0;
				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 4;
			}

			else if (_timer <= 0) {
				cursor = 10;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 10;
			}
			break;
		case 10:
			if (_timer <= 0) {
				cursor = 11;
				_speaker = nodes [cursor].SelectSingleNode ("Name").InnerText;
				_text = nodes [cursor].SelectSingleNode ("Sentence").InnerText;

				UiManager.I.ChangeStorySentence (_text, _speaker);
				next_s = 11;
			}
			break;
		case 11: //Speak_End
			if (_timer <= 0) {
				EndStory ();
                EndGame = true;
            }
			break;

		}

	}
	void ActiveTimer(){
		if (_timer >= 0) {
			_timer -= Time.deltaTime;
		}
		else {
			_timer = 0.0f;
		}
	}

	void ActiveWaitTimer(){
		if (_wait >= 0)
			_wait -= Time.deltaTime;
		else {
			_wait = 0.0f;
		}
	}

	void ResetWaitTimer(){

		_wait = WAITTIME;
	}

	void Xme_Load(string _name){

		TextAsset text = (TextAsset)Resources.Load ("XML/" + _name);
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml (text.text);

		//read by element
		sentences = xmldoc.GetElementsByTagName ("Sentence");

		//read all
		nodes = xmldoc.SelectNodes ("Uniset/Talk");

		foreach (XmlNode node in nodes) {
			//Debug.Log ("Name : " + node.SelectSingleNode ("").Innertext);

		}

	}


}

