using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public GridGenerator GridData;

    SimplePriorityQueue<Vector3, int> priorityQueue;
    HashSet<Vector3> visited;

    Dictionary<Vector3, Vector3> cellParents;
    public Dictionary<Vector3, int> distances;
    public Dictionary<Tile, int> costsTemp;
    public Dictionary<Vector3, int> costsTrue;

    private void Start()
    {
        priorityQueue = new SimplePriorityQueue<Vector3, int>();
        visited = new HashSet<Vector3>();
        cellParents = new Dictionary<Vector3, Vector3>();
        costsTemp = new Dictionary<Tile, int>();
        costsTrue = new Dictionary<Vector3, int>();
    }

    public void Search()
    {

      
        distances = GridData.WalkableCells.ToDictionary(x => x, x => int.MaxValue);
        costsTemp = GridData.WalkableTilesOnMap;
        distances[GridData.StartPosition] = 0;
        costsTrue[GridData.StartPosition] = 0;

        foreach (Tile tile in costsTemp.Keys)
        {
                costsTrue[tile.Position] = tile.Cost;
        }

        Debug.Log(costsTrue.Count);
        Debug.Log(distances.Count);

        

        ClearData();
        GridData.ClearPath();

 


        priorityQueue.Enqueue(GridData.StartPosition, 0);
        visited.Add(GridData.StartPosition);

        while (priorityQueue.Count > 0)
        {
            var currentCell = priorityQueue.Dequeue();


            if (currentCell == GridData.EndPosition)
            {
                GridData.VisualizePath(cellParents);
                return;
            }

            var neighbours = GridData.GetNeighbours(currentCell);
            foreach (var neighbour in neighbours)
            {

                if (!visited.Contains(neighbour))
                {
                    var distance = distances[currentCell] +1;
                    var cost = costsTrue[currentCell]+1;
                    distance *= cost;

                    if (distance < distances[neighbour])
                    {
                        distances[neighbour] = distance;
                        //costsTrue[neighbour] = cost;

                        var fScore = distances[neighbour] + DistanceEstimate(neighbour);

                        priorityQueue.Enqueue(neighbour, fScore);
                        visited.Add(neighbour);
                        cellParents[neighbour] = currentCell;

                    }
                }
            }
        }
    }

    private int DistanceEstimate(Vector3 cell)
    {
        var x = Mathf.Pow(cell.x - GridData.EndPosition.x, 2);
        var y = Mathf.Pow(cell.y - GridData.EndPosition.y, 2);
        var z = Mathf.Pow(cell.z - GridData.EndPosition.z, 2);

        return (int)Mathf.Sqrt(x+ y+ z);
    }


    public void ClearData()
    {
        priorityQueue.Clear();
        visited.Clear();
        cellParents.Clear();
        //costsTrue.Clear();
        costsTemp.Clear();
    }


}