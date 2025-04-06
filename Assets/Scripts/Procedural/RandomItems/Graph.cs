using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private static List<Vector2Int> neighbourse4directions = new List<Vector2Int>
    {
        new Vector2Int(0,1), //up
        new Vector2Int(1,0), //right 
        new Vector2Int(0,-1), //down
        new Vector2Int(-1,0), //left 
    };

    private static List<Vector2Int> neighbourse8directions = new List<Vector2Int>
    {
        new Vector2Int(0,1), //up
        new Vector2Int(1,0), //right
        new Vector2Int(0, -1), //down
        new Vector2Int(-1,0), //left
        new Vector2Int(1,1), //diagonal
        new Vector2Int(1,-1), //diagonal
        new Vector2Int (-1,1), // diagonal
        new Vector2Int (-1,-1) //diagonal 

    };
    List<Vector2Int> graph;

    public Graph(IEnumerable<Vector2Int> vertices)
    {
        graph = new List<Vector2Int>(vertices);
    }
    public List<Vector2Int> GetNeighbours4Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, neighbourse4directions);
    }

    public List<Vector2Int> GetNeighbours8Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, neighbourse8directions);
    }

    private List<Vector2Int> GetNeighbours(Vector2Int startPosition, List<Vector2Int> neighboursOffsetList)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        foreach (var neighboiurDirection in neighboursOffsetList)
        {
            Vector2Int potencialNeoghbour = startPosition + neighboiurDirection;
            if(graph.Contains(potencialNeoghbour))
                neighbours.Add(potencialNeoghbour);
        }
        return neighbours;
    }

}
