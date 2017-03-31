using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public List<Node> path;

	public int MaxSize { get { return numNodesX * numNodesY; } }

	private float nodeDiameter;
	private int numNodesX, numNodesY;
	private Node[,] grid;

	public void Awake()
	{
		nodeDiameter = nodeRadius * 2f;

		numNodesX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		numNodesY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);

		CreateGrid ();
	}

	public void CreateGrid ()
	{
		float EPSILON = 0.000000015f;
		grid = new Node[numNodesX, numNodesY];
		Vector3 bottomLeft = transform.position + Vector3.left * gridWorldSize.x / 2f + Vector3.back * gridWorldSize.y / 2f;

		for (int x = 0; x < numNodesX; ++x) {
			for (int y = 0; y < numNodesY; ++y) {
				Vector3 worldPoint = bottomLeft + Vector3.right * (nodeDiameter * x + nodeRadius) + Vector3.forward * (nodeDiameter * y + nodeRadius);
				bool walkable = !Physics.CheckSphere (worldPoint, nodeRadius - EPSILON, unwalkableMask);

				grid [x, y] = new Node (walkable, worldPoint, x, y);
			}
		}
	}

	public List<Node> GetNeighbors(Node node)
	{
		List<Node> neighbors = new List<Node> ();

		for (int x = -1; x <= 1; ++x) {
			for (int y = -1; y <= 1; ++y) {
				if (Mathf.Abs(x) == Mathf.Abs(y))
					continue;

				int checkX = node.gridX + x, checkY = node.gridY + y;

				if (checkX >= 0 && checkX < numNodesX && checkY >= 0 && checkY < numNodesY)
					neighbors.Add (grid [checkX, checkY]);
			}
		}

		return neighbors;
	}

	public Node NodeFromWorldPoint(Vector3 worldPoint)
	{
		float percentX = (worldPoint.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPoint.z + gridWorldSize.y / 2) / gridWorldSize.y;

		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt((numNodesX - 1) * percentX);
		int y = Mathf.RoundToInt((numNodesY - 1) * percentY);

		return grid [x, y];
	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3 (gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null) {
			foreach (Node node in grid) {
				Gizmos.color = node.walkable ? Color.white : Color.red;
				if (path != null) {
					if (path.Contains (node))
						Gizmos.color = Color.green;
				}
				Gizmos.DrawCube (node.worldPos, Vector3.one * (nodeDiameter - nodeRadius / 5));
			}
		}
	}
}
