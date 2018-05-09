/** Elmeent atomique represantant le sol
 */
public class Tile {

    public int X { get; private set; }

    public int Y { get; private set; }

    World world;

    public enum TileType    
    {
        Empty, Floor
    }

    private TileType type = TileType.Empty;
    public TileType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    LooseObject looseObject;
    InstaledObject installedObject;



    public Tile(World world, int x, int y){
        this.X = x;
        this.Y = y;
        this.world = world;
    }

}
