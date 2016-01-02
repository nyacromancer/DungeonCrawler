using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width, height, divideBy, sectorsToFillPercentage, roomSizeMin, roomSizeMax;
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
        for (int x = 0; x < width / divideBy; x++)
        {
            for (int y = 0; y < width / divideBy; y++)
            {
                if (UnityEngine.Random.Range(0, 101) < sectorsToFillPercentage)

                {
                    // if (!GenerateRandomRoom(x * divideBy, (x + 1) * divideBy, y * divideBy, (y + 1) * divideBy)) ;
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
