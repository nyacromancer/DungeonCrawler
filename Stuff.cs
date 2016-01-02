using System.Collections;
using UnityEngine;

public class Stuff //: MonoBehaviour
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
}
