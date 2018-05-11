using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

	public Sprite floorSprite;

	public static WorldController Instance{get; protected set;}
	public World World {get; protected set; }

	// Use this for initialization
	void Start () {
		if(Instance != null)
			Debug.LogError("There should be only one world controller.");
		WorldController.Instance = this;
		this.World = new World();

		for (int x = 0; x < World.Width; x++)
		{
			for (int y = 0; y < World.Heigth; y++)
			{
				Tile tile_data = World.getTileAt(x,y);
				GameObject tile_go = new GameObject("tile_at_"+x+"_"+y);
				tile_go.transform.position = new Vector3Int(tile_data.X, tile_data.Y, 0);
				tile_go.transform.SetParent(this.transform, true);

				tile_go.AddComponent<SpriteRenderer>();
				tile_data.registerTileTypeChangedCallback((tile)=> OnTileTypeChanged(tile, tile_go));
			}
		}
		World.randomizeTilesType();

		Debug.Log("World created");	
	}
	
	float localTimer = 2f;
	// Update is called once per frame
	void Update () {
		localTimer -= Time.deltaTime;
		if(localTimer < 0){
			//this.world.randomizeTilesType();
			localTimer = 2f;
		}
		
	}

	public void OnTileTypeChanged(Tile tileData, GameObject tileGO){
		if(tileData.Type == Tile.TileType.Floor){
			tileGO.GetComponent<SpriteRenderer>().sprite = floorSprite;
		}
		else if(tileData.Type == Tile.TileType.Empty)
		{
			tileGO.GetComponent<SpriteRenderer>().sprite = null;			
		}
		else {
			Debug.Log("Unknown tile type : "+tileData.Type+" for tile ["+tileData.X+";"+tileData.Y+"]");
		}
	}

	public Tile getTileFromVector(Vector3 coord){
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);
		
		return WorldController.Instance.World.getTileAt(x, y);
	}
}
