using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteGetter {
	private static readonly Dictionary<int, KeyValuePair<Vector2Int, string>> angles = new Dictionary<int, KeyValuePair<Vector2Int,string>>(){
		{0, new KeyValuePair<Vector2Int, string>(new Vector2Int(0, 1), "N")},
		{1, new KeyValuePair<Vector2Int, string>(new Vector2Int(1, 1), "NE")},
		{2, new KeyValuePair<Vector2Int, string>(new Vector2Int(1, 0), "E")},
		{3, new KeyValuePair<Vector2Int, string>(new Vector2Int(1, -1), "SE")},
		{4, new KeyValuePair<Vector2Int, string>(new Vector2Int(0, -1), "S")},
		{5, new KeyValuePair<Vector2Int, string>(new Vector2Int(-1, -1), "SW")},
		{6, new KeyValuePair<Vector2Int, string>(new Vector2Int(-1, 0), "W")},
		{7, new KeyValuePair<Vector2Int, string>(new Vector2Int(-1, 1), "NW")},
	};

	public static string GetInstalledObjectSpriteName(int x, int y, string objectType){
		string spriteName = "_";
		Queue<string> queue = new Queue<string>();
		for (int i = 0; i < 8; i++)
		{
			Tile tile = getTileFromPositionAndVector(x, y, angles[i].Key);
			if(tile != null && tile.InstalledObject != null && tile.InstalledObject.Id == objectType){
				queue.Enqueue(angles[i].Value);
			}
		}
		if(queue.Count == 8){
			spriteName = "_ALL";
		}
		else {
			spriteName += string.Join("-", queue.ToArray());
		}
		Debug.Log(spriteName);

		return spriteName;
	}

	private static Tile getTileFromPositionAndVector(int x, int y, Vector2Int vector){
		return  WorldController.Instance.World.getTileAt(x + vector.x, y + vector.y);
	}
	
}
