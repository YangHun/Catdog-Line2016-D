using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{

    PointManager manager;

    // Use this for initialization
    void Start()
    {
        manager = transform.GetComponentInParent<PointManager>();
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            manager.isgameStart = true;
        }
    }
}
