//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Unity.VisualScripting;
//using UnityEngine;

//public class TileGrid : MonoBehaviour
//{
//    public Transform[] Layers;
//    public Color BlockedColor;
//    public float xOfset;

//    public Dictionary<Vector3, Tile> Tiles = new Dictionary<Vector3, Tile>();

//    private void Start()
//    {
        
//        foreach(Transform layer in Layers)
//        {
//            if(layer.childCount != 0)
//            {
//                for(int i=0; i < layer.childCount; i++)
//                {
//                    Tiles.Add(layer.GetChild(i).localPosition, layer.GetChild(i).GetComponent<Tile>());
//                }
//            }
//            else
//            {
//                print("no child in layer");
//            }
//        }
//        print(Tiles.Count);

//        InitGrid();
//        GridUpdate();

//    }

//    void InitGrid()
//    {
//        //var maxX = Tiles.OrderBy(pair => pair.Key.x).Last().Key.x;

//        foreach(var temp in Tiles)
//        {
//            Vector3 position = temp.Key;
//            temp.Value.tileBackSprite.sortingOrder = -(int)position.z * 100 + (int)position.x * 10 - (int)position.y*2;
//        }
                
//    }
//    void GridUpdate()
//    {
//        foreach (var tile in Tiles)
//        {
//            Vector3 tilePos = tile.Key;
//            Vector3 leftTilePos;
//            Vector3 rightTilePos;
//            if(tilePos.x - xOfset >= Tiles.OrderBy(temp => temp.Key.x).First().Key.x && tilePos.x+xOfset<=Tiles.OrderBy(temp => temp.Key.x).Last().Key.x)
//            {
//                leftTilePos = new Vector3(tilePos.x - xOfset, tilePos.y, tilePos.z);
//                rightTilePos = new Vector3(tilePos.x + xOfset, tilePos.y, tilePos.z);
//                if (Tiles[leftTilePos] != null && Tiles[rightTilePos] != null)
//                {
//                    tile.Value.tileBackSprite.color = BlockedColor;
//                }
//            }

//            //tile.Value.tileBackSprite.color = BlockedColor;
//        }
//    }

//}
