using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData {

    protected int LocalPlayCount;
    protected int TotalPlayCount;

    protected int LocalPollen;
    protected int TotalPollen;

    protected Dictionary<string, int> FlowerDict;
    
    public PlayerData()
    {

        //init
        //TODO : Load
        LocalPlayCount = 0;
        TotalPlayCount = 0;
        LocalPollen = 0;
        TotalPollen = 0;

        FlowerDict = new Dictionary<string, int>()
        {
            { "Poppy", 0 }, //양귀비
            { "Aster", 0 }, //아스터
            { "White-egret", 0 }, //해오라비난초
            { "Snowdrop", 0 }, //스노우드롭
            { "Anemone", 0 }, //아네모네
            { "Baby's", 0 }, //안개꽃
            { "Valley", 0 }, //은방울꽃
            { "Kalmia", 0 },  //칼미아
        };
    }
           
    public string PathForDocumentsFile (string filename)
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }

        return "";

    }
}

public class SaveData : PlayerData
{



    public SaveData()
    {

    }

    public void Write() //Encode & Save
    {
        //string path = PathForDocumentsFile("note.diary");
        string path = "C:/Users/SuddenAttack2/Desktop/note.diary";

        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter sw = new StreamWriter(file);

        sw.Write("tc10000\n");
        sw.Write("tp20000\n");

        //TODO : Write


        sw.Close();
        file.Close();

    }



}

public class LoadData : PlayerData
{
    public LoadData()
    {

    }

    public void Read() //Load & Parse
    {
        //string path = PathForDocumentsFile("note.diary");
        string path = "C:/Users/SuddenAttack2/Desktop/note.diary";

        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            int a = 10;
            while (!sr.EndOfStream) {
                string str = sr.ReadLine();

                //TODO : Parse

                switch (str.Substring(0,2))
                {
                    case "tc":
                        LocalPlayCount = int.Parse(str.Substring(2));
                        Debug.Log("LocalPlayCount : " + LocalPlayCount);
                        break;
                    case "tp":
                        LocalPollen = int.Parse(str.Substring(2));
                        Debug.Log("LocalPollen : " + LocalPollen);
                        break;
                }

                Debug.Log(str);
            }            

            sr.Close();
            file.Close();

        }
    }
}