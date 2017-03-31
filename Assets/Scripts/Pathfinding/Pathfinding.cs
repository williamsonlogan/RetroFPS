using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour {

	PathRequestManager pathRequestManager;
	Grid grid;

	void Awake()
	{
		pathRequestManager = GetComponent<PathRequestManager> ();
		grid = GetComponent<Grid>();
	}

	public void StartFindPath(Vector3 startPos, Vector3 endPos)
	{
		StartCoroutine(FindPath(startPos, endPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint (startPos), targetNode = grid.NodeFromWorldPoint (targetPos);

		Heap<Node> openSet = new Heap<Node> (grid.MaxSize);
		HashSet<Node> closedSet = new HashSet<Node> ();
		openSet.Add (startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst ();
			closedSet.Add (currentNode);

			if (currentNode == targetNode) {
				pathSuccess = true;
			}

			foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
				if (!neighbor.walkable || closedSet.Contains (neighbor))
					continue;

				int newMovementCostToNeighbor = currentNode.gCost + GetDistance (currentNode, neighbor);
				if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains (neighbor)) {
					neighbor.gCost = newMovementCostToNeighbor;
					neighbor.hCost = GetDistance (neighbor, targetNode);

					neighbor.parent = currentNode;

					if (!openSet.Contains (neighbor))
						openSet.Add (neighbor);
					else
						openSet.UpdateItem (neighbor);
				}
			}
		}

		yield return null;
		if (pathSuccess) {
			waypoints = RetracePath (startNode, targetNode);
		}
		pathRequestManager.FinishedProcessingPath (waypoints, pathSuccess);
	}

	Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}

		grid.path = path;

		Vector3[] waypoints = SimplifyPath (path);
		Array.Reverse (waypoints);

		return waypoints;
	}

	Vector3[] SimplifyPath(List<Node> path)
	{
		List<Vector3> waypoints = new List<Vector3> ();
		Vector2 oldDirection = Vector2.zero;

		for (int i = 1; i < path.Count; ++i) {
			Vector2 newDirection = new Vector2 (path [i - 1].gridX - path [i].gridX, path [i - 1].gridY - path [i].gridY);
			if (newDirection != oldDirection) {
				waypoints.Add (path [i].worldPos);
			}
			oldDirection = newDirection;
		}

		return waypoints.ToArray ();
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Math.Abs (nodeA.gridX - nodeB.gridX);
		int dstY = Math.Abs (nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}
