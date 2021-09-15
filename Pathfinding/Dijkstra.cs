using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Dijkstra : MonoBehaviour
{
    public GridGenerator GridData;

    SimplePriorityQueue<Vector3, int> priorityQueue;
    HashSet<Vector3> visited;
    public Dictionary<Tile, int> costsTemp;
    public Dictionary<Vector3, int> costsTrue;
    Dictionary<Vector3, Vector3> cellParents;

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
        var distances = GridData.WalkableCells.ToDictionary(x => x, x => int.MaxValue);
        costsTemp = GridData.WalkableTilesOnMap;
        distances[GridData.StartPosition3] = 0;

        foreach (Tile tile in costsTemp.Keys)
        {
            costsTrue[tile.Position] = tile.Cost;
            //distances[tile.Position] = distances[tile.Position]+tile.Cost;
        }


        ClearData();
        GridData.ClearPath();

 

        priorityQueue.Enqueue(GridData.StartPosition3, 0);
        visited.Add(GridData.StartPosition3);

        while (priorityQueue.Count > 0)
        {
            var currentCell = priorityQueue.Dequeue();

            if (currentCell == GridData.EndPosition3)
            {
                GridData.VisualizePath3(cellParents);
                return;
            }

            var neighbours = GridData.GetNeighbours(currentCell);
            foreach (var neighbour in neighbours)
            {


                if (!visited.Contains(neighbour))
                {

                    var distance = distances[currentCell] + 1;



                        if (distance < distances[neighbour])
                        {
                            distances[neighbour] = distance;

                            priorityQueue.Enqueue(neighbour, distance);
                            visited.Add(neighbour);
                            cellParents[neighbour] = currentCell;
                        }
                  // }
                }
            }
        }
    }

    private void ClearData()
    {
        priorityQueue.Clear();
        visited.Clear();
        cellParents.Clear();
        costsTemp.Clear();
    }
}
