using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	public GameObject circleCursor;
	private Vector3 lastFrameMousePosition;

	private Vector3? dragMouseStartPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 currFramePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition);
		currFramePosition.z = 0;

		Tile currentTile = this.getTileFromVector(currFramePosition);
		if(currentTile != null){
			circleCursor.SetActive(true);
			circleCursor.transform.position = new Vector3Int(currentTile.X, currentTile.Y, 0);
		}
		else {
			circleCursor.SetActive(false);
		}

		if(Input.GetMouseButtonDown(0)){
			dragMouseStartPosition = currFramePosition;
		}
		if(Input.GetMouseButtonUp(0) && dragMouseStartPosition.HasValue){
			int start_x = Mathf.FloorToInt( dragMouseStartPosition.Value.x);
			int end_x = Mathf.FloorToInt(currFramePosition.x);
			if(start_x > end_x){
				int temp = start_x;
				start_x = end_x;
				end_x = temp;
			}
			int start_y = Mathf.FloorToInt( dragMouseStartPosition.Value.y);
			int end_y = Mathf.FloorToInt(currFramePosition.y);
			if(start_y > end_y){
				int temp = start_y;
				start_y = end_y;
				end_y = temp;
			}

			for (int x = start_x; x <= end_x; x++)
			{
				for (int y = start_y; y <= end_y; y++)
				{
					Tile tileToChange = WorldController.Instance.World.getTileAt(x, y);
					if(tileToChange != null){
						tileToChange.Type = Tile.TileType.Floor;
					}
				}
				
			}

			dragMouseStartPosition = null;
		}
		
		if(Input.GetMouseButton(1) || Input.GetMouseButton(2)){
			Vector3 diff = lastFrameMousePosition - currFramePosition;
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
