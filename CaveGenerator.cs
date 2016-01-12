using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    public int fillPercentage, smoothTimes, POI_amount, enemyAmount;
    public static int[,] map;

    private int[] entrancePos, exitPos;
    private List<int[]> reachableTiles;
    private int reachableTilesCount;

    [Range(40, 200)]
    public int size;

    // Use this for initialization
    private void Start()
    {
        map = new int[size, size];
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
        map = new int[size, size];
        RandomFill();
        for (int i = 0; i < smoothTimes; i++)
        {
            SmoothMap();
        };
        PlaceStuff();
    }

    private void PlaceStuff()
    {
        entrancePos = PlaceEntrance();
        MarkReachableTiles(entrancePos);
        if (reachableTilesCount + 1 < (size * size) * 0.45) GenerateMap(); // run generation again if only less than 45% of the map is accesible should probably move this somewhere else
        //Debug.Log(String.Format("#{0},#{1}", reachableTiles.Count, reachableTilesCount));
        PlaceExit();
        Debug.Log(String.Format("entrance is at #{0}, exit is at #{1}", entrancePos.ToString(), exitPos.ToString()));
        for (int i = 0; i < POI_amount; i++)
        {
            PlacePOI();
        }
    }

    private void PlacePOI()
    {
    }

    private int[] GetRandomAvailableTile()
    {
        int[] tile = reachableTiles[(UnityEngine.Random.Range(0, reachableTilesCount))];
        reachableTiles.Remove(tile);
        return tile;
    }

    //place entrance and return it's position
    private int[] PlaceEntrance()
    {
        // list to store possible location of entrance
        List<int[]> avail_pos = new List<int[]>();
        for (int x = 10; x < size - 10; x++)
        {
            for (int y = 10; y < size - 10; y++)
            {
                int[] arr = { x, y };
                // if position has less than 10 walls in the radius of cells around it, add it to list
                if (CalculateNeighbors(x, y, 5) < 10) avail_pos.Add(arr);
            }
        }
        //select random location from list and return it
        int[] entrancePos = avail_pos[(UnityEngine.Random.Range(0, avail_pos.Count))];
        map[entrancePos[0], entrancePos[1]] = 2;
        return entrancePos;
    }

    //Marks tiles that can be reached from entrance point, used to avoid placing stuff in unreachable locations;
    private void MarkReachableTiles(int[] originPos)
    {
        List<int[]> runners = new List<int[]>();
        List<int[]> runners_a = new List<int[]>();
        reachableTiles = new List<int[]>();

        int x = originPos[0];
        int y = originPos[1];

        map[x + 1, y] = -1;
        map[x - 1, y] = -1;
        map[x, y + 1] = -1;
        map[x, y - 1] = -1;

        runners.Add(new int[] { x + 1, y });
        runners.Add(new int[] { x - 1, y });
        runners.Add(new int[] { x, y - 1 });
        runners.Add(new int[] { x, y + 1 });

        reachableTiles.AddRange(runners);
        reachableTilesCount = 4;

        while (runners.Count != 0)
        {
            runners_a.Clear();
            foreach (int[] runner in runners)
            {
                int leftX = runner[0] - 1;
                int rightX = runner[0] + 1;
                int botX = runner[0];
                int topX = runner[0];
                int rightY = runner[1];
                int leftY = runner[1];
                int topY = runner[1] + 1;
                int botY = runner[1] - 1;
                if (map[leftX, leftY] == 0)
                {
                    reachableTilesCount++;
                    map[leftX, leftY] = -1;
                    runners_a.Add(new int[] { leftX, leftY });
                }
                if (map[topX, topY] == 0)
                {
                    reachableTilesCount++;
                    map[topX, topY] = -1;
                    runners_a.Add(new int[] { topX, topY });
                }
                if (map[rightX, rightY] == 0)
                {
                    reachableTilesCount++;
                    map[rightX, rightY] = -1;
                    runners_a.Add(new int[] { rightX, rightY });
                }
                if (map[botX, botY] == 0)
                {
                    reachableTilesCount++;
                    map[botX, botY] = -1;
                    runners_a.Add(new int[] { botX, botY });
                }
            }
            runners = new List<int[]>(runners_a);
            reachableTiles.AddRange(runners);
        }
    }

    private void PlaceExit()
    {
        exitPos = GetRandomAvailableTile();
        map[exitPos[0], exitPos[1]] = 2;
    }

    private void RandomFill()
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
                if (CalculateNeighbors(x, y) > 4)
                    map[x, y] = 1;
                else if (CalculateNeighbors(x, y) < 4)
                    map[x, y] = 0;
            }
        }
    }

    public int CalculateNeighbors(int x, int y, int radius = 1)
    {
        int wallCount = 0;
        for (int neighbourX = x - radius; neighbourX <= x + radius; neighbourX++)
        {
            for (int neighbourY = y - radius; neighbourY <= y + radius; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < size && neighbourY >= 0 && neighbourY < size)
                {
                    if (neighbourX != x || neighbourY != y)
                    {
                        wallCount += (map[neighbourX, neighbourY] == 1) ? 1 : 0;
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
                    Gizmos.color = (map[x, y] == -1) ? Color.yellow : (map[x, y] == 2) ? Color.red : (map[x, y] == 0) ? Color.white : Color.black;
                    Vector3 pos = new Vector3(-size / 2 + x + .5f, -size / 2 + y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
