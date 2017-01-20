using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopFlower : MonoBehaviour {

	[SerializeField]
	private int index;
	public int Index {
		get { return index; }
	}

	[SerializeField]
	private int price;
	public int Price {
		get { return price; }
	}

	public void ChangeIndex (int v){

		index = v;
	}

	public void ChangePrice (int v){

		price = v;
	}
}
