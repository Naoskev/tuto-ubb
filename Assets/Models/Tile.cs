/** Elmeent atomique represantant le sol
 */
public class Tile {

	int x;
    int y;

    World world;

    enum TileType    
    {
        Empty, Floor
    }

    TileType type = TileType.Empty;

    LooseObject looseObject;
    InstaledObject installedObject;

    public Tile(World world, int x, int y){
        this.x = x;
        this.y = y;
        this.world = world;
    }

}
