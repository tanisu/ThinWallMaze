using System.Collections.Generic;
using UnityEngine;

public class MazeCreator
{
    const int PATH = 0;
    const int WALL = 1;
    int width, height;
    int[,] maze;
    System.Random rnd = new System.Random();
    
    string[,] hexMaze { get; set; }
    public string[,] HexMaze { get { return hexMaze; } }


    List<Cell> startCells = new List<Cell>();
    enum DIRECTION
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
    //コンストラクター
    public MazeCreator(int width, int height)
    {

        this.width = MakeOddNumber(width);
        this.height = MakeOddNumber(height);

        maze = new int[this.width, this.height];
    }

    public void GenerateMaze()
    {
        //迷路初期化：すべて壁で埋める
        InitMaze();
        Dig(1, 1);
        
        SetWallOutside();
        MazeToHex();
        PrintMaze();
        Debug.Log(hexMaze.Length);
        // return hexMaze;
    }


    void InitMaze()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    //外周をPATHに設定
                    maze[x, y] = PATH;
                }
                else
                {
                    //y,x座標をWALLに設定
                    maze[x, y] = WALL;
                }
            }
        }

    }

    //穴掘りスタート地点を決める
    (int, int) SelectStartPoint()
    {
        System.Random rnd = new System.Random();
        int y = rnd.Next(1, height - 1);
        int x = rnd.Next(1, width - 1);
        if (y % 2 == 0)
        {
            y--;
        }
        if (x % 2 == 0)
        {
            x--;
        }
        return (x, y);
    }

    void SetWallOutside()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    maze[x, y] = WALL;
                }
            }
        }
    }

    void Dig(int _x, int _y)
    {
        while (true)
        {
            List<DIRECTION> directions = new List<DIRECTION>();
            if (maze[_x, _y - 1] == WALL && maze[_x, _y - 2] == WALL)
                directions.Add(DIRECTION.UP);
            if (maze[_x + 1, _y] == WALL && maze[_x + 2, _y] == WALL)
                directions.Add(DIRECTION.RIGHT);
            if (maze[_x, _y + 1] == WALL && maze[_x, _y + 2] == WALL)
                directions.Add(DIRECTION.DOWN);
            if (maze[_x - 1, _y] == WALL && maze[_x - 2, _y] == WALL)
                directions.Add(DIRECTION.LEFT);

            if (directions.Count == 0) break;

            SetPath(_x, _y);
            int directionIndex = rnd.Next(directions.Count);
            switch (directions[directionIndex])
            {
                case DIRECTION.UP:
                    SetPath(_x, --_y);
                    SetPath(_x, --_y);
                    break;
                case DIRECTION.RIGHT:
                    SetPath(++_x, _y);
                    SetPath(++_x, _y);
                    break;
                case DIRECTION.DOWN:
                    SetPath(_x, ++_y);
                    SetPath(_x, ++_y);
                    break;
                case DIRECTION.LEFT:
                    SetPath(--_x, _y);
                    SetPath(--_x, _y);
                    break;
            }
        }
        Cell cell = GetStartCell();
        if (cell != null)
        {
            Dig(cell.x, cell.y);
        }
    }
    void SetPath(int _x, int _y)
    {
        maze[_x, _y] = PATH;
        if (_x % 2 == 1 && _y % 2 == 1)
        {
            startCells.Add(new Cell() { x = _x, y = _y });
        }
    }

    Cell GetStartCell()
    {
        if (startCells.Count == 0) return null;
        int index = rnd.Next(startCells.Count);
        Cell startCell = startCells[index];
        startCells.RemoveAt(index);
        return startCell;
    }

    void MazeToHex()
    {

        hexMaze = new string[(width - 1) / 2, (height - 1) / 2];
        for (int y = 1; y < height - 1; y += 2)
        {
            for (int x = 1; x < width - 1; x += 2)
            {
                int cellValue = 0;
                // 上のセルを確認
                if (y > 0 && maze[y - 1, x] == WALL) cellValue += 1;
                // 右のセルを確認
                if (x < width - 1 && maze[y, x + 1] == WALL) cellValue += 2;
                // 下のセルを確認
                if (y < height - 1 && maze[y + 1, x] == WALL) cellValue += 4;
                // 左のセルを確認
                if (x > 0 && maze[y, x - 1] == WALL) cellValue += 8;
                hexMaze[(x - 1) / 2, (y - 1) / 2] = cellValue.ToString("X");
            }
        }
    }

    void PrintMaze()
    {
        for (int y = 0; y < height; y++)
        {
            string line = "";
            for (int x = 0; x < width; x++)
            {
                line += maze[x, y] == WALL ? "■" : "□";
            }
            //Debug.Log(line);
        }
        for (int y = 0; y < height / 2; y++)
        {
            string line = "";
            for (int x = 0; x < width / 2; x++)
            {
                line += hexMaze[x, y];
            }
            Debug.Log(line);
        }
    }
    int MakeOddNumber(int value)
    {
        if(value % 2 == 0)
        {
            value += 1;
        }
        return value;
    }
}

public class Cell
{
    public int x, y;
}
