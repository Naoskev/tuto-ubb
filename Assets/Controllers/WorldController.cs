using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

	public Sprite floorSprite;

	World world;

	// Use this for initialization
	void Start () {
		this.world = new World();

		for (int x = 0; x < world.Width; x++)
		{
			for (int y = 0; y < world.Heigth; y++)
			{
				Tile tile_data = world.getTileAt(x,y);
				GameObject tile_go = new GameObject("tile_at_"+x+"_"+y);
				tile_go.transform.position = new Vector3Int(tile_data.X, tile_data.Y, 0);
				tile_go.transform.SetParent(tile_go.transform, true);

				tile_go.AddComponent<SpriteRenderer>();
				tile_data.registerTileTypeChangedCallback((tile)=> OnTileTypeChanged(tile, tile_go));
			}
		}
		world.randomizeTilesType();

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
}
