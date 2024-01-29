using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TileData", menuName ="Tile Data")]
public class TileData : ScriptableObject
{
    public int tileTypeID;
    public Sprite tileSprite;

    public ParticleSystem tileTypeVFX;

}
