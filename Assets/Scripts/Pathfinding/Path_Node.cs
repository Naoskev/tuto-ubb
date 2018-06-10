using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Node<T> {

	public T data;

	//Edges vers des nodes sortant du node actuel
	public Path_Edge<T>[] edges;

	public Path_Node(T data){
		this.data = data;
	}
}
