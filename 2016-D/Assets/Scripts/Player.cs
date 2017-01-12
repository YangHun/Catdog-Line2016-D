using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	private float speed;
	[SerializeField]
	private Vector3 Point;
	[SerializeField]
	private Vector3 Dir;
	private Vector3 rotAngle;

	private bool _killed = false;
	public bool isPlayerKilled{
		get { return _killed; }
	}

	void Start()
	{
		//init
		speed = 0.1f;
		rotAngle = Vector3.zero;
		Dir = Vector3.zero;
	}

	// Update is called once per frame
	void Update () {


		if (GameManager.I.CurrentState == GameManager.GameFlow.Game)
		{
			GetHandle();
			UiManager.I.SetHandle(Point, Dir,UiManager.UICanvas.Game);
		}
		else if (GameManager.I.CurrentState == GameManager.GameFlow.Tutorial)
		{
			GetHandle();
			UiManager.I.SetHandle(Point, Dir,UiManager.UICanvas.Tutorial);
		}

	}

	private bool isActive = false;

	public void EndGame(){
		_killed = false;	
	}


	void GetHandle()
	{
		if (!isActive)
		{
			if (Input.GetMouseButton(0))
			{
				isActive = true;
				Point = Input.mousePosition;
			}
		}
		else//hold
		{
			if (Input.GetMouseButton(0))
			{
				Dir = Input.mousePosition - Point;
				Dir.Normalize();
				transform.position = transform.position + Dir * speed;
				Camera.main.transform.position = transform.position - new Vector3(0.0f, 0.0f, 10.0f);

				RotatePlayer();
			}
			else if (Input.GetMouseButtonUp(0))
			{
				isActive = false;
				Point = Vector3.zero;
				Dir = Vector3.zero;
			}
		}
	}

	void RotatePlayer()
	{
		rotAngle.z = Vector2.Angle(new Vector2(Dir.x, Dir.y), Vector2.up);

		if (Dir.x > 0)
			rotAngle.z = 360.0f - rotAngle.z;

		transform.rotation = Quaternion.Euler(rotAngle);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Pollen") {
			Pollen p = col.gameObject.GetComponent<Pollen> ();

            if (GameManager.I.CurrentState == GameManager.GameFlow.Game)
                FieldManager.I.ObtainPollen(p.Value);
            else if (GameManager.I.CurrentState == GameManager.GameFlow.Tutorial)
                TutorialManager.I.ObtainPollen(p.Value);
        }
        else if (col.gameObject.tag == "Flower")
        {

            GameObject t = col.gameObject;

            if (GameManager.I.CurrentState == GameManager.GameFlow.Tutorial)
            {

                TutorialManager.I.ObtainFlower(t);
            }
        }
    }

	private void OnCollisionEnter2D(Collision2D col){


		if (col.gameObject.tag == "Enemy") {
			_killed = true;
		}

        
	}
}
