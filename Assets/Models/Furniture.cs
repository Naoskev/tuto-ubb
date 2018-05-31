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

    Func<Tile, bool> _isValidPosition;

    public bool IsValidPosition(Tile t){
        return this._isValidPosition(t);
    }

    protected Furniture(){
    }

    public static Furniture CreatePrototype(string id, float movementCost = 1f, int width = 1, int height = 1, bool isConnected = false){
        Furniture obj = new Furniture();
        obj.Id = id;
        obj.MovementCost = movementCost;
        obj.Width = width;
        obj.Height = height;
        obj.IsConnected = isConnected;

        obj._isValidPosition = obj.isValidPosition_Base;
        return obj;
    }

    public static Furniture PlaceInstance(Furniture proto, Tile tile){
        if(proto._isValidPosition(tile) == false){
            return null;
        }
        Furniture obj = new Furniture();
        obj.Id = proto.Id;
        obj.MovementCost = proto.MovementCost;
        obj.Width = proto.Width;
        obj.Height = proto.Height;
        obj.IsConnected = proto.IsConnected;
        obj._isValidPosition = proto._isValidPosition;

        // TODO : gérer les objets sur plusieurs tuiles
        obj.MasterTile = tile;

        if(tile.PlaceFurniture(obj) == false){
            return null;
        }

        // SI on a une connection avec les voisins du même type
        // On les notifie pour qu'ils se mettent à jour
        if(obj.IsConnected){
            foreach (var neighbour in FurnitureUtility.GetCardinalNeighboords(obj))
            {
                neighbour.Value.cbOnChanged(neighbour.Value);
            }
        }

        return obj;
    }

    public void RegisterOnObjectChangeCallback(Action<Furniture> callback){
        this.cbOnChanged += callback;
    }
    public void UnregisterOnObjectChangeCallback(Action<Furniture> callback){
        this.cbOnChanged -= callback;
    }

    private bool isValidPosition_Base(Tile tile){
        if(tile.Type != TileType.Floor) return false;

        if(tile.Furniture != null) return false;

        return true;
    }

    private bool isValidPosition_Door(Tile tile){
        if(this.isValidPosition_Base(tile) == false) return false;

        // TODO

        return true;
    }
}
