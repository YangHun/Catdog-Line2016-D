using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour {

    private bool _is_start;
    public bool isgameStart
    {
        get { return _is_start; }
        set { _is_start = value; }
    }

    [SerializeField]
    Collider2D colSave, colStart;

    float timer;

    void Start()
    {

        isgameStart = false;
        timer = 0.0f;
    }


    void Update()
    {

        if(timer > 1.0f)
        {
            Debug.Log("End!");
            isgameStart = false;
        }

    }

    public void ActiveTimer()
    {
        if (isgameStart)
            timer += Time.deltaTime;    
    }
    
}
