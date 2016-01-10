using System;
using System.Collections;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    public int size, fillPercentage, smoothTimes, smoothTreshold;
    private int[,] map;

    // Use this for initialization
    private void Start()
    {
        map = new int[size, size];
        GenerateMap();
        for (int i = 0; i < smoothTimes; i++)
        {
            SmoothMap();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GenerateMap();
            for (int i = 0; i < smoothTimes; i++)
            {
                SmoothMap();
            }
        }
    }

    private void GenerateMap()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (x == 0 || x == size - 1 || y == 0 || y == size - 1)
                {
                    map[x, y] = 1;
                }
                else {
                    map[x, y] = (UnityEngine.Random.Range(0, 100) < fillPercentage) ? 1 : 0;
                }
            }
        }
    }

    private void SmoothMap()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (CalculateNeighbors(x, y) > smoothTreshold)
                    map[x, y] = 1;
                else if (CalculateNeighbors(x, y) < smoothTreshold)
                    map[x, y] = 0;
            }
        }
    }

    public int CalculateNeighbors(int x, int y)
    {
        int wallCount = 0;
        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < size && neighbourY >= 0 && neighbourY < size)
                {
                    if (neighbourX != x || neighbourY != y)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Gizmos.color = (map[x, y] == 0) ? Color.white : Color.black;
                    Vector3 pos = new Vector3(-size / 2 + x + .5f, -size / 2 + y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
