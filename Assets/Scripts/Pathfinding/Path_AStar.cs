using System.Collections;
using System.Collections.Generic;

public class Path_AStar {

	Queue<Tile> calculated_path = new Queue<Tile>();

	public Path_AStar(World World, Tile startTile, Tile endTile){

		Path_Node<Tile> start = World.TileGraph.Nodes[startTile], goal = World.TileGraph.Nodes[endTile];

		List<Path_Node<Tile>> closedSet = new List<Path_Node<Tile>>();
		Priority_Queue.SimplePriorityQueue<Path_Node<Tile>> openSet = new Priority_Queue.SimplePriorityQueue<Path_Node<Tile>>();
		
		Dictionary<Path_Node<Tile>,Path_Node<Tile>> cameFrom = new Dictionary<Path_Node<Tile>, Path_Node<Tile>>();

		Dictionary<Path_Node<Tile>, float> g_scores = new Dictionary<Path_Node<Tile>, float>();
		Dictionary<Path_Node<Tile>, float> f_scores = new Dictionary<Path_Node<Tile>, float>();
		foreach (Path_Node<Tile> node in World.TileGraph.Nodes.Values)
		{
			g_scores[node] = float.PositiveInfinity;
			f_scores[node] = float.PositiveInfinity;
		}
		g_scores[start] = 0;
		f_scores[start] = this.heuristic_cost_estimate(start, goal);
		openSet.Enqueue(start, f_scores[start]);


		while(openSet.Count > 0){
			Path_Node<Tile> current = openSet.Dequeue();
			if(current == goal){
				reconstructPath(cameFrom, current);
				return;
			}

			closedSet.Add(current);
			foreach (Path_Edge<Tile> edge in current.edges)
			{
				Path_Node<Tile> neighbour = edge.node;
				if(closedSet.Contains(neighbour)) continue;

				float tentative_g_score = g_scores[current] + this.dist_between(current, neighbour);

				if(openSet.Contains(neighbour) && tentative_g_score >= g_scores[neighbour] ) continue;

				g_scores[neighbour] = tentative_g_score;
				f_scores[neighbour] = tentative_g_score + heuristic_cost_estimate(current, goal);
				cameFrom[neighbour] = current;
				if(openSet.Contains(neighbour) == false){
					openSet.Enqueue(neighbour, f_scores[neighbour]);
				}
			}

		}
	}

	public Tile GetNextTile(){
		return calculated_path.Dequeue();
	}

	private float heuristic_cost_estimate(Path_Node<Tile> start, Path_Node<Tile> goal){
		return (float)System.Math.Sqrt(System.Math.Pow(start.data.X - goal.data.X, 2) + System.Math.Pow(start.data.Y - goal.data.Y, 2));
	}

	float dist_between( Path_Node<Tile> a, Path_Node<Tile> b ) {
		// Hori/Vert neighbours have a distance of 1
		if( System.Math.Abs( a.data.X - b.data.X ) + System.Math.Abs( a.data.Y - b.data.Y ) == 1 ) {
			return 1f;
		}

		// Diag neighbours have a distance of 1.41421356237	
		if( System.Math.Abs( a.data.X - b.data.X ) == 1 && System.Math.Abs( a.data.Y - b.data.Y ) == 1 ) {
			return 1.41421356237f;
		}

		// Otherwise, do the actual math.
		return (float)System.Math.Sqrt(
			System.Math.Pow(a.data.X - b.data.X, 2) +
			System.Math.Pow(a.data.Y - b.data.Y, 2)
		);

	}

	private void reconstructPath(Dictionary<Path_Node<Tile>,Path_Node<Tile>> cameFrom, Path_Node<Tile> current){
		// Queue<Path_Node<Tile>> path = new Queue<Path_Node<Tile>>();
		calculated_path.Enqueue(current.data);
		while(cameFrom.ContainsKey(current)){
			current = cameFrom[current];
			calculated_path.Enqueue(current.data);
		}

		// while(calculated_path.Count >0){
		// 	this.calculated_path.Enqueue(path.Dequeue().data);
		// }
	}

}
