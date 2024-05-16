using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    int[,] map;
    GameObject instance;
    GameObject[,] field;

    /// <summary>
    /// 与えられた数字をマップ上で移動させる
    /// </summary>
    /// <param name="number">移動させる数字</param>
    /// <param name="moveFrom">元の位置</param>
    /// <param name="moveTo">移動先の位置</param>
    /// <returns>移動可能な時 true</returns>
    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }


        if (field[moveTo.y, moveTo.x]?.tag == "Box")
        {
            var offset = moveTo - moveFrom;
            bool result = MoveNumber(moveTo, moveTo + offset);

            if (!result) { return false; }
        }


        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;

        return true;

        /*if (map[moveTo] == 2)
        {
            int offset = moveTo - moveFrom; // 箱の行先を決めるための差分
            bool success = MoveNumber(2, moveTo, moveTo + offset);

            if (!success)
            {
                return false;
            }
        }   // 行先に箱がある時

        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;*/
    }

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                GameObject obj = field[y, x];

                if (obj != null && obj?.tag == "Player")    // variable"?" == xxx (null check)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(-1, -1);  // player not found
    }

    void PrintArray()
    {
        string debugText = "";

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";
            }

            debugText += "\n";
        }

        Debug.Log(debugText);
    }

    void Start()
    {
        map = new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 2, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 0, 2, 0, 0 },
            { 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 2, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };
        field = new GameObject
            [
            map.GetLength(0),
            map.GetLength(1)
            ];

        PrintArray();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(playerPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(boxPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, playerPosition + Vector2Int.right); // Vector2Int.right == (1,0)
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, playerPosition + Vector2Int.left);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, playerPosition + Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, playerPosition + Vector2Int.up);
        }
    }
}