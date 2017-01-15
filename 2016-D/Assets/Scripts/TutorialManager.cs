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

	private bool _open = false;
	private bool _start_menu_tutorial = false;

	[SerializeField]
	private GameObject Gate;
	[SerializeField]
	private static GameObject[] Flowers = new GameObject[6];
    [SerializeField]
    private bool[] HasFlowers = new bool[6];
    
	private Dictionary<GameObject, int> FlowerDict;

    private Vector3[,] _flowerpos = new Vector3[2, 3] {
        { new Vector3 (-5.83f, -0.16f, 0.0f), new Vector3 (-3.64f, 4.84f, 0.0f), new Vector3(2.38f, -5.42f, 0.0f) },
        { new Vector3 (-0.58f, -8.57f, 0.0f), new Vector3 (-6.65f, -4.75f, 0.0f), new Vector3(-7.92f, 1.51f, 0.0f) }
    };


    int fieldopen = -1;

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

		GameObject _f = transform.GetChild (3).gameObject;
		for (int i = 0; i < 6; i++) {
			Flowers [i] = _f.transform.GetChild (i).gameObject;
		}

		FlowerDict = new Dictionary<GameObject, int>()
		{
			{ Flowers[0], 0 }, //안개꽃
			{ Flowers[1], 1 }, //은방울꽃
			{ Flowers[2], 2 },  //칼미아
			{ Flowers[3], 3 }, //안개꽃
			{ Flowers[4], 4 }, //은방울꽃
			{ Flowers[5], 5 }  //칼미아
		};

    }
	
	// Update is called once per frame
	void Update () {
		if (fieldopen > -1 && cursor < 2)
        {
			
			Wall[fieldopen].OpenWall();
            cursor++;
            fieldopen = -1;
        }

        if(cursor >= 2) {

			if (!_open) {
				
				StartCoroutine ("OpenGate");
				_open = true;
			}            
                //End Tutorial
        }
	}

	IEnumerator OpenGate(){

		yield return new WaitForSeconds (2f);
		Gate.SetActive (true);
	}

    public void Init()
    {



		UiManager.I.UpdatePollenText (GameManager.Data.LocalPlln);

        cursor = 0;
	
        GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.zero;

		_open = false;
		Gate.SetActive (_open);

    }

    public void ObtainFlower(GameObject t)
    {
        Debug.Log("Enter!!");
        HasFlowers[FlowerDict[t]] = true;

		fieldopen = openfield();
    }

	private enum WallOpenType {Wall_1, Wall_2, None}

	int openfield()
    {
		int c = 0;

        for (int i = 0; i < 3; i++)
        {
			if (!HasFlowers [i]) {
				c = -1;
				break;
			}
        }

		if (c == 0 && cursor > 0) {
			c = 1;
			for (int i = 3; i < 6; i++)
			{
				if (!HasFlowers [i]) {
					c = -1;
					break;
				}
			}

		}

        return c;
    }
		

	public void EndTutorialGame(){
		GameManager.I.TrnsTutorialToTMenu ();
	}
}
