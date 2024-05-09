using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    int[,] map;

    /* int GetPlayerIndex()
     {
         for (int i = 0; i < map.Length; i++)
         {
             if (map[i] == 1)
             {
                 return i;
             }
         }
         return -1;
     }
     bool MoveNumber(int number, int moveFrom, int moveTo)
     {
         //restricting movement between array
         if (moveTo < 0 || moveTo >= map.Length) { return false; }

         if (map[moveTo] == 2)//when destination is box
         {
             //indicate moving left or right (-1 / +1)
             int offset = moveTo - moveFrom;
             //if able to move, move 2 to left or right
             bool success = MoveNumber(2, moveFrom, moveTo + offset);
             //if box is unable to move, player is also unable to move
             if (!success) { return false; }
         }
         map[moveTo] = number;
         map[moveFrom] = 0;
         return true;
     }
     void PrintArray()
     {
         string debugText = "";
         for (int i = 0; i < map.Length; i++)
         {
             debugText += map[i].ToString() + ",";
         }
         Debug.Log(debugText);
     }*/

    // Start is called before the first frame update
    void Start()
    {
        map = new int[,]
        {
            { 0, 0, 0, 2, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0 },
        };
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

    // Update is called once per frame
    void Update()
    {
        /*//Move Right
        if (Input.GetKeyDown(KeyCode.D))
        {
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex + 1);

            PrintArray();
        }
        //Move Left
        if (Input.GetKeyDown(KeyCode.A))
        {
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex - 1);

            PrintArray();
        }*/
    }
}
