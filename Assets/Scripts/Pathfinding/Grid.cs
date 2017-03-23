using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;

	private float nodeDiameter;
	private int numNodesX, numNodesY;
	private Node[,] grid;

	public void Start()
	{
		nodeDiameter = nodeRadius * 2;

		numNodesX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		numNodesY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);
	}

	public void CreateGrid ()
	{
		grid = new Node[numNodesX, numNodesY];
		Vector3 bottomLeft = transform.position + Vector3.left * gridWorldSize.x / 2 + Vector3.back * gridWorldSize.y;

		for (int x = 0; x < numNodesX; ++x) {
			for (int y = 0; y < numNodesY; ++y) {
				Vector3 worldPoint = bottomLeft + Vector3.right * (nodeDiameter * x + nodeRadius) + Vector3.forward * (nodeDiameter * y + nodeRadius);
				bool walkable = !Physics.CheckSphere (worldPoint, nodeRadius, unwalkableMask);

				grid [x, y] = new Node (walkable, worldPoint);
			}
		}
	}
}
