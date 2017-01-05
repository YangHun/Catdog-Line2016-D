using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTeller : MonoBehaviour {


	public enum ScriptType { Text_I, Text_Catcher, Question_Catcher, Null };

	[SerializeField]
	private List<string> Script;
	[SerializeField]
	private List<float> Time;
	[SerializeField]
	private List<ScriptType> Type;


	[SerializeField]
	List<bool> MakeCondition = new List<bool>();
	[SerializeField]
	private List<string> ConditionScript;
	[SerializeField]
	private List<int> ConditionIndexStart;
	[SerializeField]
	private List<int> ConditionIndexEnd;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
