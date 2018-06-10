
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

    public Job pendingFurnitureJob;

    public float MovementCost {
        get {
            if(this.Type == TileType.Empty) return 0;

            if(this.Furniture == null) return 1;

            return 1 * this.Furniture.MovementCost;
        }
    }



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

    public bool IsNeighbourWith(Tile otherTile, bool onlyCardinal){
        int XDistance = Math.Abs(this.X - otherTile.X), YDistance = Math.Abs(this.Y - otherTile.Y);

        if(onlyCardinal){
            return (XDistance == 1 && YDistance == 0) || (XDistance == 0 && YDistance == 1);  
        }
        return XDistance <= 0 && YDistance <= 1;
    }
}
