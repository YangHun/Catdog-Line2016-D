using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    float speed;
    Vector3 direction;
    Vector3 rotAngle;
    Camera cam;
    
	// Use this for initialization
	void Start () {

        // init
        speed = 0.1f;
        cam = Camera.main;
        rotAngle = new Vector3(0.0f, 0.0f, 0.0f);
        direction = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {

            

        }


#endif

#if UNITY_EDITOR

        if (Input.GetMouseButton(0))
        {
           
            direction = Input.mousePosition - new Vector3(Screen.width/2.0f, Screen.height/2.0f,0.0f);
            rotAngle.z = Vector2.Angle(new Vector2(direction.x, direction.y), Vector2.up);
            direction.Normalize();
         
            transform.position += direction * speed;

            if (direction.x > 0)
                rotAngle.z = 360.0f - rotAngle.z;
            
            transform.rotation = Quaternion.Euler(rotAngle);

          //  cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
           
        }

#endif
    }
}
