using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridConfig : MonoBehaviour
{
    public List<Tile> Tiles = new List<Tile>();

    public int numberOfType;
    public Vector3 GridSize;
    public float OfsetX;
    public float OfsetY;
    
}
