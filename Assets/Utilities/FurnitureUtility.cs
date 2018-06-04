using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FurnitureUtility {
	private static readonly Vector2Int[] allNeighbours = new Vector2Int[]{
		new Vector2Int(0, 1), 
		new Vector2Int(1, 1),
		new Vector2Int(1, 0), 
		new Vector2Int(1, -1), 
		new Vector2Int(0, -1), 
		new Vector2Int(-1, -1),
		new Vector2Int(-1, 0),
		new Vector2Int(-1, 1)
	};

	private static readonly Vector2Int[] cardinalNeighbours = new Vector2Int[]{
		allNeighbours[0], allNeighbours[2], allNeighbours[4], allNeighbours[6]
	};

	private static readonly Dictionary<Vector2Int, string> angles = new Dictionary<Vector2Int,string>(){
		{allNeighbours[0], "N"},
		// {1, new KeyValuePair<Vector2Int, string>(new Vector2Int(1, 1), "NE")},
		{allNeighbours[2], "E"},
		// {3, new KeyValuePair<Vector2Int, string>(new Vector2Int(1, -1), "SE")},
		{allNeighbours[4], "S"},
		// {5, new KeyValuePair<Vector2Int, string>(new Vector2Int(-1, -1), "SW")},
		{allNeighbours[6], "W"},
		// {7, new KeyValuePair<Vector2Int, string>(new Vector2Int(-1, 1), "NW")},
	};

	private static KeyValuePair<Vector2Int, Furniture>[] GetNeighboords(string furnitureId, Tile originTile, Vector2Int[] directionToGet){
		Queue<KeyValuePair<Vector2Int, Furniture>> queue = new Queue<KeyValuePair<Vector2Int, Furniture>>();

		foreach (Vector2Int direction in directionToGet)
		{
			Tile tile = WorldController.Instance.WorldData.getNeighbourTile(originTile, direction.x, direction.y);
			if(tile != null && tile.Furniture!= null && tile.Furniture.Id == furnitureId){
				queue.Enqueue(new KeyValuePair<Vector2Int, Furniture>(direction, tile.Furniture));
			}
		}

		return queue.ToArray();
	}

	public static KeyValuePair<Vector2Int, Furniture>[] GetCardinalNeighboords(string furnitureId, Tile tile){
		return GetNeighboords(furnitureId, tile, cardinalNeighbours);
	}

	public static string GetFurnitureSpriteName(string furnitureId, Tile tile){
		string spriteName = "_";
		Queue<string> queue = new Queue<string>();
		foreach (var neighbour in GetCardinalNeighboords(furnitureId, tile))
		{
			queue.Enqueue(angles[neighbour.Key]);
		}
		if(queue.Count == 8){
			spriteName = "_ALL";
		}
		else {
			spriteName += string.Join("-", queue.ToArray());
		}

		return spriteName;
	}	
}
