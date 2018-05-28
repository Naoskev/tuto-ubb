
using System;
/** Element atomique represantant le sol
*/
public enum TileType    
{
    Empty, Floor
}
public class Tile {

    public int X { get; private set; }

    public int Y { get; private set; }

    World world;

    private Action<Tile> cbTileTypeChanged;


    private TileType type = TileType.Empty;
    public TileType Type
    {
        get
        {
            return type;
        }

        set
        {
            TileType oldType = type;
            type = value;
            if(cbTileTypeChanged != null && type != oldType)
                cbTileTypeChanged(this);
        }
    }

    LooseObject looseObject;
    public Furniture Furniture {get; protected set;}



    public Tile(World world, int x, int y){
        this.X = x;
        this.Y = y;
        this.world = world;
    }

    public void RegisterTileTypeChangedCallback(Action<Tile> callback){
        this.cbTileTypeChanged += callback;
    }

    public void UnregisterTileTypeChangedCallback(Action<Tile> callback){
        this.cbTileTypeChanged -= callback;
    }

    public bool PlaceFurniture(Furniture furniture){
        if(furniture == null){
            this.Furniture = null;
            return true;
        }

        if(this.Furniture != null){
            return false;
        }


        this.Furniture = furniture;
        return true;
    }
}
