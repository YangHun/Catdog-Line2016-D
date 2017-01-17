using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pollen : MonoBehaviour {
	

    private const float FADETIME = 0.3f;

	[SerializeField]
	private int _value;
    public int Value
    {
        get { return _value; }
    }

    public Pollen (int v)
    {
        _value = v;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine("FadeOut");
       
    }

    IEnumerator FadeOut()
    {
       
        for (float i = FADETIME; i >= 0; i -= Time.deltaTime)
        {
            Renderer r = GetComponent<SpriteRenderer>();
            Color c = r.material.color;
            c.a = i / FADETIME;
			r.material.color = c;
            yield return null;
        }
           
        Destroy(this.gameObject);
    }
}