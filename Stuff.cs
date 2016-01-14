using UnityEngine;

public class Stuff : MonoBehaviour
{
    public bool IsEven(int num)
    {
        return num % 2 == 0;
    }

    public int RoundToOdd(int num, int min = 0, int max = 0)
    {
        if (IsEven(num)) return num;               // return num unless it's even
        if (min == max || num++ < max) return num++; // round up if it boundaries are not set or if upper boundary allows it
        return num--;                               //otherwise round down
    }

    public float DistanceBetween(int x1, int y1, int x2, int y2)
    {
        return Mathf.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
    }
}
