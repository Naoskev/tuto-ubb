using System;
using System.Collections.Generic;

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


    public Dictionary<string, object> furnitureParameters;
    public Action<Furniture, float> updateAction;


    public void Update(float deltaTime){
        if(this.updateAction != null){
            this.updateAction(this, deltaTime);
        }
    }

    public bool IsValidPosition(Tile t){
        return this._isValidPosition(t);
    }

    protected Furniture(){
        this.furnitureParameters = new Dictionary<string, object>();
    }

    protected Furniture(Furniture other){
        this.Id = other.Id;
        this.MovementCost = other.MovementCost;
        this.Width = other.Width;
        this.Height = other.Height;
        this.IsConnected = other.IsConnected;
        this._isValidPosition = other._isValidPosition;
        this.furnitureParameters = new Dictionary<string, object>();
        if(other.updateAction != null){
            this.updateAction = (Action<Furniture, float>) other.updateAction.Clone();
        }
    }

    public virtual Furniture Clone(){
        return new Furniture(this);
    }

    // Create furniture from parameter : only for prototypes
    public Furniture(string id, float movementCost = 1f, int width = 1, int height = 1, bool isConnected = false){
        this.Id = id;
        this.MovementCost = movementCost;
        this.Width = width;
        this.Height = height;
        this.IsConnected = isConnected;

        this._isValidPosition = this.isValidPosition_Base;
        this.furnitureParameters = new Dictionary<string, object>();
    }

    public static Furniture PlaceInstance(Furniture proto, Tile tile){
        if(proto._isValidPosition(tile) == false){
            return null;
        }
        Furniture obj = proto.Clone();
        // TODO : gérer les objets sur plusieurs tuiles
        obj.MasterTile = tile;

        if(tile.PlaceFurniture(obj) == false){
            return null;
        }

        // SI on a une connection avec les voisins du même type
        // On les notifie pour qu'ils se mettent à jour
        if(obj.IsConnected){
            foreach (var neighbour in FurnitureUtility.GetCardinalNeighboords(obj.Id, obj.MasterTile))
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
