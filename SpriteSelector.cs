using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    public List<Sprite> sprites;

    // Use this for initialization
    private void Start()
    {
        sprites = new List<Sprite>(sprites);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
