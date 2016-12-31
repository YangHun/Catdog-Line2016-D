using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData {

    private int LocalPlayCount;
    public int LocalPCount
    {
        get { return LocalPlayCount; }
            
    }

    private int LocalPollen;
    public int LocalPlln
    {
        get { return LocalPollen; }
    }

    private int TotalPlayCount;
    private int TotalPollen;

    public List<string> FlowerIndex;
    private Dictionary<string, int> FlowerDict;

    public PlayerData()
    {

        //init
        //TODO : Load
        LocalPlayCount = 0;
        TotalPlayCount = 0;
        LocalPollen = 0;
        TotalPollen = 0;

        FlowerIndex = new List<string>()
        {
            "Poppy", //양귀비
            "Aster", //아스터
            "White-egret", //해오라비난초
            "Snowdrop", //스노우드롭
            "Anemone", //아네모네
            "Baby's", //안개꽃
            "Valley", //은방울꽃
            "Kalmia"  //칼미아
        };

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

    private string PathForDocumentsFile (string filename)
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }

        return "";

    }

    public bool Write()
    {
        _write();

        return true;
    }

    private void _write() //Encode & Save
    {
        //string path = PathForDocumentsFile("note.diary");
        string path = "C:/Users/KUICAT/Desktop/note.diary";

        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter sw = new StreamWriter(file);

/*
        TotalPlayCount = 1000;
        TotalPollen = 1200;

        for (int i = 0; i < FlowerDict.Count; i++)
        {
            FlowerDict[FlowerIndex[i]] = 2 * i + 1;
        }

    */

        //TODO : Write
        sw.WriteLine("tc");
        sw.WriteLine(TotalPlayCount);
        sw.WriteLine("tp");
        sw.WriteLine(TotalPollen);
        sw.WriteLine("fdict");
        for (int i = 0; i < FlowerDict.Count; i++)
        {
            sw.WriteLine(FlowerDict[FlowerIndex[i]]);
        }

        sw.Close();
        file.Close();

    }

    public bool Read()
    {
        _read();

        return true;
    }

    private void _read() //Load & Parse
    {
        //string path = PathForDocumentsFile("note.diary");
        string path = "C:/Users/KUICAT/Desktop/note.diary";

        if (!File.Exists(path))
        {
            _write(); //make a data file
        }

        FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(file);

        while (!sr.EndOfStream)
        {
            string str = sr.ReadLine();

            switch (str)
            {
                case "tc":
                    string tmp = sr.ReadLine();
                    LocalPlayCount = int.Parse(tmp);
                    Debug.Log("LocalPlayCount : " + LocalPlayCount);
                    break;
                case "tp":
                    tmp = sr.ReadLine();
                    LocalPollen = int.Parse(tmp);
                    Debug.Log("LocalPollen : " + LocalPollen);
                    break;
                case "fdict":
                    for (int i = 0; i < FlowerDict.Count; i++)
                    {
                        tmp = sr.ReadLine();
                        if (tmp != null)
                            FlowerDict[FlowerIndex[i]] = int.Parse(tmp);
                        else
                            FlowerDict[FlowerIndex[i]] = -1;

                    }
                    break;
            }
        }

        sr.Close();
        file.Close();
    }

    public enum UpdateType { PlayCount, Pollen }
    public bool Update(int l, UpdateType type)
    {
        if (type == UpdateType.PlayCount)
        {
            _update(l, 0, -1, 0);
        }
        else
        {
            _update(0, l, -1, 0);
        }
        return true;
    }

    public bool Update(int index, int value)
    {
        _update(0, 0, index, value);
        return true;
    }

    public bool Update(int lc, int lp, int[] fd)
    {
        _updateall(lc, lp, fd);

        return true;
    }

    private void _update(int lc, int lp, int index, int value)
    {
        if (lc > 0)
            TotalPlayCount += lc;

        if (lp > 0)
            TotalPollen += lp;

        if (index >= 0 && index < FlowerIndex.Count && value > 0)
        {
            FlowerDict[FlowerIndex[index]] = value;
        }
    }

    private void _updateall(int lc, int lp, int[] fd)
    {
        if(lc > 0)
           TotalPlayCount += lc;

        if(lp > 0)
            TotalPollen += lp;

        if (fd != null)
        {
            for (int i = 0; i < fd.Length; i++)
            {
                FlowerDict[FlowerIndex[i]] = fd[i];
            }
        }
    }
}

 