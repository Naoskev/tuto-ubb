using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	enum MouseButton{
		LeftClick = 0,
		RightClick = 1,
		MiddleClick = 2

	}

	public GameObject circleCursor;
	private Vector3 lastFrameMousePosition;
	private Vector3 currFramePosition;

	private Vector3? dragMouseStartPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currFramePosition = this.getMousePosition();

		// this.showCursor();
		this.dragAndDropTiles();
		this.updateCameraPosition();		


		lastFrameMousePosition = this.getMousePosition();
	}

	// private void showCursor(){
	// 	Tile currentTile = WorldController.Instance.getTileFromVector(currFramePosition);
	// 	if(currentTile != null){
	// 		circleCursor.SetActive(true);
	// 		circleCursor.transform.position = new Vector3Int(currentTile.X, currentTile.Y, 0);
	// 	}
	// 	else {
	// 		circleCursor.SetActive(false);
	// 	}
	// }

	private void updateCameraPosition(){		
		if(Input.GetMouseButton((int)MouseButton.RightClick) || Input.GetMouseButton((int)MouseButton.MiddleClick)){
			Vector3 diff = lastFrameMousePosition - currFramePosition;
			Camera.main.transform.Translate(diff);
		}
	}

	private void dragAndDropTiles(){		
		if(Input.GetMouseButtonDown((int)MouseButton.LeftClick)){
			dragMouseStartPosition = currFramePosition;
		}
		if(Input.GetMouseButtonUp((int)MouseButton.LeftClick) && dragMouseStartPosition.HasValue){
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
	}


	private Vector3 getMousePosition(){
		Vector3 vector = Camera.main.ScreenToWorldPoint( Input.mousePosition);
		vector.z = 0;
		return vector;
	}

}
