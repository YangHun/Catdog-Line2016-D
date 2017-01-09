using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    //Singleton
    private static TutorialManager _manager;
    public static TutorialManager I
    {
        get
        {
            return _manager;
        }
    }

    [SerializeField]
    private Wall[] Wall;

    private int cursor = 0;


    [SerializeField]
    private UnityEngine.Object[] FlowerPrefab = new UnityEngine.Object[3];
    private GameObject[] Flowers = new GameObject[3];
    [SerializeField]
    private bool[] HasFlowers = new bool[3];
    
    private Dictionary<Flower.Type, int> FlowerDict = new Dictionary<Flower.Type, int>()
        {
            { Flower.Type.Baby_s, 0 }, //안개꽃
            { Flower.Type.Valley, 1 }, //은방울꽃
            { Flower.Type.Kalmia, 2 },  //칼미아
        };

    private Vector3[,] _flowerpos = new Vector3[2, 3] {
        { new Vector3 (-5.83f, -0.16f, 0.0f), new Vector3 (-3.64f, 4.84f, 0.0f), new Vector3(2.38f, -5.42f, 0.0f) },
        { new Vector3 (-0.58f, -8.57f, 0.0f), new Vector3 (-6.65f, -4.75f, 0.0f), new Vector3(-7.92f, 1.51f, 0.0f) }
    };


    bool fieldopen = false;

    // Use this for initialization
    void Start () {
        if(_manager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _manager = this;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (fieldopen)
        {
            Wall[cursor].OpenWall();
            cursor++;
            resetFlowers();
            fieldopen = false;
        }

        if(cursor >= 2) {
            
                //End Tutorial
        } 
	}

    public void Init()
    {

        cursor = 0;

        for (int i=0; i<Flowers.Length; i++)
        {
            GameObject g = (GameObject)Instantiate(FlowerPrefab[i], _flowerpos[cursor,i], Quaternion.identity);
            g.transform.SetParent(transform.GetChild(2)); //Flower Empty Object
        }

        GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.zero;

    }

    public void ObtainFlower(Flower.Type t)
    {
        Debug.Log("Enter!!");
        HasFlowers[FlowerDict[t]] = true;

        fieldopen = openfield();
    }

    bool openfield()
    {
        bool c = true;

        for (int i = 0; i < HasFlowers.Length; i++)
        {
            if (!HasFlowers[i])
                c = false;
        }

        return c;
    }

    void resetFlowers()
    {
        if (cursor < 2)
        {
            for (int i = 0; i < HasFlowers.Length; i++)
            {
                HasFlowers[i] = false;

                Flowers[i] = (GameObject)Instantiate(FlowerPrefab[i], Vector3.zero, Quaternion.identity);
                Flowers[i].gameObject.transform.position = _flowerpos[cursor, i];
                Flowers[i].transform.SetParent(transform.GetChild(2).transform);
            }
        }

    }

        

}
