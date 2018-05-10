

/** Contient tous les élements du sol du jeu
 */
public class World {

 	Tile[,] tiles;

    public int Width { get; private set; }

    public int Heigth { get; private set; }


	public World(int width = 100, int heigth=100){
		this.Width = width;
		this.Heigth = heigth;

		this.tiles = new Tile[width, heigth];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < heigth; y++)
			{
				this.tiles[x,y] = new Tile(this, x, y);
			}
		}
	}


    public Tile getTileAt(int x, int y){
		if(x < 0 || x > this.Width || y < 0 || y> this.Heigth){
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

	public void randomizeTilesType(){
		foreach (var tile in this.tiles)
		{
			if(random.Next(0,2) == 0){
				tile.Type = Tile.TileType.Empty;
			}
			else{
				tile.Type = Tile.TileType.Floor;
			}
		}
	}
}
