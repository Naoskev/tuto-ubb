using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	public GameObject circleCursor;
	private Vector3 lastFrameMousePosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 currMouseCoordinate = Camera.main.ScreenToWorldPoint( Input.mousePosition);
		currMouseCoordinate.z = 0;

		circleCursor.transform.position = currMouseCoordinate;
		
		if(Input.GetMouseButton(1) || Input.GetMouseButton(2)){
			Vector3 diff = lastFrameMousePosition - currMouseCoordinate;
			Camera.main.transform.Translate(diff);
		}

		lastFrameMousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition);
		lastFrameMousePosition.z =0;
	}
}
