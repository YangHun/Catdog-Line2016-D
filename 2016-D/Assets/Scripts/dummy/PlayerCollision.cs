using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pollen")
        {
            Pollen p = collision.gameObject.GetComponent<Pollen>();

            FieldManager.I.ObtainPollen(p.Value);
        }
    }
}
