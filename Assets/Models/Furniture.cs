using System;

/** Objet fixe */
public class Furniture {

    public string Id {get; protected set;}

    public int Width {get; protected set; }
    public int Height {get; protected set; }

    public float MovementCost {get; protected set;}

    public Tile MasterTile {get; protected set; }

    public bool IsConnected {get; protected set;}

    Action<Furniture> cbOnChanged;

    protected Furniture(){

    }

    public static Furniture CreatePrototype(string id, float movementCost = 1f, int width = 1, int height = 1, bool isConnected = false){
        Furniture obj = new Furniture();
        obj.Id = id;
        obj.MovementCost = movementCost;
        obj.Width = width;
        obj.Height = height;
        obj.IsConnected = isConnected;
        return obj;
    }

    public static Furniture PlaceInstance(Furniture proto, Tile tile){
        Furniture obj = new Furniture();
        obj.Id = proto.Id;
        obj.MovementCost = proto.MovementCost;
        obj.Width = proto.Width;
        obj.Height = proto.Height;
        obj.IsConnected = proto.IsConnected;

        // TODO : gérer les objets sur plusieurs tuiles
        obj.MasterTile = tile;

        if(tile.PlaceFurniture(obj) == false){
            return null;
        }

        if(obj.IsConnected){
            
        }

        return obj;
    }

    public void RegisterOnObjectChangeCallback(Action<Furniture> callback){
        this.cbOnChanged += callback;
    }
    public void UnregisterOnObjectChangeCallback(Action<Furniture> callback){
        this.cbOnChanged -= callback;
    }
}
