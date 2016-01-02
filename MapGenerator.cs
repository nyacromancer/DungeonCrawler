using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width, height, roomsMin, roomsMax, roomSizeMin, roomSizeMax;
    private int[,] map;
    private Stuff stuff;

    // Use this for initialization
    private void Start()
    {
        stuff = new Stuff();
        map = new int[width, height];
        GenerateMap();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        GenerateRooms();
        GenerateCorridors();
    }

    private void GenerateRooms()
    {
        int amount = UnityEngine.Random.Range(roomsMin, roomsMax);
        int fails_in_a_row = 0;
        for (int i = 0; i < amount;)
        {
            int centerX = UnityEngine.Random.Range(1 + (roomSizeMax / 2) - 1, width - (roomSizeMax / 2) - 1);
            int centerY = UnityEngine.Random.Range(1 + (roomSizeMax / 2) - 1, height - (roomSizeMax / 2) - 1);

            int roomSize = stuff.RoundToOdd(UnityEngine.Random.Range(roomSizeMin, roomSizeMax));
            int halfsSize = roomSize / 2;
            int leftwall = centerX - halfsSize;
            int rightwall = centerX + halfsSize;
            int botwall = centerY - halfsSize;
            int topwall = centerY + halfsSize;
            if (CheckEmpty(leftwall, botwall, roomSize))
            {
                map[centerY, centerX] = 3;
                Debug.Log(String.Format("room center is located at {0},{1}", centerX, centerY));
                Debug.Log(String.Format("room size is {0}", roomSize));
                Debug.Log(String.Format("left wall is at {0}, right wall is at {1}, top wall is at {2}, bottom wall is at {3}", leftwall, rightwall, topwall, botwall));
                Debug.Log(String.Format("checked for corner at {0},{1}", leftwall, botwall));
                for (int y = botwall; y <= topwall; y++)
                {
                    for (int x = leftwall; x <= rightwall; x++)
                    {
                        if (map[y, x] == 0) map[y, x] = 1;
                    }
                }
                fails_in_a_row = 0;
                i++;
            }
            else {
                if (fails_in_a_row > 10) break;
                fails_in_a_row++;
                continue;
            }
        }
    }

    private bool CheckEmpty(int cornerx, int cornery, int size)
    {
        for (int y = cornery; y <= cornery + size; y++)
        {
            for (int x = cornerx; x <= cornerx + size; x++)
            {
                if (map[y, x] != 0) return false;
            }
        }
        return true;
    }

    private void GenerateCorridors()
    {
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 3) ? Color.cyan : (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
