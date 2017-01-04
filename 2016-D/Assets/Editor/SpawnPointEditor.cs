using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SpawnPoint))]
//[CanEditMultipleObjects]
public class SpawnPointEditor : Editor {

	private const int MAX = 8;

	SerializedProperty _flowertypes;
	SerializedProperty _flowercollected;
	SerializedProperty _spawnpos;

	SerializedProperty _type;
	SpawnPoint.Type __type; 

	Vector3 pos;

	int count;

	void OnEnable(){
		
		_flowertypes = serializedObject.FindProperty ("_flowers");
		_flowercollected = serializedObject.FindProperty ("_collected");
		_spawnpos = serializedObject.FindProperty ("_spawnpos");
		_type = serializedObject.FindProperty ("_spawn");
		__type = (SpawnPoint.Type)(_type.enumValueIndex);
	}

	public override void OnInspectorGUI(){


	//	Tools.hidden = true;

		if (__type == SpawnPoint.Type.Null) {

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Flower")) {
				__type = SpawnPoint.Type.Flower;
				_type.enumValueIndex = (int)__type;
			} else if (GUILayout.Button ("Enemy")) {
				__type = SpawnPoint.Type.Enemy;
				_type.enumValueIndex = (int)__type;
			} else {
				EditorGUILayout.LabelField (" : Select Spawn Type");
			}

			serializedObject.ApplyModifiedProperties ();
			EditorGUILayout.EndHorizontal ();

		}
		else if (__type == SpawnPoint.Type.Flower) {

			if (GUILayout.Button ("Reset Spawn Type")) {
				__type = SpawnPoint.Type.Null;
				_type.enumValueIndex = (int)__type;
				serializedObject.ApplyModifiedProperties ();
			}

			int current = 0;

			serializedObject.Update ();

			EditorGUILayout.Foldout (true, "List of Spawning Flowers");

			EditorGUILayout.BeginHorizontal ();


			if (GUILayout.Button ("▲")) {
				/*	for(int i =0; i < _flowertypes.arraySize - 1 ; i++){
					_flowertypes.Next (true);
				}

				if (current > 0)
					current--;
				else
					current = _flowertypes.arraySize - 1;

				Debug.Log (current);
		*/
			}

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

				if (_flowertypes.arraySize < MAX) {
					_flowertypes.arraySize++;
					serializedObject.ApplyModifiedProperties ();
				}
				if (_flowercollected.arraySize < MAX) {

					_flowercollected.arraySize++;
					serializedObject.ApplyModifiedProperties ();

				}
				if (_spawnpos.arraySize < MAX) {
					_spawnpos.arraySize++;
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
				if (_spawnpos.arraySize > 0) {
					_spawnpos.arraySize--;
					serializedObject.ApplyModifiedProperties ();

				}
			}
			EditorGUILayout.EndHorizontal ();


			EditorGUILayout.BeginHorizontal ();

			EditorGUILayout.PropertyField (_flowertypes, true);
			EditorGUILayout.PropertyField (_flowercollected, true);

			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.PropertyField (_spawnpos, true);

			serializedObject.ApplyModifiedProperties ();

		} else if (__type == SpawnPoint.Type.Enemy) {

			if (GUILayout.Button ("Reset Spawn Type")) {
				__type = SpawnPoint.Type.Null;
				_type.enumValueIndex = (int)__type;
				serializedObject.ApplyModifiedProperties ();
			}
				
			EditorGUILayout.LabelField ("Enemy");

			pos = Selection.activeGameObject.transform.position;

			EditorGUILayout.Vector3Field ("Current Pos :", pos);


			EditorGUILayout.BeginHorizontal ();


			if (GUILayout.Button ("Add")) {

				_spawnpos.arraySize++;
				serializedObject.ApplyModifiedProperties ();

			//	_spawnpos.Getat.objectReferenceValue = pos;
				_spawnpos.GetArrayElementAtIndex(_spawnpos.arraySize - 1).vector3Value = pos;

				serializedObject.ApplyModifiedProperties ();

			}
			if (GUILayout.Button ("Edit")) {
				if (_spawnpos.arraySize > 0) {

				
				}
			}
			if (GUILayout.Button ("Remove")) {

				if (_spawnpos.arraySize > 0) {
					_spawnpos.arraySize--;
					serializedObject.ApplyModifiedProperties ();
				}
			}

			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();

			serializedObject.Update ();
			EditorGUILayout.PropertyField (_spawnpos, true);
			serializedObject.ApplyModifiedProperties ();

			EditorGUILayout.EndHorizontal ();

		}


	}

	public void OnSceneGUI(){

	}
}