using System;

/** Objet fixe */
public class InstalledObject {

    public string Id {get; protected set;}

    public int Width {get; protected set; }
    public int Height {get; protected set; }

    public float MovementCost {get; protected set;}

    public Tile MasterTile {get; protected set; }

    Action<InstalledObject> cbOnChanged;

    protected InstalledObject(){

    }

    public static InstalledObject CreatePrototype(string id, float movementCost = 1f, int width = 1, int height = 1){
        InstalledObject obj = new InstalledObject();
        obj.Id = id;
        obj.MovementCost = movementCost;
        obj.Width = width;
        obj.Height = height;
        return obj;
    }

    public static InstalledObject PlaceInstance(InstalledObject proto, Tile tile){
        InstalledObject obj = new InstalledObject();
        obj.Id = proto.Id;
        obj.MovementCost = proto.MovementCost;
        obj.Width = proto.Width;
        obj.Height = proto.Height;

        // TODO : gérer les objets sur plusieurs tuiles
        obj.MasterTile = tile;

        if(tile.PlaceInstalledObject(obj) == false){
            return null;
        }

        return obj;
    }

    public void RegisterOnobjectChangeCallback(Action<InstalledObject> callback){
        this.cbOnChanged += callback;
    }
    public void UnregisterOnobjectChangeCallback(Action<InstalledObject> callback){
        this.cbOnChanged -= callback;
    }
}
