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

		Tile tile = this.getTileFromVector(currMouseCoordinate);
		if(tile != null){
			circleCursor.SetActive(true);
			circleCursor.transform.position = new Vector3Int(tile.X, tile.Y, 0);
		}
		else {
			circleCursor.SetActive(false);
		}
		
		if(Input.GetMouseButton(1) || Input.GetMouseButton(2)){
			Vector3 diff = lastFrameMousePosition - currMouseCoordinate;
			Camera.main.transform.Translate(diff);
		}

		lastFrameMousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition);
		lastFrameMousePosition.z =0;
	}

	Tile getTileFromVector(Vector3 coord){
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);
		
		return WorldController.Instance.World.getTileAt(x, y);
	}
}
