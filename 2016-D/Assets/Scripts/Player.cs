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
			UiManager.I.SetHandle(Point, Dir);
		}
	}

	private bool isActive = false;

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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Pollen")
		{
			Pollen p = collision.gameObject.GetComponent<Pollen>();

			FieldManager.I.ObtainPollen(p.Value);
		}
	}

}
