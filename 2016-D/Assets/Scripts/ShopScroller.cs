using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScroller : MonoBehaviour {

	[SerializeField]
	private GameObject[] obj;
	private GameObject c;

	private Image img;

	[SerializeField]
	private List<Vector2> _pos = new List<Vector2>();

	private int _w;
	private int _h;

	private Vector2 Cursor;

	// Use this for initialization
	void Start () {

		img = GameObject.Find ("Catcher UI/Image").GetComponent<Image> ();
		c = GameObject.Find ("Catcher UI/Scroll View/Viewport/Content");
		obj = new GameObject[c.transform.childCount];

		for (int i = 0; i < obj.Length; i++) {
			obj [i] = c.transform.GetChild (i).gameObject;
			_pos.Add (obj [i].GetComponent<RectTransform> ().position);
		}

		_w = (int)(gameObject.GetComponent<RectTransform> ().rect.width / 2.0f );
		_h = (int)gameObject.GetComponent<RectTransform> ().rect.height;

		Cursor = new Vector2 (_w, _h);
        Debug.Log(Cursor);

		ChangeImage ();
	}

	public void ChangeImage(){


        //TODO : 고쳐야한다
		List<Vector2> __p = new List<Vector2>();

		for (int i = 0; i < _pos.Count; i++) {
			__p.Add( _pos [i] + (Vector2)(c.GetComponent<RectTransform>().position));
		}
			
		Debug.Log(~(__p.BinarySearch (Cursor, new Vector2Comparer())));

		int _cursor = ~(__p.BinarySearch (Cursor, new Vector2Comparer ()));

		img.sprite = obj [_cursor].GetComponent<Image> ().sprite;
		img.gameObject.GetComponent<ShopFlower> ().ChangeIndex( obj [_cursor + 1].GetComponent<ShopFlower> ().Index);
		img.gameObject.GetComponent<ShopFlower> ().ChangePrice (obj [_cursor + 1 ].GetComponent<ShopFlower> ().Price);

	}

	// Update is called once per frame
	void Update () {
		
	}
}

class Vector2Comparer : IComparer<Vector2>
{
	public int Compare(Vector2 a, Vector2 b)
	{
		if (a.x == b.x) {

			if (a.y == b.y) {
				return 0; //equal
			} else {

				if (a.y > b.y)
					return 1; //a is greater
				else
					return 1; //b is greater

			}
		}

		else {

			if (a.x > b.x)
				return 1; //a is greater
			else
				return -1; //b is greater
		}

	}
}