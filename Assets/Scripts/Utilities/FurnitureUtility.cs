using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FurnitureUtility {

	private static readonly Dictionary<Vector2Int, string> angles = new Dictionary<Vector2Int,string>(){
		{TileUtility.allNeighbours[0], "N"},
		// {1, new KeyValuePair<Vector2Int, string>(new Vector2Int(1, 1), "NE")},
		{TileUtility.allNeighbours[2], "E"},
		// {3, new KeyValuePair<Vector2Int, string>(new Vector2Int(1, -1), "SE")},
		{TileUtility.allNeighbours[4], "S"},
		// {5, new KeyValuePair<Vector2Int, string>(new Vector2Int(-1, -1), "SW")},
		{TileUtility.allNeighbours[6], "W"},
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
		return GetNeighboords(furnitureId, tile, TileUtility.cardinalNeighbours);
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
