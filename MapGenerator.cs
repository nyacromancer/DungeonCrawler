using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width, height, divideBy, sectorsToFillPercentage, roomSizeMin, roomSizeMax;
    public int[,] map;
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
        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
    }

    private void GenerateRooms()
    {
        for (int x = 0; x < width / divideBy; x++)
        {
            for (int y = 0; y < height / divideBy; y++)
            {
                if (UnityEngine.Random.Range(0, 101) < sectorsToFillPercentage)

                {
                    GenerateRandomRoom(x * divideBy, (x + 1) * divideBy, y * divideBy, (y + 1) * divideBy);
                }
            }
        }
    }

    private bool GenerateRandomRoom(int leftEdge, int rightEdge, int botEdge, int topEdge)
    {
        int horizontalSpace = rightEdge - leftEdge;
        int verticalSpace = topEdge - botEdge;
        if (horizontalSpace < roomSizeMin || verticalSpace < roomSizeMin)
        {
            Debug.Log(string.Format("leftEdge {0},rightEdge {1},botEdge {2},topEdge {3}", leftEdge, rightEdge, botEdge, topEdge));
            return false;
        }
        int horizontalSize = UnityEngine.Random.Range(roomSizeMin, Math.Min(horizontalSpace, roomSizeMax));
        int verticalSize = UnityEngine.Random.Range(roomSizeMin, Math.Min(verticalSpace, roomSizeMax)); ;
        int horizontalOffset = UnityEngine.Random.Range(0, horizontalSpace - horizontalSize); ;
        int verticalOffset = UnityEngine.Random.Range(0, verticalSpace - verticalSize);
        for (int x = leftEdge + horizontalOffset; x <= leftEdge + horizontalSize + horizontalOffset; x++)
        {
            for (int y = botEdge + verticalOffset; y <= botEdge + verticalSize + verticalOffset; y++)
            {
                map[x, y] = 1;
            }
        }
        return true;
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

    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (CalculateNeighbors(x, y) > 4) map[x, y] = 1;
            }
        }
    }

    public int CalculateNeighbors(int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            if (x + i < 0 || x + i > width - 1) continue;
            for (int ii = -1; ii <= 1; ii++)
            {
                if (y + ii < 0 || y + ii > height - 1) continue;
                if (map[x + i, y + ii] != 0) count++;
            }
        }
        return count;
    }

    public int CalculateNeighboringRooms(int x, int y)
    {
        for (int i = 0; i < width / divideBy; i++)
        {
            for (int ii = 0; ii < height / divideBy; ii++)
            {
                //do stuff to each sector
            }
        }
        return 0;
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 0) ? Color.white : Color.black;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
