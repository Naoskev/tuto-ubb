using System.Collections.Generic;
using System;

/** Contient tous les élements du sol du jeu
 */
public class World {

 	Tile[,] tiles;
	public List<Character> Characters { get; protected set; }
	public List<Furniture> Furnitures { get; protected set; }


    public int Width { get; private set; }

    public int Height { get; private set; }

	private Dictionary<string, Furniture> furniturePrototypes;

	private Action<Furniture> cbOnFurniturePlaced;
	private Action<Character> cbOnCharacterCreated;
	private Action<Tile> cbOnTileChanged;

	public JobQueue JobQueue {get; protected set; }

	private Path_TileGraph _tileGraph;
	public Path_TileGraph TileGraph {  
		get{
			if(this._tileGraph == null){
				this._tileGraph = new Path_TileGraph(this);
			}
			return this._tileGraph;
	} }

	public World(int width = 100, int heigth=100, bool empty = false){
		this.Width = width;
		this.Height = heigth;
		this.JobQueue = new JobQueue();
		this.Furnitures = new List<Furniture>();
		this.Characters = new List<Character>();

		this.tiles = new Tile[width, heigth];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < heigth; y++)
			{
				this.tiles[x,y] = new Tile(this, x, y);
				tiles[x,y].RegisterTileTypeChangedCallback( OnTileChanged );
			}
		}

		this.initializeInstalledObjectPrototypes();

		// this.SetupPathfindingExample();
		if(empty == false){
			this.CreateCharacter(this.getTileAt(this.Width / 2, this.Height / 2));
		}
	}

	public void Update(float time){
		foreach (Character character in this.Characters)
		{
			character.Update(time);
		}
	}

	void initializeInstalledObjectPrototypes(){
		this.furniturePrototypes = new Dictionary<string, Furniture>();


		this.furniturePrototypes.Add("Wall", Furniture.CreatePrototype("Wall", 0, 1,1, true));
	}


    public Tile getTileAt(int x, int y){
		if(x < 0 || x >= this.Width || y < 0 || y>= this.Height){
			System.Console.Error.WriteLine( "Tile ["+x+";"+y+"] does not exist.");
			return null;			
		}
		// if(x < 0 || x > this.Width){
		// 	throw new System.ArgumentOutOfRangeException("x", "La valeur X ("+x+") est en dehors du tableau (max abscisse : "+Width+")");			
		// }
		// if( y < 0 || y> this.Heigth){
		// 	throw new System.ArgumentOutOfRangeException("y", "La valeur Y ("+y+") est en dehors du tableau (max ordonnée : "+Heigth+")");			
		// }
		return this.tiles[x, y];
	}

	public static System.Random random = new System.Random();

	// public void randomizeTilesType(){
	// 	foreach (var tile in this.tiles)
	// 	{
	// 		if(random.Next(0,2) == 0){
	// 			tile.Type = TileType.Empty;
	// 		}
	// 		else{
	// 			tile.Type = TileType.Floor;
	// 		}
	// 	}
	// }

	public void SetupPathfindingExample() {
		Logger.LogInfo ("SetupPathfindingExample");

		// Make a set of floors/walls to test pathfinding with.
		int l = Width / 2 - 5;
		int b = Height / 2 - 5;

		for (int x = l-5; x < l + 15; x++) {
			for (int y = b-5; y < b + 15; y++) {
				tiles[x,y].Type = TileType.Floor;

				if(x == l || x == (l + 9) || y == b || y == (b + 9)) {
					if(x != (l + 9) && y != (b + 4)) {
						PlaceFurniture("Wall", tiles[x,y]);
					}
				}
			}
		}
	}

	public Furniture PlaceFurniture(string furnitureId, Tile tile){
		if(this.furniturePrototypes.ContainsKey(furnitureId) == false){
			Logger.LogError("Aucun prototype pour le meuble d'id : "+furnitureId);
			return null;
		}

		Furniture furniture = Furniture.PlaceInstance(this.furniturePrototypes[furnitureId], tile);

		if(furniture == null){
			Logger.LogError("Cannot place furniture on tile "+tile);
			return null;
		}
		this.Furnitures.Add(furniture);

		if(this.cbOnFurniturePlaced != null){
			cbOnFurniturePlaced(furniture);
			this.invalidTileGraph();
		}
		return furniture;
	}

	public Character CreateCharacter(Tile t){
		Character character = new Character(t);
		this.Characters.Add(character);

		if(this.cbOnCharacterCreated != null){
			this.cbOnCharacterCreated(character);
		}

		return character;
	}

	public void RegisterOnFurniturePlaced(Action<Furniture> callback){
		this.cbOnFurniturePlaced += callback;
	}
	
	public void UnregisterOnFurniturePlaced(Action<Furniture> callback){
		this.cbOnFurniturePlaced -= callback;
	}

	public void RegisterOnCharacterCreated(Action<Character> callback){
		this.cbOnCharacterCreated += callback;
	}
	
	public void UnregisterOnCharacterCreated(Action<Character> callback){
		this.cbOnCharacterCreated -= callback;
	}
	

	public void RegisterOnTileChanged(Action<Tile> callback){
		this.cbOnTileChanged += callback;
	}
	
	public void UnregisterOnTileChanged(Action<Tile> callback){
		this.cbOnTileChanged -= callback;
	}
	

	void OnTileChanged(Tile t) {
		if(this.cbOnTileChanged == null)
			return;
		
		this.cbOnTileChanged(t);
		this.invalidTileGraph();
	}
	
	private void invalidTileGraph(){
		this._tileGraph = null;
	}

	public bool IsFurniturePositionValid(string furnitureId, Tile t){
		return this.furniturePrototypes[furnitureId].IsValidPosition(t);
	}	
	
	public Tile getNeighbourTile(Tile originTile, int vectorX, int vectorY){
		return  this.getTileAt(originTile.X + vectorX, originTile.Y + vectorY);
	}

	public Furniture getFurniturePrototype(string id){
		if(this.furniturePrototypes.ContainsKey(id)){
			return this.furniturePrototypes[id];
		}

		Logger.LogError("Aucun prototype pour le meuble d'id : "+id);
		return null;
	}

	public void ApplyToTiles(Action<Tile> callback){
		foreach (Tile tile in this.tiles)	
		{
			callback(tile);	
		}
	}
}
