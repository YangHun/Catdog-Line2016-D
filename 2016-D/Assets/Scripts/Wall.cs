using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenWall()
    {
        _open();
    }

    private void _open()
    {
        this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        StartCoroutine("FadeOut");
    }

    private const float FADETIME = 1.0f;

    IEnumerator FadeOut()
    {

        for (float i = FADETIME; i >= 0; i -= Time.deltaTime)
        {
            Renderer r = GetComponent<SpriteRenderer>();
            Color c = r.material.color;
            c.a = i / FADETIME;
            r.material.color = c;

			for (int j = 0; j < transform.childCount; j++) {
				transform.GetChild (j).GetComponent<SpriteRenderer> ().material.color = c;
			}
            yield return null;
        }

        if (GetComponent<SpriteRenderer>().material.color.a <= Time.deltaTime)
            Destroy(this.gameObject);
    }

}
