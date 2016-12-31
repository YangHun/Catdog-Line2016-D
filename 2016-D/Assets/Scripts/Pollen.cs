using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pollen : MonoBehaviour {

    [SerializeField]
    private int _value;

    private const float FADETIME = 0.7f;

    public int Value
    {
        get { return _value; }
    }

    public Pollen (int v)
    {
        _value = v;
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
       
        for (float i = FADETIME; i > 0; i -= Time.deltaTime)
        {
            Renderer r = GetComponent<SpriteRenderer>();
            Color c = r.material.color;
            c.a = i / FADETIME;
            r.material.color = c;

            if (c.a < 0)
                Destroy(this.gameObject);

            yield return null;
        }
    }
}