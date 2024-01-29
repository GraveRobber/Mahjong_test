using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEvent : MonoBehaviour
{
    public static event Action<Tile> TileClikedEvent;

    public static void InvokeTileClikedEvent(Tile tile)
    {
        TileClikedEvent?.Invoke(tile);
    }
}
