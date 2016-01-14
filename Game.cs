using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    private CaveGenerator cavegen;
    private int[,] map;
    private SpriteSelector spriteselect;

    // Use this for initialization
    private void Start()
    {
        map = cavegen.GenerateMap();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
