using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fileIO : MonoBehaviour {

    PlayerData data;
    SaveData save;
    LoadData load;

	// Use this for initialization
	void Start () {

        data = new PlayerData();
        save = new SaveData();
        load = new LoadData();

        string path = data.PathForDocumentsFile("note.diary");
        GameObject.Find("Canvas/Text").GetComponent<Text>().text = path;

        save.Write();
        load.Read();

    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
