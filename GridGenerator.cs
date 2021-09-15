using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TileType
{
    Water,
    Land,
    Sand
}
public class Tile
{
    public Vector3 Position { get; set; }
    public TileType TileType { get; set; }
    public int Cost { get; set; }

    public Tile(Vector3 position, TileType tileType, int cost)
    {
        Position = position;
        TileType = tileType;
        Cost = cost;
    }

};
public class GridGenerator : MonoBehaviour
{
    bool isPlayerMoving = false;
    public int pathIndex;
    public int pathIndex2;
    public int pathIndex3;
    public int width = 20;
    public int depth = 20;

    public int numOfObstacles = 10;

    public int numOfEnemies;
    public Transform EnemiesHolder;
    public GameObject Enemy;
    public GameObject Enemy2;
    public GameObject Enemy3;

    public GameObject Player;
    public GameObject playerInstance;

    public GameObject Destination;
    public GameObject Destination2;
    public GameObject Destination3;

    public GameObject Obstacle;

    public GameObject Ground;
    public GameObject Tiling;
    public MeshRenderer TileColour;

    [Header("Visualize Path")]
    public Transform PathCellsHolder;
    public GameObject Path;

    [Header("Tiles")]
    public TileType tileType;
    public Tile tile;
    public Tile startingTile;

    [HideInInspector]
    public Vector3 StartPosition;
    [HideInInspector]
    public Vector3 EndPosition;  
    [HideInInspector]
    public Vector3 OriginalStartPosition;
    [HideInInspector]
    public Vector3 OriginalEndPosition;    
    [HideInInspector]
    public Vector3 StartPosition2;
    [HideInInspector]
    public Vector3 EndPosition2;  
    [HideInInspector]
    public Vector3 OriginalStartPosition2;
    [HideInInspector]
    public Vector3 OriginalEndPosition2;    
    [HideInInspector]
    public Vector3 StartPosition3;
    [HideInInspector]
    public Vector3 EndPosition3;  
    [HideInInspector]
    public Vector3 OriginalStartPosition3;
    [HideInInspector]
    public Vector3 OriginalEndPosition3;

    public HashSet<Vector3> Obstacles;
    public HashSet<Vector3> Enemies;
    public HashSet<Vector3> WalkableCells;
    public Dictionary<Tile, int> TilesOnMap;
    public Dictionary<Tile, int> WalkableTilesOnMap;
    public List<Vector3> EnemyPath;
    public List<Vector3> EnemyPath2;
    public List<Vector3> EnemyPath3;
    GameObject enemyInstance;
    Enemy enemyMain;

    public NavMeshSurface navMeshSurface;

    private void Start()
    {
        tile = new Tile(StartPosition, tileType, 1);
        Obstacles = new HashSet<Vector3>();
        Enemies = new HashSet<Vector3>();
        WalkableCells = new HashSet<Vector3>();
        TilesOnMap = new Dictionary<Tile, int>();
        WalkableTilesOnMap = new Dictionary<Tile, int>();

        GenerateGrid();

        navMeshSurface.BuildNavMesh();
    }

    public void GenerateGrid()
    {
        ClearData();
        ClearPath();

        Ground.transform.position = new Vector3(width / 2f, 0, depth / 2f);
        Ground.transform.localScale = new Vector3(width/10f, 1, depth/10f);





        PlaceObstacles();
        PlaceFloor();
      //  PlaceEnemies();
        PlaceObject(Player);
        StartPosition = PlaceObject(Enemy);
        EndPosition = PlaceObject(Destination);
        StartPosition2 = PlaceObject(Enemy2);
        EndPosition2 = PlaceObject(Destination2);
        StartPosition3 = PlaceObject(Enemy3);
        EndPosition3 = PlaceObject(Destination3);
       // OriginalStartPosition = StartPosition;
       // OriginalEndPosition = EndPosition ;

        LocateWalkableCells();
    }

    private void PlaceFloor()
    {
        var rowsToPlace = 20;
        var columnsToPlace = 20;
      
        for(int i = 0; i<21;i++)
        {
            for (int j = 0; j < 21; j++)
            {

                var randomType = UnityEngine.Random.Range(1, 4);
                var positionX = i;
                var positionZ = j;

                var cellPosition = new Vector3(positionX, 0f, positionZ);
                //var objectPosition = cellPosition;
                ////objectPosition.y = Tiling.transform.position.y;

                var tileTemp =Instantiate(Tiling, cellPosition, Quaternion.identity, transform);
                TileColour = tileTemp.GetComponent<MeshRenderer>();
                TilesOnMap.Add(tile = new Tile(cellPosition, RandomTileType(randomType), AssignCost(randomType)),tile.Cost);

                if(StartPosition == tile.Position)
                {
                    startingTile = tile;
                }
                rowsToPlace--;
                columnsToPlace--;
            }
        }

    }

    private TileType RandomTileType(int randomNum)
    {

        switch (randomNum)
        {
            case 1:
                tileType = TileType.Land;
                tile.Cost = 1;
                TileColour.material.color = Color.green;
                break;
            case 2:
                tileType = TileType.Sand;
                tile.Cost = 3;
                TileColour.material.color = Color.yellow;
                break;
            case 3:
                tileType = TileType.Water;
                tile.Cost = 10;
                TileColour.material.color = Color.blue;
                break;
        }

        return tileType;
    }

    private int AssignCost(int randomNum)
    {
        int cost =1;
        switch (randomNum)
        {
            case 1:
                cost = 0;
                break;
            case 2:
                cost = 50;
                break;
            case 3:
                cost = 1000;
                break;
        }
        
        return cost;
    }

    private Vector3 PlaceObject(GameObject gameObjectToPlace)
    {
        while (true)
        {
            var positionX = UnityEngine.Random.Range(1, width);
            var positionZ = UnityEngine.Random.Range(1, depth);

            var cellPosition = new Vector3(positionX, 0, positionZ);

            if (!IsCellOccupied(cellPosition))
            {

                var objectPosition = cellPosition;
                objectPosition.y = gameObjectToPlace.transform.position.y;

                if(gameObjectToPlace.name == "Player")
                {
                    playerInstance = Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                }

                else if(gameObjectToPlace.name == "Enemy")
                {
                    enemyInstance = Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                    //enemyMain = enemyInstance.GetComponent<Enemy>();
                    OriginalStartPosition = objectPosition;
                    Enemies.Add(cellPosition);
                }
                else if(gameObjectToPlace.name == "Enemy2")
                {
                    enemyInstance = Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                   // enemyMain = enemyInstance.GetComponent<Enemy>();
                    OriginalStartPosition2 = objectPosition;
                    Enemies.Add(cellPosition);
                }
                else if(gameObjectToPlace.name == "Enemy3")
                {
                    enemyInstance = Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                    // enemyMain = enemyInstance.GetComponent<Enemy>();
                    OriginalStartPosition3 = objectPosition;
                    Enemies.Add(cellPosition);
                }
                else if(gameObjectToPlace.name == "Destination")
                {
                    Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                    OriginalEndPosition = objectPosition;
                }
                else if (gameObjectToPlace.name == "Destination2")
                {
                    Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                    OriginalEndPosition2 = objectPosition;
                }
                else if (gameObjectToPlace.name == "Destination3")
                {
                    Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                    OriginalEndPosition3 = objectPosition;
                }
                else
                {
                    Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                }

                return cellPosition;
            }
        }
    }
    public void LocateWalkableCells()
    {
  
        foreach (Tile tile in TilesOnMap.Keys)
        {
            var currentCell = tile.Position;
            if(!IsCellOccupied(currentCell))
            {
                WalkableCells.Add(currentCell);
                WalkableTilesOnMap.Add(tile, tile.Cost);
            }
        }
    }
    private void PlaceObstacles()
    {

        var obstaclesToGenerate = numOfObstacles;
        while (obstaclesToGenerate > 0)
        {
            var positionX = UnityEngine.Random.Range(1, width);
            var positionZ = UnityEngine.Random.Range(1, depth);

            var cellPosition = new Vector3(positionX, 0f, positionZ);

            if (!IsCellOccupied(cellPosition))
            {
                Obstacles.Add(cellPosition);

                var objectPosition = cellPosition;
                objectPosition.y = Obstacle.transform.position.y;

                Instantiate(Obstacle, objectPosition, Quaternion.identity, transform);
                obstaclesToGenerate--;
            }


        }
    }


    public List<Vector3> GetNeighbours(Vector3 currentCell)
    {
        var neighbours = new List<Vector3>()
        {
            new Vector3(currentCell.x, 0 , currentCell.z + 1), //Up
            new Vector3(currentCell.x, 0 , currentCell.z-1),   //Down
            new Vector3(currentCell.x -1, 0, currentCell.z),    //Left
            new Vector3(currentCell.x + 1, 0, currentCell.z), //Right
        };

        var walkableNeighbours = new List<Vector3>();
        foreach(var neighbour in neighbours)
        {
            if(!IsCellOccupied(neighbour) && IsInLevelBounds(neighbour))
            {
                walkableNeighbours.Add(neighbour);
            }
        }

        return walkableNeighbours;

    }

    private bool IsInLevelBounds(Vector3 cell)
    {
        if(cell.x > 0 && cell.x < width && cell.z > 0 && cell.z < depth)
        {
            return true;
        }
        return false;
    }

    public void VisualizePath(Dictionary<Vector3,Vector3> cellParents)
    {
        var path = new List<Vector3>();

        var current = cellParents[EndPosition];

        path.Add(EndPosition);

        while(current != StartPosition)
        {
            path.Add(current);
            current = cellParents[current];
        }

        //for (int i = 1; i < path.Count; i++)
        //{
        //    var pathCellPosition = path[i];
        //    pathCellPosition.y = Path.transform.position.y;
        //    Instantiate(Path, pathCellPosition, Quaternion.identity, PathCellsHolder);

        //}

        MoveEnemy(path);
    }
    
    public void VisualizePath2(Dictionary<Vector3,Vector3> cellParents)
    {
        var path2 = new List<Vector3>();

        var current = cellParents[EndPosition2];

        path2.Add(EndPosition2);

        while(current != StartPosition2)
        {
            path2.Add(current);
            current = cellParents[current];
        }

        //for (int i = 1; i < path2.Count; i++)
        //{
        //    var pathCellPosition = path2[i];
        //    pathCellPosition.y = Path.transform.position.y;
        //    Instantiate(Path, pathCellPosition, Quaternion.identity, PathCellsHolder);

        //}

        MoveEnemy2(path2);
    }
    
    public void VisualizePath3(Dictionary<Vector3,Vector3> cellParents)
    {
        var path3 = new List<Vector3>();

        var current = cellParents[EndPosition3];

        path3.Add(EndPosition3);

        while(current != StartPosition3)
        {
            path3.Add(current);
            current = cellParents[current];
        }

        //for (int i = 1; i < path3.Count; i++)
        //{
        //    var pathCellPosition = path3[i];
        //    pathCellPosition.y = Path.transform.position.y;
        //    Instantiate(Path, pathCellPosition, Quaternion.identity, PathCellsHolder);

        //}

        MoveEnemy3(path3);
    }

    private void MoveEnemy(List<Vector3> path)
    {
        //isPlayerMoving = true;
        EnemyPath = path;
        pathIndex = EnemyPath.Count - 1;
    }   
    private void MoveEnemy2(List<Vector3> path2)
    {
        //isPlayerMoving = true;
        EnemyPath2 = path2;
        pathIndex2 = EnemyPath2.Count - 1;
    } 
    private void MoveEnemy3(List<Vector3> path3)
    {
        //isPlayerMoving = true;
        EnemyPath3 = path3;
        pathIndex3 = EnemyPath3.Count - 1;
    }


    private bool IsCellOccupied(Vector3 cellPosition)
    {
        if(Obstacles.Contains(cellPosition)||Enemies.Contains(cellPosition))
        {
            return true;
        }
        return false;
    }

    private void ClearData()
    {
        DeleteChildren(transform);
        Obstacles.Clear();
        Enemies.Clear();
        WalkableCells.Clear();
    }

    public void ClearPath()
    {
        DeleteChildren(PathCellsHolder);
    }
    private void DeleteChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
