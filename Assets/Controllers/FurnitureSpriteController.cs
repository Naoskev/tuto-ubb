﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSpriteController : MonoBehaviour {

	private Dictionary<Furniture, GameObject> furnitureGameObjects = new Dictionary<Furniture, GameObject>();

	private Dictionary<string, Sprite> furnitureSprites = new Dictionary<string, Sprite>();

	private World _world {get{ return WorldController.Instance.WorldData; } }

	// Use this for initialization
	void Start () {
		Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Furnitures");
		foreach (Sprite sprite in sprites)
		{
			this.furnitureSprites.Add(sprite.name, sprite);
		}

		this._world.RegisterOnFurniturePlaced(this.OnFurniturePlaced);
	}

	public void OnFurniturePlaced(Furniture furniture){		
		GameObject furn_go = new GameObject( furniture.Id +"_at_"+furniture.MasterTile.X+"_"+furniture.MasterTile.Y);
		this.furnitureGameObjects.Add(furniture, furn_go);

		furn_go.transform.position = new Vector3Int(furniture.MasterTile.X,furniture.MasterTile.Y, 0);
		furn_go.transform.SetParent(this.transform, true);

		furn_go.AddComponent<SpriteRenderer>().sprite = this.getFurnitureSprite(furniture);
		furniture.RegisterOnObjectChangeCallback(OnFurnitureChange);
	}

	private Sprite getFurnitureSprite(Furniture furniture){
		string spriteName = furniture.Id;
		if(furniture.IsConnected){
			spriteName += FurnitureUtility.GetFurnitureSpriteName(furniture);
		}
		if(this.furnitureSprites.ContainsKey(spriteName) == false){
			Debug.LogError("Aucune sprite pour "+spriteName);
			return null;
		}
		
		return this.furnitureSprites[spriteName];
	}

	public void OnFurnitureChange(Furniture furniture){

		if(this.furnitureGameObjects.ContainsKey(furniture) == false){
			Debug.LogError("Aucun gameObject pour "+furniture.Id);
			return;
		}

		GameObject io_go = this.furnitureGameObjects[furniture];
		io_go.GetComponent<SpriteRenderer>().sprite = this.getFurnitureSprite(furniture);

	}
}
