using System.Collections.Generic;
using System;

/** Contient tous les élements du sol du jeu
 */
public class World {

 	Tile[,] tiles;
	List<Character> characters = new List<Character>();


    public int Width { get; private set; }

    public int Height { get; private set; }

	private Dictionary<string, Furniture> furniturePrototypes;

	private Action<Furniture> cbOnFurniturePlaced;
	private Action<Character> cbOnCharacterCreated;
	private Action<Tile> cbOnTileChanged;

	public JobQueue JobQueue {get; protected set; }

	public World(int width = 100, int heigth=100){
		this.Width = width;
		this.Height = heigth;
		this.JobQueue = new JobQueue();

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
	}

	public void Update(float time){
		foreach (Character character in this.characters)
		{
			character.Update(time);
		}
	}

	void initializeInstalledObjectPrototypes(){
		this.furniturePrototypes = new Dictionary<string, Furniture>();


		this.furniturePrototypes.Add("Wall", Furniture.CreatePrototype("Wall", 1f, 1,1, true));
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

	public void PlaceFurniture(string furnitureId, Tile tile){
		if(this.furniturePrototypes.ContainsKey(furnitureId) == false){
			Logger.LogError("Aucun prototype pour le meuble d'id : "+furnitureId);
			return;
		}

		Furniture furniture = Furniture.PlaceInstance(this.furniturePrototypes[furnitureId], tile);

		if(furniture == null){
			Logger.LogError("Cannot place furniture on tile "+tile);
			return;
		}

		if(this.cbOnFurniturePlaced != null){
			cbOnFurniturePlaced(furniture);
		}
	}

	public void CreateCharacter(Tile t){
		Character character = new Character(t);
		this.characters.Add(character);

		if(this.cbOnCharacterCreated != null){
			this.cbOnCharacterCreated(character);
		}
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
	}
	

	public bool IsFurniturePositionValid(string furnitureId, Tile t){
		return this.furniturePrototypes[furnitureId].IsValidPosition(t);
	}	
	
	public Tile getNeighbourTile(Tile originTile, int vectorX, int vectorY){
		return  WorldController.Instance.WorldData.getTileAt(originTile.X + vectorX, originTile.Y + vectorY);
	}

	public Furniture getFurniturePrototype(string id){
		if(this.furniturePrototypes.ContainsKey(id)){
			return this.furniturePrototypes[id];
		}

		Logger.LogError("Aucun prototype pour le meuble d'id : "+id);
		return null;
	}
}
