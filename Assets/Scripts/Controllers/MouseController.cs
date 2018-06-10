using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

	enum MouseButton{
		LeftClick = 0,
		RightClick = 1,
		MiddleClick = 2

	}

	public GameObject circleCursorPrefab;
	private Vector3 lastFrameMousePosition;
	private Vector3 currFramePosition;

	private Vector3? dragMouseStartPosition;
	private List< GameObject> dragAndDropPreviewObjects;


	// Use this for initialization
	void Start () {
		this.dragAndDropPreviewObjects = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		currFramePosition = this.getMousePosition();

		// this.showCursor();
		this.dragAndDropTiles();
		this.updateCameraPosition();		


		lastFrameMousePosition = this.getMousePosition();
	}

	private void updateCameraPosition(){		
		if(Input.GetMouseButton((int)MouseButton.RightClick) || Input.GetMouseButton((int)MouseButton.MiddleClick)){
			Vector3 diff = lastFrameMousePosition - currFramePosition;
			Camera.main.transform.Translate(diff);
		}

		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
	}

	private void dragAndDropTiles(){	
		if(EventSystem.current.IsPointerOverGameObject()){
			return;
		}

		if(Input.GetMouseButtonDown((int)MouseButton.LeftClick)){
			dragMouseStartPosition = currFramePosition;
		}
		if(dragMouseStartPosition.HasValue){
			int start_x = Mathf.FloorToInt( dragMouseStartPosition.Value.x);
			int end_x = Mathf.FloorToInt(currFramePosition.x);
			int start_y = Mathf.FloorToInt( dragMouseStartPosition.Value.y);
			int end_y = Mathf.FloorToInt(currFramePosition.y);

			if(start_x > end_x){
				int temp = start_x;
				start_x = end_x;
				end_x = temp;
			}
			if(start_y > end_y){
				int temp = start_y;
				start_y = end_y;
				end_y = temp;
			}

			this.dragAndDropPreviewObjects.ForEach((ob) => SimplePool.Despawn(ob));
			this.dragAndDropPreviewObjects.Clear();

			if(Input.GetMouseButton((int)MouseButton.LeftClick)){
				for (int x = start_x; x <= end_x; x++)
				{
					for (int y = start_y; y <= end_y; y++)
					{
						Tile tileToChange = WorldController.Instance.WorldData.getTileAt(x, y);
						if(tileToChange != null){
							GameObject circleGO = SimplePool.Spawn(circleCursorPrefab, new Vector3(x,y,0), Quaternion.identity);
							circleGO.transform.SetParent(this.transform, true);
							this.dragAndDropPreviewObjects.Add(circleGO);
						}
					}					
				}
			}

			if(Input.GetMouseButtonUp((int)MouseButton.LeftClick)){

				BuildModeController bmc = GameObject.FindObjectOfType<BuildModeController>();
				for (int x = start_x; x <= end_x; x++)
				{
					for (int y = start_y; y <= end_y; y++)
					{
						Tile tileToChange = WorldController.Instance.WorldData.getTileAt(x, y);
						if(tileToChange != null){
							bmc.DoBuild(tileToChange);
						}
					}
					
				}
				dragMouseStartPosition = null;
			}
		}
	}


	private Vector3 getMousePosition(){
		Vector3 vector = Camera.main.ScreenToWorldPoint( Input.mousePosition);
		vector.z = 0;
		return vector;
	}
}
