using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameData", menuName ="Game Data")]
public class GameData : ScriptableObject
{
    public List<GameObject> gridsPrefabs = new List<GameObject>();
    public List<TileData> tileTypes = new List<TileData>();


    public Color BlockedTileColor;
}
