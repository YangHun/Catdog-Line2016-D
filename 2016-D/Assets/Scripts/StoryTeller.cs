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

		ActiveTimer ();

	}

	[SerializeField]
	private float _timer = SPEAKTIME;
	[SerializeField]
	private float _wait = WAITTIME;
	private const float SPEAKTIME = 5.0f;
	private const float WAITTIME = 10.0f;

	int cursor = 0;

	bool isFirstReject = true;
	bool isTimeRunning = true;

	public void StoryTelling(){

		if ( _timer <= 0) {

			Debug.Log (nodes);
			Debug.Log (nodes [cursor]);
			Debug.Log (nodes [cursor].SelectSingleNode("type"));
			Debug.Log (nodes [cursor].SelectSingleNode("type").InnerText);

			string n = nodes [cursor].SelectSingleNode("type").InnerText;
			string _speaker;
			string _text;


			cursor++;
			_speaker = nodes[cursor].SelectSingleNode ("Name").InnerText;
			_text = nodes[cursor].SelectSingleNode ("Sentence").InnerText;

			if (n == "Speak") {

				UiManager.I.ChangeStorySentence (_text, _speaker);
				ResetTimer ();

			}
			else if (n == "Question") {

				UiManager.I.ChangeStorySentence (_text, _speaker);

				ActiveWaitTimer ();

				if(_wait <= 0.0f){

					if (isFirstReject) {

						cursor = 7 - 1;
						isFirstReject = false;
					}
					else {
						cursor = 10 - 1;
					}


					ResetTimer ();
					ResetWaitTimer ();
				}

				else if (Input.GetMouseButtonDown(0)) {
					cursor = 5 - 1;

					ResetTimer ();
					ResetWaitTimer ();
				}

			}

			 else if (n == "Answer") {

				UiManager.I.ChangeStorySentence (_text, _speaker);
				// optional scripting

				ResetTimer ();
			}

			else if (n == "Speak_End"){
				UiManager.I.ChangeStorySentence (_text, _speaker);


			}

		}

	}

	void ActiveTimer(){
		if (_timer >= 0)
			_timer -= Time.deltaTime;
		else {
			_timer = 0.0f;
		}
	}

	void ResetTimer(){

		_timer = SPEAKTIME;
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

