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

	private static KeyValuePair<Vector2Int, Furniture>[] GetNeighboords(Furniture furn, Vector2Int[] directionToGet){
		Queue<KeyValuePair<Vector2Int, Furniture>> queue = new Queue<KeyValuePair<Vector2Int, Furniture>>();

		foreach (Vector2Int direction in directionToGet)
		{
			Tile tile = getNeighbourTile(furn, direction);
			if(tile != null && tile.Furniture!= null && tile.Furniture.Id == furn.Id){
				queue.Enqueue(new KeyValuePair<Vector2Int, Furniture>(direction, tile.Furniture));
			}
		}

		return queue.ToArray();
	}

	public static KeyValuePair<Vector2Int, Furniture>[] GetCardinalNeighboords(Furniture furn){
		return GetNeighboords(furn, cardinalNeighbours);
	}

	public static string GetFurnitureSpriteName(Furniture furn){
		string spriteName = "_";
		Queue<string> queue = new Queue<string>();
		foreach (var neighbour in GetCardinalNeighboords(furn))
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

	private static Tile getNeighbourTile(Furniture furn, Vector2Int vector){
		return  WorldController.Instance.WorldData.getTileAt(furn.MasterTile.X + vector.x, furn.MasterTile.Y + vector.y);
	}
	
}
