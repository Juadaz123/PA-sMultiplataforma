using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform seeker;
    [SerializeField] private Transform target;
    
    private Grid _grid;
    
    [HideInInspector] public List<Vector3> finalPath;
    
    private void Awake() => _grid = GetComponent<Grid>();

    private void FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        Node startNode = _grid.NodeFromWorldPoint(startPosition);
        Node targetNode = _grid.NodeFromWorldPoint(endPosition);
        
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        
        openSet.Add(startNode);
        
        while(openSet.Count > 0) // Mientras tengamos candidatos por explorar (vecinos) seguimos
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                {
                    if(openSet[i].hCost < currentNode.hCost) currentNode = openSet[i];
                }
            }
            
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in _grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;
                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    if(!openSet.Contains(neighbour)) openSet.Add(neighbour);
                }
            }
        }
    }

    private int GetDistance(Node a, Node b)
    {
        int distanceX = Mathf.Abs(a.gridX - b.gridX);
        int distanceY = Mathf.Abs(a.gridY - b.gridY);
        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
    
    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    private void RetracePath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode; // Cuando llegamos al final

        while (currentNode != startNode) // Mientras a un no lleguemos al inicio
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();

        _grid.path = path;
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}