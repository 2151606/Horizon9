using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	Camera cam;

	[SerializeField] private Transform player;

	private float PlayerOffsetFromCenter;

	private float minSize;

	private float maxSize;

	private float t;

	private Vector3 CameraOffset;

	// Use this for initialization
	void Start () {
		CameraOffset = transform.position - player.position;
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

		PlayerOffsetFromCenter = Mathf.Abs (player.position.y);



		t = Mathf.InverseLerp (0, 5, PlayerOffsetFromCenter);

		cam.orthographicSize = Mathf.Lerp (5, 6, t);



		//Vector3 screenPos = cam.WorldToScreenPoint(player.transform.position);

		//if (screenPos.y > (Screen.height) || screenPos.y < 0)
		//	cam.orthographicSize +=1;
	}

	void LateUpdate () 
	{
		transform.position = Vector3.Scale (player.position, Vector3.right) + CameraOffset;
	}
}
