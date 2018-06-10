using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Edge<T> {

	// cout pour suivre cet edge (ou route)
	public float cost;

	// destination pointée par cet edge (ou route)
	public Path_Node<T> node;

}
