

/** Contient tous les élements du sol du jeu
 */
public class World {

	Tile[,] tiles;

	int width;
	int heigth;


	public World(int width = 100, int heigth=100){
		this.width = width;
		this.heigth = heigth;

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
		if(x < 0 || x > this.width){
			throw new System.ArgumentOutOfRangeException("x", "La valeur X ("+x+") est en dehors du tableau (max abscisse : "+width+")");			
		}
		if( y < 0 || y> this.heigth){
			throw new System.ArgumentOutOfRangeException("y", "La valeur Y ("+y+") est en dehors du tableau (max ordonnée : "+heigth+")");			
		}
		return this.tiles[x, y];
	}
}
