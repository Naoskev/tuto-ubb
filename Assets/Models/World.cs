using System.Collections.Generic;
using System;

/** Contient tous les élements du sol du jeu
 */
public class World {

 	Tile[,] tiles;
	List<Character> characters = new List<Character>();


    public int Width { get; private set; }

    public int Heigth { get; private set; }

	private Dictionary<string, Furniture> furniturePrototypes;

	private Action<Furniture> cbOnFurniturePlaced;
	private Action<Character> cbOnCharacterCreated;
	private Action<Tile> cbOnTileChanged;

	public JobQueue JobQueue {get; protected set; }

	public World(int width = 100, int heigth=100){
		this.Width = width;
		this.Heigth = heigth;
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
	}

	void initializeInstalledObjectPrototypes(){
		this.furniturePrototypes = new Dictionary<string, Furniture>();


		this.furniturePrototypes.Add("Wall", Furniture.CreatePrototype("Wall", 1f, 1,1, true));
	}


    public Tile getTileAt(int x, int y){
		if(x < 0 || x >= this.Width || y < 0 || y>= this.Heigth){
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

	public void PlaceInstalledObject(string objectType, Tile tile){
		if(this.furniturePrototypes.ContainsKey(objectType) == false){
			return;
		}

		Furniture obj = Furniture.PlaceInstance(this.furniturePrototypes[objectType], tile);

		if(obj == null){
			return;
		}

		if(this.cbOnFurniturePlaced != null){
			cbOnFurniturePlaced(obj);
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

		// TODO : log
		return null;
	}
}
