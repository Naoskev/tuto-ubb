
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
    InstalledObject installedObject;



    public Tile(World world, int x, int y){
        this.X = x;
        this.Y = y;
        this.world = world;
    }

    public void registerTileTypeChangedCallback(Action<Tile> callback){
        this.cbTileTypeChanged += callback;
    }

    public void unregisterTileTypeChangedCallback(Action<Tile> callback){
        this.cbTileTypeChanged -= callback;
    }

    public bool PlaceInstalledObject(InstalledObject installedObject){
        if(installedObject == null){
            this.installedObject = null;
            return true;
        }

        if(this.installedObject != null){
            return false;
        }


        this.installedObject = installedObject;
        return true;
    }
}
