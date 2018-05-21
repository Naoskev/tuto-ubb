using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

	private Dictionary<Tile, GameObject> tileGameObjects = new Dictionary<Tile, GameObject>();
	private Dictionary<InstalledObject, GameObject> installedObjectGameObjects = new Dictionary<InstalledObject, GameObject>();

	public Sprite floorSprite;
	public Sprite wallSprite;

	public static WorldController Instance{get; protected set;}
	public World World {get; protected set; }

	// Use this for initialization
	void Start () {
		if(Instance != null)
			Debug.LogError("There should be only one world controller.");
		WorldController.Instance = this;
		this.World = new World();
		this.World.RegisterOnInstalledObjectPlaced(this.OnInstalledObjectPlaced);

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

	public void OnInstalledObjectPlaced(InstalledObject installedObject){		
		GameObject installedObject_go = new GameObject( installedObject.Id +"_at_"+installedObject.MasterTile.X+"_"+installedObject.MasterTile.Y);
		this.installedObjectGameObjects.Add(installedObject, installedObject_go);

		installedObject_go.transform.position = new Vector3Int(installedObject.MasterTile.X,installedObject.MasterTile.Y, 0);
		installedObject_go.transform.SetParent(this.transform, true);

		installedObject_go.AddComponent<SpriteRenderer>().sprite = this.wallSprite;
		installedObject.RegisterOnObjectChangeCallback(OnInstalledObjectChange);
	}

	public void OnInstalledObjectChange(InstalledObject obj){

	}
}
