using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SpawnPoint))]
//[CanEditMultipleObjects]
public class SpawnPointEditor : Editor {

	private const int MAX = 8;

	SpawnPoint.Type __type;
	SerializedProperty _flowertypes;
	SerializedProperty _flowercollected;
	SerializedProperty _flowerpos;

	void OnEnable(){
		
		_flowertypes = serializedObject.FindProperty ("_flowers");
		_flowercollected = serializedObject.FindProperty ("_collected");
		_flowerpos = serializedObject.FindProperty ("_spawnpos");
	}

	public override void OnInspectorGUI(){



		__type = (SpawnPoint.Type)EditorGUILayout.EnumPopup ("Spawn Type ", __type);
		/*
		switch (__type){
		case SpawnPoint.Type.Flower:
			EditorGUILayout.LabelField ("Flower");

			break;
		case SpawnPoint.Type.Enomy:
			EditorGUILayout.LabelField ("Enomy");
			break;
		}

		*/

		int current = 0;

		serializedObject.Update ();

		EditorGUILayout.Foldout (true,"List of Spawning Flowers");

		GUILayout.BeginHorizontal ();


		if (GUILayout.Button ("▲")) {
		/*	for(int i =0; i < _flowertypes.arraySize - 1 ; i++){
				_flowertypes.Next (true);
			}

			if (current > 0)
				current--;
			else
				current = _flowertypes.arraySize - 1;

			Debug.Log (current);
	*/	}

		if (GUILayout.Button ("▼")) {
	/*		_flowertypes.Next (true);

			if (current < _flowertypes.arraySize)
				current++;
			else
				current = 0;

			Debug.Log (current);
*/
		}

		if (GUILayout.Button ("Add")) {
			
			if(_flowertypes.arraySize < MAX){
				_flowertypes.arraySize++;
				serializedObject.ApplyModifiedProperties ();
			}
			if(_flowercollected.arraySize < MAX){
	
				_flowercollected.arraySize++;
				serializedObject.ApplyModifiedProperties ();

			}
			if(_flowerpos.arraySize < MAX){
				_flowerpos.arraySize++;
				serializedObject.ApplyModifiedProperties ();

			}
		}

		if (GUILayout.Button ("Remove")) {

			if (_flowertypes.arraySize > 0) {
				Debug.Log (_flowertypes.arraySize + " : -");
				_flowertypes.arraySize--;
				serializedObject.ApplyModifiedProperties ();
			}
			if (_flowercollected.arraySize > 0) {
				Debug.Log (_flowercollected.arraySize + " : -");
				_flowercollected.arraySize--;
				serializedObject.ApplyModifiedProperties ();
			}
			if(_flowerpos.arraySize > 0){
				_flowerpos.arraySize--;
				serializedObject.ApplyModifiedProperties ();

			}
		}
		GUILayout.EndHorizontal ();


		EditorGUILayout.BeginHorizontal ();

		EditorGUILayout.PropertyField (_flowertypes, true);
		EditorGUILayout.PropertyField (_flowercollected, true);

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.PropertyField (_flowerpos, true);

		serializedObject.ApplyModifiedProperties ();


	}

	public void OnSceneGUI(){

	}
}