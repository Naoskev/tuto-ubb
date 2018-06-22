
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
        bool isCardinal = XDistance + YDistance ==  1;
        if(onlyCardinal){
            return isCardinal;  
        }
        return isCardinal || XDistance + YDistance == 2;
    }

    public Tile[] GetNeighbours(bool diagOk){
        Tile[] result;
        if(diagOk == true){
            result = new Tile[8];
        }
        else {            
            result = new Tile[4];
        }

        result[0] = WorldController.Instance.WorldData.getNeighbourTile(this, 0, 1);
        result[2] = WorldController.Instance.WorldData.getNeighbourTile(this, 1, 0);
        result[4] = WorldController.Instance.WorldData.getNeighbourTile(this, 0, -1);
        result[6] = WorldController.Instance.WorldData.getNeighbourTile(this, -1, 0);

        if(diagOk){            
            result[1] = WorldController.Instance.WorldData.getNeighbourTile(this, 1, 1);
            result[3] = WorldController.Instance.WorldData.getNeighbourTile(this, 1, -1);
            result[5] = WorldController.Instance.WorldData.getNeighbourTile(this, -1, -1);
            result[7] = WorldController.Instance.WorldData.getNeighbourTile(this, -1, 1);
        }
        return result;
    }
}
