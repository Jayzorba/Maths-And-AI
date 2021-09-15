using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour
{
    public GridGenerator GridData;

    Stack<Vector3> stack;
    HashSet<Vector3> visited;

    Dictionary<Vector3, Vector3> cellParents;

    private void Start()
    {
        stack = new Stack<Vector3>();
        visited = new HashSet<Vector3>();
        cellParents = new Dictionary<Vector3, Vector3>();
    }

    public void Search()
    {
        ClearData();
        GridData.ClearPath();

        stack.Push(GridData.StartPosition);
        visited.Add(GridData.StartPosition);

        while (stack.Count > 0)
        {
            var currentCell = stack.Pop();

            if (currentCell == GridData.EndPosition)
            {
                Debug.Log("Destination Reached");
                GridData.VisualizePath(cellParents);
                return;
            }

            var neighbours = GridData.GetNeighbours(currentCell);
            foreach (var neighbour in neighbours)
            {
                if(!visited.Contains(neighbour))
                {
                    stack.Push(neighbour);
                    visited.Add(neighbour);
                    cellParents[neighbour] = currentCell;
                }
            }
        }
    }

    private void ClearData()
    {
        stack.Clear();
        visited.Clear();
        cellParents.Clear();
    }
}
