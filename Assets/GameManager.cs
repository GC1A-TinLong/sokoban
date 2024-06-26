using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject clearText;
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject wallPrefab;
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
        if (moveTo.y < 1 || moveTo.y >= field.GetLength(0) - 1) { return false; }
        if (moveTo.x < 1 || moveTo.x >= field.GetLength(1) - 1) { return false; }

        if (field[moveTo.y, moveTo.x]?.tag == "Box")
        {
            var offset = moveTo - moveFrom;
            bool result = MoveNumber(moveTo, moveTo + offset);

            if (!result) { return false; }
        }

        GameObject playerOrBox = field[moveFrom.y, moveFrom.x];
        Move move = playerOrBox.GetComponent<Move>();
        move.MoveTo(new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0));
        // Moving field data
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;

        return true;
    }

    void ResetScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            map = new int[,]
        {
            { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
            { 4, 0, 0, 2, 0, 0, 0, 0, 0, 4 },
            { 4, 3, 0, 0, 0, 0, 2, 3, 0, 4 },
            { 4, 0, 0, 0, 0, 1, 2, 2, 0, 4 },
            { 4, 0, 0, 2, 0, 0, 0, 0, 0, 4 },
            { 4, 0, 0, 0, 0, 0, 0, 2, 0, 4 },
            { 4, 0, 3, 0, 0, 0, 3, 0, 0, 4 },
            { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
        };
        }
    }

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                GameObject obj = field[y, x];

                if (obj?.tag == "Player")    // Hoge?.hoge == Hoge (null check)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(-1, -1);  // player not found
    }

    bool IsCleared()
    {
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }
        foreach (var g in goals)
        {
            var go = field[g.y, g.x];
            if (go == null || go.tag != "Box") { return false; }
        }
        return true;
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
        clearText.SetActive(false);

        map = new int[,]
        {
            { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
            { 4, 0, 0, 2, 0, 0, 0, 0, 0, 4 },
            { 4, 3, 0, 0, 0, 0, 2, 3, 0, 4 },
            { 4, 0, 0, 0, 0, 1, 2, 2, 0, 4 },
            { 4, 0, 0, 2, 0, 0, 0, 0, 0, 4 },
            { 4, 0, 0, 0, 0, 0, 0, 2, 0, 4 },
            { 4, 0, 3, 0, 0, 0, 3, 0, 0, 4 },
            { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
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
                else if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(boxPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }
                else if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(goalPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }
                else if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(wallPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, playerPosition + Vector2Int.right);  // Vector2Int.right == (1,0)
            if (IsCleared()) { clearText.SetActive(true); }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, playerPosition + Vector2Int.left);   // Vector2Int.left == (-1.0)
            if (IsCleared()) { clearText.SetActive(true); }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, playerPosition - Vector2Int.up);
            if (IsCleared()) { clearText.SetActive(true); }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var playerPosition = GetPlayerIndex();
            MoveNumber(playerPosition, playerPosition - Vector2Int.down);
            if (IsCleared()) { clearText.SetActive(true); }
        }
    }
}