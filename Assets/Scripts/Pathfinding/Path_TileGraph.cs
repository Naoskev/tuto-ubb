﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_TileGraph {

	Dictionary<Tile, Path_Node<Tile>> nodes = new Dictionary<Tile, Path_Node<Tile>>();

	public Path_TileGraph(World world){

		for (int x = 0; x < world.Width; x++)
		{
			for (int y = 0; y < world.Height; y++)
			{
				Tile data = world.getTileAt(x,y);
				if(data.MovementCost > 0){
					nodes.Add(data, new Path_Node<Tile>(data));
				}
			}
		}

		foreach (Tile tile in nodes.Keys)
		{
			Tile[] neighbours = tile.GetNeighbours(true);
			List<Path_Edge<Tile>> edges = new List<Path_Edge<Tile>>();
			for (int i = 0; i < 8; i++)
			{
				if(neighbours[i] != null && neighbours[i].MovementCost > 0){
					Path_Edge<Tile> edge = new Path_Edge<Tile>();
					edge.cost = neighbours[i].MovementCost;
					edge.node = this.nodes[neighbours[i]];
					edges.Add(edge);
				}
			}
			this.nodes[tile].edges = edges.ToArray();
		}
	}

}
