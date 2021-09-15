using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour
{
    public GridGenerator GridData;

    Queue<Vector3> queue;
    HashSet<Vector3> visited;

    Dictionary<Vector3, Vector3> cellParents;

    private void Start()
    {
        queue = new Queue<Vector3>();
        visited = new HashSet<Vector3>();
        cellParents = new Dictionary<Vector3, Vector3>();
    }

    public void Search()
    {
        ClearData();
        GridData.ClearPath();

        queue.Enqueue(GridData.StartPosition2);
        visited.Add(GridData.StartPosition2);

        while (queue.Count > 0)
        {
            var currentCell = queue.Dequeue();

            if (currentCell == GridData.EndPosition2)
            {
                GridData.VisualizePath2(cellParents);
                return;
            }

            var neighbours = GridData.GetNeighbours(currentCell);
            foreach (var neighbour in neighbours)
            {
                
                if(!visited.Contains(neighbour))
                {
                    queue.Enqueue(neighbour);
                    visited.Add(neighbour);
                    cellParents[neighbour] = currentCell;
                }
            }
        }
    }

    private void ClearData()
    {
        queue.Clear();
        visited.Clear();
        cellParents.Clear();
    }
}
