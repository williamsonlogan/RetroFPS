using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>{
	public bool walkable;
	public Vector3 worldPos;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public int fCost { get { return hCost + gCost; } }

	int heapIndex;
	public int HeapIndex { get { return heapIndex; } set { heapIndex = value; } }

	public Node parent;

	public Node (bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPos = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int CompareTo(Node otherNode)
	{
		int compare = fCost.CompareTo(otherNode.fCost);

		if (compare == 0)
			compare = hCost.CompareTo (otherNode.hCost);

		return -compare;
	}
}
