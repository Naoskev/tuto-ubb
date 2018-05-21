using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

	private Dictionary<Tile, GameObject> tileGameObjects = new Dictionary<Tile, GameObject>();
	private Dictionary<Furniture, GameObject> furnitureGameObjects = new Dictionary<Furniture, GameObject>();

	private Dictionary<string, Sprite> furnitureSprites = new Dictionary<string, Sprite>();

	public Sprite floorSprite;

	public static WorldController Instance{get; protected set;}
	public World World {get; protected set; }

	// Use this for initialization
	void Start () {
		Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Furnitures");
		foreach (Sprite sprite in sprites)
		{
			this.furnitureSprites.Add(sprite.name, sprite);
		}

		if(Instance != null)
			Debug.LogError("There should be only one world controller.");
		WorldController.Instance = this;
		this.World = new World();
		this.World.RegisterOnInstalledObjectPlaced(this.OnFurniturePlaced);

		for (int x = 0; x < World.Width; x++)
		{
			for (int y = 0; y < World.Heigth; y++)
			{
				Tile tile_data = World.getTileAt(x,y);
				GameObject tile_go = new GameObject("tile_at_"+x+"_"+y);
				tileGameObjects.Add(tile_data, tile_go);

				tile_go.transform.position = new Vector3Int(tile_data.X, tile_data.Y, 0);
				tile_go.transform.SetParent(this.transform, true);

				tile_go.AddComponent<SpriteRenderer>();
				tile_data.registerTileTypeChangedCallback(OnTileTypeChanged);
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

	public Tile getTileFromVector(Vector3 coord){
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);
		
		return WorldController.Instance.World.getTileAt(x, y);
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
