using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] private float nodeRadius = 1.5f;

    private HashSet<Node> nodes = new HashSet<Node>();

    public Vector2[] FindPath(Vector2 start, Vector2 end)
    {
        Node startNode = FindNearestNodeTo(start);
        Node endNode = FindNearestNodeTo(end);

        HashSet<Node> open = new HashSet<Node>();
        open.Add(startNode);

        HashMap<Node, Node> cameFrom = new HashMap<Node, Node>();
        
        HashMap<Node, float> gScore = new HashMap<Node, float>();
        gScore.Add(startNode, 0.0f);

        HashMap<Node, float> fScore = new HashMap<Node, float>();
        fScore.Add(startNode, Heuristic(startNode, endNode));

        while(open.Count > 0)
        {
            Node current = FindNodeWithLowestFScore(open, fScore);
            if(current == endNode) return ReconstructPath(cameFrom, current);

            open.Remove(current);
            foreach(Node neighbour in current.neighbours)
            {
                float tentativeGScore = gScore[current] + (current.position - neighbour.position).sqrMagnitude;
                if(tentativeGScore < gScore[neighbour])
                {
                    if(!cameFrom.ContainsKey(neighbour)) cameFrom.Add(neighbour, current);
                    else cameFrom[neighbour] = current;

                    if(!gScore.ContainsKey(neighbour)) gScore.Add(neighbour, tentativeGScore);
                    else gScore[neighbour] = tentativeGScore;

                    if(!fScore.ContainsKey(neighbour)) fScore.Add(neighbour, gScore[neighbour] + Heuristic(neighbour, endNode));
                    else fScore[neighbour] = gScore[neighbour] + Heuristic(neighbour, endNode);

                    if(!open.Contains(neighbour)) open.Add(neighbour);
                }
            }
        }

        return null;
    }

    public void ConnectNodes(Node a, Node b)
    {
        a.neighbours.Add(b);
        b.neighbours.Add(a);
    }

    public void AddNode(Node node) => nodes.Add(node);
    public void RemoveNode(Node node) => nodes.Remove(node);
    private float Heuristic(Node n, Node end) => (n.position - end.position).sqrMagnitude;

    private Vector2[] ReconstructPath(HashMap<Node, Node> cameFrom, Node current)
    {
        List<Vector2> path = new List<Vector2>();
        path.Add(current.position);

        while(cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        return path.ToArray();
    }

    //This is slow but I didn't feel like implementing a priority queue
    private Node FindNodeWithLowestFScore(HashSet<Node> set, HashMap<Node, float> fScores)
    {
        Node lowest = set[0];
        foreach(Node node in set)
        {
            if(fScores.ContainsKey(lowest) && fScores.ContainsKey(node))
            {
                if(fScores[node] < fScores[lowest])
                {
                    lowest = node;
                }
            } else
            {
                lowest = node;
            }
        }

        return lowest;
    }

    private Node FindNearestNodeTo(Vector2 position)
    {
        Node nearest = nodes[0];
        foreach(Node node in nodes)
        {
            Vector2 d1 = position - node.position;
            Vector2 d2 = position - nearest.position;
            if(d1.sqrMagnitude < d2.sqrMagnitude) nearest = node;
        }

        return nearest;
    }

    public class Node
    {
        public Vector2 position;
        public float cost;
        public List<Node> neighbours;

        public Node(Vector2 position, float cost)
        {
            this.position = position;
            this.cost = cost;
            neighbours = new List<Node>();
        }
    }
}
