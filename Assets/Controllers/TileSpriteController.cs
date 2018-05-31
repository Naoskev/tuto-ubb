﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteController : MonoBehaviour {

	private Dictionary<Tile, GameObject> tileGameObjects = new Dictionary<Tile, GameObject>();

	public Sprite floorSprite;

	private World _world {get{
		return WorldController.Instance.WorldData;
	} }

	// Use this for initialization
	void Start () {		

		for (int x = 0; x < this._world.Width; x++)
		{
			for (int y = 0; y < this._world.Heigth; y++)
			{
				Tile tile_data = this._world.getTileAt(x,y);
				GameObject tile_go = new GameObject("tile_at_"+x+"_"+y);
				tileGameObjects.Add(tile_data, tile_go);

				tile_go.transform.position = new Vector3Int(tile_data.X, tile_data.Y, 0);
				tile_go.transform.SetParent(this.transform, true);

				tile_go.AddComponent<SpriteRenderer>();
			}
		}
		
		this._world.RegisterOnTileChanged(OnTileTypeChanged);		
	}

	public void OnTileTypeChanged(Tile tile_data){
		if(this.tileGameObjects.ContainsKey(tile_data) == false) {
			Debug.LogError("tileGameObjects doesn't contain the tile_data -- did you forget to add the tile to the dictionary? Or maybe forget to unregister a callback?");
			return;
		}
		GameObject tile_go = this.tileGameObjects[tile_data];
		if(tile_go == null){
			Debug.LogError("Missing game object for tile "+tile_data);
			return;
		}

		if(tile_data.Type == TileType.Floor){
			tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
		}
		else if(tile_data.Type == TileType.Empty)
		{
			tile_go.GetComponent<SpriteRenderer>().sprite = null;			
		}
		else {
			Debug.Log("Unknown tile type : "+tile_data.Type+" for tile ["+tile_data.X+";"+tile_data.Y+"]");
		}
	}
}