using System;
using System.Collections.Generic;
using UnityEngine;

public class TileUtility{
    
	public static readonly Vector2Int[] allNeighbours = new Vector2Int[]{
		new Vector2Int(0, 1), 
		new Vector2Int(1, 1),
		new Vector2Int(1, 0), 
		new Vector2Int(1, -1), 
		new Vector2Int(0, -1), 
		new Vector2Int(-1, -1),
		new Vector2Int(-1, 0),
		new Vector2Int(-1, 1)
	};

	public static readonly Vector2Int[] cardinalNeighbours = new Vector2Int[]{
		allNeighbours[0], allNeighbours[2], allNeighbours[4], allNeighbours[6]
	};
}