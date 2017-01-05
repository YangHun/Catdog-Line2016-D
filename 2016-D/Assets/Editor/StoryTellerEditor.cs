using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

//[CustomEditor(typeof(StoryTeller))]
[CanEditMultipleObjects]
public class StoryTellerEditor : Editor {

	SerializedProperty _script;
	SerializedProperty _condition;
	SerializedProperty _cstartindex;
	SerializedProperty _cendindex;
	SerializedProperty _time;
	SerializedProperty _type;

	SerializedProperty _makecondition;

	int n = 0;
	int cursor = 0;

	int ccursor = -1;

	int start = 0;
	int end = 0;

	void OnEnable(){

		_script = serializedObject.FindProperty ("Script");
		_condition = serializedObject.FindProperty ("ConditionScript");
		_cstartindex = serializedObject.FindProperty ("ConditionIndexStart");
		_cendindex = serializedObject.FindProperty ("ConditionIndexEnd");
		_time = serializedObject.FindProperty ("Time");
		_type = serializedObject.FindProperty ("Type");

		_makecondition = serializedObject.FindProperty ("MakeCondition");

	}

	public override void OnInspectorGUI ()
	{

		EditorGUILayout.LabelField ("Story Teller");


		serializedObject.Update ();
		serializedObject.ApplyModifiedProperties ();

		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Add")) {
			n++;

			_script.arraySize = n;
			_time.arraySize = n;
			_type.arraySize = n;
			serializedObject.ApplyModifiedProperties ();
		}

		if (GUILayout.Button ("Remove")) {
			if (n > 0)
				n--;
			
			_script.arraySize = n;
			_time.arraySize = n;
			_type.arraySize = n;
			serializedObject.ApplyModifiedProperties ();
		}

		EditorGUILayout.EndHorizontal ();



		EditorGUILayout.BeginHorizontal ();

		EditorGUILayout.LabelField ("Time", EditorStyles.boldLabel, GUILayout.Width (50));
		EditorGUILayout.LabelField ("Type", EditorStyles.boldLabel, GUILayout.Width (100));
		EditorGUILayout.LabelField ("Script", EditorStyles.boldLabel);

		EditorGUILayout.EndHorizontal ();


		for (int i = 0; i < n; i++) {
			EditorGUILayout.BeginHorizontal ();
			serializedObject.Update ();
			_time.GetArrayElementAtIndex (i).floatValue = EditorGUILayout.FloatField (_time.GetArrayElementAtIndex (i).floatValue, GUILayout.Width (50));
			serializedObject.ApplyModifiedProperties ();
			_type.GetArrayElementAtIndex (i).enumValueIndex = (int)((StoryTeller.ScriptType)(EditorGUILayout.EnumPopup ((StoryTeller.ScriptType)_type.GetArrayElementAtIndex (i).enumValueIndex, GUILayout.Width (100))));
			serializedObject.ApplyModifiedProperties ();
		
			if (_type.GetArrayElementAtIndex (i).enumValueIndex != (int)StoryTeller.ScriptType.Question_Catcher) {
				_script.GetArrayElementAtIndex (i).stringValue = EditorGUILayout.TextField (_script.GetArrayElementAtIndex (i).stringValue);
				serializedObject.ApplyModifiedProperties ();
			} 
				
			EditorGUILayout.EndHorizontal ();

		


			if (_type.GetArrayElementAtIndex (i).enumValueIndex == (int)StoryTeller.ScriptType.Question_Catcher) {


				if ((ccursor < 0) || ((ccursor>=0)&& (!_makecondition.GetArrayElementAtIndex(ccursor).boolValue))) {
					if (GUILayout.Button ("Make Conditions!")) {
						_makecondition.arraySize++;
						_makecondition.GetArrayElementAtIndex (_makecondition.arraySize - 1).boolValue = true;

						ccursor++;
						start = end;
						end = start;
						_cstartindex.arraySize++;
						serializedObject.ApplyModifiedProperties ();
						_cendindex.arraySize++;
						serializedObject.ApplyModifiedProperties ();
					}
				}
				if ((ccursor >= 0) && (_makecondition.GetArrayElementAtIndex(ccursor).boolValue)) {
				
					if (GUILayout.Button ("Discard Conditions")) {
						
						_makecondition.arraySize--;
						serializedObject.ApplyModifiedProperties ();

						ccursor--;
							
						for(int j = start; j < end; j++){
							_condition.arraySize--;
						}


						if (ccursor >= 0) {
							_cstartindex.arraySize = ccursor;
							_cendindex.arraySize = ccursor;
							start = _cstartindex.GetArrayElementAtIndex (ccursor).intValue;
							end = _cendindex.GetArrayElementAtIndex (ccursor).intValue;

						} else {
							_cstartindex.arraySize = 0;
							_cendindex.arraySize = 0;

							_condition.arraySize = 0;
						}
						serializedObject.ApplyModifiedProperties ();
					}
					EditorGUILayout.BeginHorizontal ();

					if (GUILayout.Button ("Add Condition Script")) {
						end++;

						_condition.arraySize++;	
						_cstartindex.GetArrayElementAtIndex (ccursor).intValue = start;
						serializedObject.ApplyModifiedProperties ();
						_cendindex.GetArrayElementAtIndex (ccursor).intValue = end;
						serializedObject.ApplyModifiedProperties ();
					}

					if (GUILayout.Button ("Remove Condition Script")) {
						if (_condition.arraySize > 0) {

							if (start < end)
								end--;

							_condition.arraySize--;
							_cstartindex.GetArrayElementAtIndex (ccursor).intValue = start;
							serializedObject.ApplyModifiedProperties ();
							_cendindex.GetArrayElementAtIndex (ccursor).intValue = end;
							serializedObject.ApplyModifiedProperties ();
						}
					}

					EditorGUILayout.EndHorizontal ();

					for (int j = 0; j < _condition.arraySize; j++) {
						EditorGUILayout.BeginHorizontal ();

						EditorGUILayout.LabelField ((start + j).ToString (), GUILayout.Width (50));
						_condition.GetArrayElementAtIndex (start + j).stringValue = EditorGUILayout.TextField (_condition.GetArrayElementAtIndex (start + j).stringValue);

						EditorGUILayout.EndHorizontal ();
					}


					EditorGUILayout.LabelField ("ccursor", ccursor.ToString()); //condition index cursor
					if (ccursor >= 0) {
						EditorGUILayout.LabelField ("start", _cstartindex.GetArrayElementAtIndex (ccursor).intValue.ToString ());
						EditorGUILayout.LabelField ("end", _cendindex.GetArrayElementAtIndex (ccursor).intValue.ToString ());
					}
				}

			}
		}

        ccursor = EditorGUILayout.IntField(ccursor);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_cstartindex, true);
        EditorGUILayout.PropertyField(_cendindex, true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(_condition, true);

		EditorGUILayout.PropertyField (_makecondition, true);
	}
}	
			
		