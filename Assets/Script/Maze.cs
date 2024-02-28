using System;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{

    MazeCreator maze;
    public GameObject groundPrefab, cornerPrefab;

    public GameObject[] wallPrafabs;
    //public Player playerPrefab;
    [SerializeField] int mapSizeX, mapSizeY;
    int width, height;
    float tileSize;
    Vector3 mapCenter;
    int[,] map;
    string[,] hexMapData;
    int[,] binaryMaze;
    enum DIRECTION
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
    void Start()
    {

        maze = new MazeCreator(mapSizeX, mapSizeY);
        maze.GenerateMaze();
        //map = maze.GenerateMaze();

        //width = map.GetLength(0);
        //height = map.GetLength(1);
        /*フィールドの初期化*/
        hexMapData = maze.HexMaze;
        
        width = hexMapData.GetLength(0);
        height = hexMapData.GetLength(1);
        
        binaryMaze = new int[width,height];

        ////メソッド実行
        ConvertHexToBinary();
        PlaceTiles();
    }

    void ConvertHexToBinary()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                string hexValue = hexMapData[x, y];//エラー
                binaryMaze[x, y] = Convert.ToInt32(hexValue, 16);
            }
        }
    }

    void PlaceTiles()
    {
        tileSize= groundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        mapCenter = new Vector3(width * tileSize/ 2,height * tileSize/ 2 - (tileSize/ 2), 0);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 position = GetWorldPositionFromCell(x, y);
                Instantiate(groundPrefab, position, Quaternion.Euler(0, 0, 0.0f), transform);

                int cellValue = binaryMaze[x, y];
                Instantiate(groundPrefab, position, Quaternion.Euler(0, 0, 0.0f), transform);
                Instantiate(cornerPrefab, position, Quaternion.Euler(0, 0, 0.0f), transform);
                if ((cellValue & 1) != 0) InstantiateWall(position, DIRECTION.UP);
                if ((cellValue & 2) != 0) InstantiateWall(position, DIRECTION.RIGHT);
                if ((cellValue & 4) != 0) InstantiateWall(position, DIRECTION.DOWN);
                if ((cellValue & 8) != 0) InstantiateWall(position, DIRECTION.LEFT);
            }
        }
    }
    Vector3 GetWorldPositionFromCell(int x, int y)
    {
        Vector3 pos = new Vector3(x * tileSize, (height - 1 - y) * tileSize, 0) - mapCenter;
        return pos;
    }
    void InstantiateWall(Vector3 postiosn, DIRECTION dir)
    {
        Instantiate(wallPrafabs[(int)dir], postiosn, Quaternion.Euler(0, 0, 0), transform);
    }
}
