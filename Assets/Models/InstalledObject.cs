/** Objet fixe */
public class InstalledObject {

    string Id {get; private set;};

    int Width {get; protected set; };
    int Height {get; protected set; };

    float MovementCost {get; protected set;};

    Tile masterTile;

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
        obj.masterTile = tile;

        if(tile.PlaceInstalledObject(obj) == false){
            return null;
        }

        return obj;
    }
}
