using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

	int x;
    int y;

    enum TileType    
    {
        Empty, Floor
    }

    TileType type = TileType.Floor;

    public Tile(int x, int y){
        this.x = x;
        this.y = y;
    }

}
