using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
	public static WorldController Instance{get; protected set;}
	public World WorldData {get; protected set; }

	// Before any game object start
	void OnEnable () {
		if(Instance != null)
			Debug.LogError("There should be only one world controller.");
		WorldController.Instance = this;
		this.WorldData = new World();

		int worldMiddleX = WorldData.Width / 2, worldMiddleY = WorldData.Heigth / 2, spawnArea = 5;
		// for (int x = worldMiddleX - spawnArea; x < worldMiddleX + spawnArea; x++)
		// {
		// 	for (int y = worldMiddleY - spawnArea; y < worldMiddleY + spawnArea; y++)
		// 	{
		// 		WorldData.getTileAt(x, y).Type = TileType.Floor;
		// 	}
		// }

		Camera.main.transform.position = new Vector3(worldMiddleX, worldMiddleY, Camera.main.transform.position.z);

		Debug.Log("World created");	
	}

	void Update(){
		this.WorldData.Update(Time.deltaTime);
	}
	
	public Tile getTileFromVector(Vector3 coord){
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);
		
		return WorldController.Instance.WorldData.getTileAt(x, y);
	}
}
